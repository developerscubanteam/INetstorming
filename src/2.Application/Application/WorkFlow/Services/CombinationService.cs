using Application.Dto.AvailabilityService;
using Domain.Availability;
using Domain.Common;
using Domain.Common.CancellationPolicy;
using Domain.Common.MinimumPrice;
using Domain.Common.Price;
using Infrastructure.Connectivity.Connector.Extension;


namespace Application.WorkFlow.Services
{
    public class CombinationService
    {
        /// <summary>
        /// This recive a MealPlan whit all atomic result of combination, and from the request it combine it.        
        /// </summary>
        /// <param name="mealplan"></param>
        /// <param name="availabilityQuery"></param>
        /// <param name="SumValCode">Sumvalcode es a custom function for sum Valiation Code for use in the next step of booking</param>
        /// <returns></returns>
        public static Domain.Availability.Mealplan BuildCombinations(Domain.Availability.Mealplan mealplan, AvailabilityQuery availabilityQuery, Func<IEnumerable<Combination>, string> SumValCode)
        {

            var grouped = GroupByDistributions(mealplan.Combinations, availabilityQuery);
            var combined = GetCombinations(grouped).Select(comb => SumCombinations(comb, SumValCode)).ToList();
            mealplan.Combinations = combined;
            return mealplan;
        }

        /// <summary>
        /// Combine Groups
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<T[]> GetCombinations<T>(IEnumerable<IEnumerable<T>> input)
        {
            var result = new T[input.Count()];
            var indices = new int[result.Length];
            for (int pos = 0, index = 0; ;)
            {
                for (; pos < result.Length; pos++, index = 0)
                {
                    indices[pos] = index;
                    result[pos] = input.ElementAt(pos).ElementAt(index);
                }
                yield return result;
                do
                {
                    if (pos == 0) yield break;
                    index = indices[--pos] + 1;
                }
                while (index >= input.ElementAt(pos).Count());
            }
        }

        public static IEnumerable<IEnumerable<Combination>> GroupByDistributions(IEnumerable<Combination> combinations, AvailabilityQuery availabilityQuery)
        {
            var referenceRooms = availabilityQuery.SearchCriteria.RoomCandidates.Select(room => room.RoomRefId);
            var resultCom = referenceRooms.GroupJoin(combinations, p => p, comb => comb.Rooms.ElementAt(0)
                                        .RoomRefId, (p, c) => new { p, c });
            return resultCom.Select(comb => comb.c);
        }

        public static Combination SumCombinations(IEnumerable<Combination> combinations, Func<IEnumerable<Combination>, string> SumValCode = null)
        {
            var result = new Combination
            {
                AdditionalServices = combinations.Select(combination => combination.AdditionalServices.ToList()).ToList()
                                            .Aggregate(new List<AdditionalService>(), (a, b) => a.Concat(b).ToList())
            };

            if (combinations.All(x => x.Status == StatusAvailability.Available))
                result.Status = StatusAvailability.Available;
            else
                result.Status = StatusAvailability.OnRequest;

            foreach (var combination in combinations)
            {
                combination.CancellationPolicy = ConvertToAmountRule(combination.CancellationPolicy, combination.Price);
            }

            if (combinations.All(comb => comb.NonRefundable.HasValue && comb.NonRefundable.Value))
                result.CancellationPolicy = new CancellationPolicy()
                {
                    PenaltyRules =
                                    [
                                        new PenaltyRule() {
                                            Type= PolicyRule.NonRefundable,
                                            DateFrom = DateTime.Now
                                        }
                                    ]
                };
            else
                result.CancellationPolicy = combinations.Select(combination => combination.CancellationPolicy)
                                            .Aggregate((a, b) => SumCombProp(a, b));

            result.Fees = combinations.Select(combination => combination.Fees)
                                            .Aggregate((a, b) => (IList<Fee>)SumCombProp(a, b));

            result.NonRefundable = combinations.Select(combination => combination.NonRefundable)
                                            .Aggregate((a, b) => SumCombProp(a, b));

            var minimumPrice = combinations.Select(combination => combination.MinimumPrice).Aggregate((a, b) => SumCombProp(a, b));
            if (minimumPrice != null && minimumPrice.Purchase != null)
                result.MinimumPrice = minimumPrice;

            result.Price = combinations.Select(combination => combination.Price)
                                            .Aggregate((a, b) => SumCombProp(a, b));
            result.Promotions = combinations.Select(combination => combination.Promotions)
                                            .Aggregate((a, b) => (IList<Promotion>)SumCombProp(a, b));
            result.RateConditions = combinations.Select(combination => combination.RateConditions)
                                            .Aggregate((a, b) => (IList<RateConditionType>)SumCombProp(a, b));
            result.Remarks = combinations.Select(combination => combination.Remarks)
                                            /*.Aggregate((a, b) => (IList<string>)(a.Concat(b)).Distinct());*/
                                            .Aggregate((a, b) => a.Concat(b).Distinct().ToList());

            // Si no hay remarks, no se debe devolver un objeto vacío, sino null
            if (result.Remarks != null && result.Remarks.Count() == 0)
                result.Remarks = null;

            result.Rooms = combinations.Select(combination => combination.Rooms)
                                            /*.Aggregate((a, b) => (IList<Domain.Common.Room>)a.Concat(b));*/
                                            .Aggregate((a, b) => a.Concat(b).ToList());
            result.ValuationCode = SumValCode(combinations);


            return result;
        }

