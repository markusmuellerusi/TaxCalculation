using TaxCalculation.Dtos;

namespace TaxCalculation.Common
{
    public interface IProductManager
    {
        Product GetProduct(int id);
    }
}
