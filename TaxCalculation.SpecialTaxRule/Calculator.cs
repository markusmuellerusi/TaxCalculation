using System;
using TaxCalculation.Common;

namespace TaxCalculation.SpecialTaxRule
{
    public class Calculator : ITaxCalculator
    {
        public decimal CalcTax(bool includeQuantity, decimal quantity, decimal price, decimal rate)
        {
            if (includeQuantity)
            {
                //For wholesale trade
                return Math.Round(quantity * price * rate * 20.0M + 0.499m, MidpointRounding.AwayFromZero) * 0.05M;
            }
            
            //For retail trade
            var result = Math.Round(price * rate * 20.0M + 0.499m, MidpointRounding.AwayFromZero) * 0.05M;
            return result;
        }
    }
}