        public static bool SumCombProp(bool? Nfr, bool? nfr1)
        {
            if (Nfr == null || nfr1 == null)
                return false;

            return Nfr.Value && nfr1.Value;
        }

        public static CancellationPolicy SumCombProp(CancellationPolicy policy, CancellationPolicy policy1)
        {
            if (policy == null && policy1 == null)
                return policy;
            if (policy.PenaltyRules.Count == 0 && policy1.PenaltyRules.Count == 0)
                return policy;

            var currency = "";
            var polices = new CancellationPolicy() { PenaltyRules = [] };
            var allRules = policy.PenaltyRules.Concat(policy1.PenaltyRules);
            var standarRule = policy.PenaltyRules.Select(i => new Tuple<int, DateTime, decimal>(1, i.DateFrom, i.Price.Purchase.Gross))
                .Concat(policy1.PenaltyRules.Select(i => new Tuple<int, DateTime, decimal>(2, i.DateFrom, i.Price.Purchase.Gross)));
            if (policy.PenaltyRules != null && policy.PenaltyRules.Count > 0)
            {
                currency = policy.PenaltyRules.ElementAt(0).CurrencyCode;
            }
            else
            {
                currency = policy1.PenaltyRules.ElementAt(0).CurrencyCode;
            }

            polices = Extension.ProcessCancelPolice(standarRule.ToList(), currency, 2);
            polices.Description = policy.Description;
            return polices;
        }

        public static IEnumerable<Fee> SumCombProp(IEnumerable<Fee> fee, IEnumerable<Fee> fee1)
        {
            var result = new List<Fee>();
            if (fee != null)
                result = result.Concat(fee).ToList();
            if (fee1 != null)
                result = result.Concat(fee1).ToList();
            return result;
        }

        public static MinimumPrice SumCombProp(MinimumPrice item, MinimumPrice item1)
        {
            var result = new MinimumPrice() { Purchase = new Domain.Common.MinimumPrice.Purchase() { Amount = 0 } };
            if (item != null && item.Purchase != null && item.Purchase.Amount != 0)
            {
                result.Purchase.Amount += item.Purchase.Amount;
            }
            if (item1 != null && item1.Purchase != null && item1.Purchase != null && item1.Purchase.Amount != 0)
            {
                result.Purchase.Amount += item1.Purchase.Amount;
            }

            if (result.Purchase.Amount == 0)
                return null; //new MinimumPrice();

            return result;
        }

