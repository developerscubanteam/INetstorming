using Domain.Common.CancellationPolicy;
using Domain.Common.Price;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using System.Globalization;

namespace Application.WorkFlow.Services
{
    internal class CancellationPolicyService
    {
        public static CancellationPolicy? GetCancellationPolicy(
            AvailHotelAgreement? agreement, bool nonRefundable, BookingMethod method)
        {
            if (agreement != null)
            {
                var result = new CancellationPolicy()
                {
                    PenaltyRules = []
                };

                // Si es nonrefundable se debe agregar una regla para esto
                if (nonRefundable)
                {
                    result.PenaltyRules.Add(new PenaltyRule()
                    {
                        Type = PolicyRule.NonRefundable,
                        DateFrom = DateTime.Now.Date
                    });
                }
                else if (agreement.deadline != null) // es refundable, procesar las políticas
                {
                    var deadLine = DateTime.ParseExact(agreement.deadline.value.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    // Según la doc, cuando en el GetDeadLine no hay policies ni remarks, la cancelación es a partir del deadline y el costo es el precio de la primera noche
                    if (method == BookingMethod.GetDeadLine && (agreement.policies == null || agreement.policies.Length == 0))
                    {
                        var cost = GetOneNightCost(agreement);
                        if (cost > 0)
                        {
                            result.PenaltyRules.Add(new PenaltyRule()
                            {
                                Type = PolicyRule.AmountRule,
                                DateFrom = deadLine,
                                CurrencyCode = agreement.currency,
                                Price = new Price()
                                {
                                    Purchase = new Domain.Common.Price.Purchase()
                                    {
                                        CurrencyCode = agreement.currency,
                                        Gross = cost,
                                        Net = cost,
                                        CostType = PurchaseCostType.Nett,
                                        Cost = new Cost()
                                        {
                                            Price = cost,
                                            Amount = cost,
                                            Quantity = 1
                                        }
                                    }
                                }
                            });
                        }
                    }
                    else
                    {
                        for (int i = 0; i < agreement.policies.Length; i++)
                        {
                            var dateFrom = DateTime.ParseExact(agreement.policies[i].from.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                            // Desde el deadline hasta la fecha de la primera policy se carga el precio de una noche
                            if (i == 0 && deadLine < dateFrom)
                            {
                                var cost = GetOneNightCost(agreement);
                                if (cost > 0)
                                {
                                    result.PenaltyRules.Add(new PenaltyRule()
                                    {
                                        Type = PolicyRule.AmountRule,
                                        CurrencyCode = agreement.currency,
                                        DateFrom = dateFrom,
                                        Price = new Price()
                                        {
                                            Purchase = new Domain.Common.Price.Purchase()
                                            {
                                                CurrencyCode = agreement.currency,
                                                Gross = cost,
                                                Net = cost,
                                                CostType = PurchaseCostType.Nett,
                                                Cost = new Cost()
                                                {
                                                    Price = cost,
                                                    Amount = cost,
                                                    Quantity = 1
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                            else
                            {
                                result.PenaltyRules.Add(new PenaltyRule()
                                {
                                    Type = PolicyRule.PercentageRule,
                                    DateFrom = dateFrom,
                                    Percentage = agreement.policies[i].percentage
                                });
                            }
                        }
                        ;
                    }
                    ;
                }
                if (result.PenaltyRules.Any())
                    return result;

                // IMPOTRTANTE!!!!!
                //      1- Si no hay políticas de cancelación, debe devolverse NULL en lugar de un objeto vacío
                //      2- Nunca se debe devolver políticcas de cancelación con precio cero

            }
            return null;
        }
        private static decimal GetOneNightCost(AvailHotelAgreement agreement)
        {
            decimal result = 0;
            if (agreement != null && agreement.room != null)
                foreach (var item in agreement.room)
                {
                    var dateFrom = DateTime.ParseExact(item.price[0].from.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    var dateTo = DateTime.ParseExact(item.price[0].to.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    var days = (dateTo - dateFrom).TotalDays;
                    var cotPrice = item.price[0].cotprice != null ? item.price[0].cotprice.nett : 0;

                    var roomCost = (item.price[0].roomprice.nett + cotPrice) / (decimal)days;
                    result += roomCost;
                }
            ;
            return result;
        }


    }
}
