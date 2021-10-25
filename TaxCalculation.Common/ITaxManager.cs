using TaxCalculation.Dtos;

namespace TaxCalculation.Common
{
    public interface ITaxManager
    {
        TaxCode GetTaxCode(int taxCodeId);
        void CalcTax(TransactionDocument doc);
    }
}