        public static Price SumCombProp(Price item, Price item2)
        {
            var result = new Price()
            {
                Purchase = new Domain.Common.Price.Purchase()
                {
                    Cost = new Domain.Common.Price.Cost()
                    {
                        Amount = 0,
                        Price = 0,
                        Quantity = 1
                    },
                    Commission = new PurchaseCommission()
                    {
                        Commission = 0,
                        Percentage = 0
                    },
                    Gross = 0,
                    Net = 0,
                    CostType = item.Purchase.CostType,
                    CurrencyCode = item.Purchase.CurrencyCode
                }
            };

            result.Purchase.Gross = item.Purchase.Gross + item2.Purchase.Gross;
            result.Purchase.Net = item.Purchase.Net + item2.Purchase.Net;
            result.Purchase.Cost.Amount = item.Purchase.Cost.Amount + item2.Purchase.Cost.Amount;
            result.Purchase.Cost.Price = item.Purchase.Cost.Price + item2.Purchase.Cost.Price;

            if (item.Purchase.Commission != null && item2.Purchase.Commission != null)
            {
                result.Purchase.Commission.Commission = item2.Purchase.Commission.Commission + item2.Purchase.Commission.Commission;
            }
            else if (item2.Purchase.Commission != null)
            {
                result.Purchase.Commission = item2.Purchase.Commission;
            }
            else if (item.Purchase.Commission != null)
            {
                result.Purchase.Commission = item.Purchase.Commission;
            }

            return result;
        }

        public static IEnumerable<Promotion> SumCombProp(IEnumerable<Promotion> item, IEnumerable<Promotion> item1)
        {
            var result = new List<Promotion>();
            if (item != null)
                result = result.Concat(item).ToList();
            if (item1 != null)
                result = result.Concat(item1).ToList();
            return result;
        }

        public static IEnumerable<RateConditionType> SumCombProp(IEnumerable<RateConditionType> item, IEnumerable<RateConditionType> item1)
        {
            var result = new List<RateConditionType>();
            if (item != null)
                result = result.Concat(item).ToList();
            if (item1 != null)
                result = result.Concat(item1).ToList();
            return result;
        }

        public static CancellationPolicy ConvertToAmountRule(CancellationPolicy cancellationPolicy, Price roomPrice)
        {
            var allAmount = new List<PenaltyRule>();

            cancellationPolicy.PenaltyRules.ForEach(rule =>
            {
                if (rule.Type == PolicyRule.AmountRule)
                {
                    allAmount.Add(rule);
                }
                if (rule.Type == PolicyRule.NonRefundable)
                {
                    allAmount.Add(new PenaltyRule()
                    {
                        Type = PolicyRule.AmountRule,
                        DateFrom = DateTime.Now,
                        CurrencyCode = roomPrice.Purchase.CurrencyCode,
                        Price = new Price()
                        {
                            Purchase = new Domain.Common.Price.Purchase
                            {
                                CurrencyCode = rule.CurrencyCode,
                                Gross = roomPrice.Purchase.Gross,
                                Net = roomPrice.Purchase.Gross,
                                CostType = PurchaseCostType.Nett,
                                Cost = new Cost()
                                {
                                    Price = roomPrice.Purchase.Gross,
                                    Amount = roomPrice.Purchase.Gross,
                                    Quantity = 1
                                }
                            }
                        }
                    });
                }
                else if (rule.Type == PolicyRule.PercentageRule)
                {
                    allAmount.Add(new PenaltyRule()
                    {
                        Type = PolicyRule.AmountRule,
                        DateFrom = rule.DateFrom,
                        CurrencyCode = roomPrice.Purchase.CurrencyCode,
                        Price = new Price()
                        {
                            Purchase = new Domain.Common.Price.Purchase
                            {
                                CurrencyCode = rule.CurrencyCode,
                                Gross = roomPrice.Purchase.Gross * (decimal)rule.Percentage / 100,
                                Net = roomPrice.Purchase.Gross * (decimal)rule.Percentage / 100,
                                CostType = PurchaseCostType.Nett,
                                Cost = new Cost()
                                {
                                    Price = roomPrice.Purchase.Gross * (decimal)rule.Percentage / 100,
                                    Amount = roomPrice.Purchase.Gross * (decimal)rule.Percentage / 100,
                                    Quantity = 1
                                }
                            }
                        }
                    });
                }
            });

            var newRules = new List<PenaltyRule>();
            newRules = newRules.Concat(allAmount).ToList();
            cancellationPolicy.PenaltyRules = newRules;
            return cancellationPolicy;

        }

    }
}
