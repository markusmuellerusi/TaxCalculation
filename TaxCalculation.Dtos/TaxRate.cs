using System;
using System.Collections.Generic;

namespace TaxCalculation.Dtos
{
    public class TaxRate
    {
        public TaxRate()
        {
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MaxValue;
        }

        public int TaxRateId { get; set; }
        public string TaxRateName { get; set; }
        public decimal Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class TaxRates : List<TaxRate>
    {
    }
}
