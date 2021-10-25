using System;
using System.Collections.Generic;

namespace TaxCalculation.Dtos
{
    public class TaxCode
    {
        public int TaxCodeId { get; set; }
        public string TaxCodeName { get; set; }
        public TaxRates TaxRates { get; set; }

        public string TaxCalculator { get; set; }
    }

    public class TaxCodes : List<TaxCode>
    {
    }
}
