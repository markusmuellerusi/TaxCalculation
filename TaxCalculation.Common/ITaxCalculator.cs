namespace TaxCalculation.Common
{
    public interface ITaxCalculator
    {
        decimal CalcTax(bool includeQuantity, decimal quantity, decimal price, decimal rate);
    }
}
