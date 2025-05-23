using Domain.Common.Price;
using System;

namespace Application.WorkFlow.Services
{
    class PriceService
    {
        public static Price GetPrice(
          string currency,
          Decimal gross,
          bool commissionIncluded,
          Decimal? commission,
          Decimal? commissionPercentage)
        {

            //Nett by default
            var purchase = new Purchase() {CurrencyCode = currency, CostType = PurchaseCostType.Nett, Gross = gross, Net = gross};

            //Check if supplier send commissions.
            if (commissionIncluded && commission.HasValue && commissionPercentage.HasValue)
            {
                purchase.Commission = new PurchaseCommission
                {
                    Commission = commission.Value,
                    Percentage = commissionPercentage.Value
                };
                purchase.CostType = PurchaseCostType.Commissionable;
                purchase.Net = gross - commission.GetValueOrDefault();
            };

            purchase.Cost = new Cost
            {
                Amount = purchase.Gross,
                Price = purchase.Net,
                Quantity = 1
            };

            var price = new Price { Purchase = purchase };
            return price;
        }
    }
}
