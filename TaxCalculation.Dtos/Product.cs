using System.Collections.Generic;

namespace TaxCalculation.Dtos
{
    public class Product
    {
        public Product()
        {
            TaxCodes = new TaxCodes();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public bool IsImported { get; set; }
        public TaxCodes TaxCodes { get; set; }
    }

    public class Products : List<Product>
    {
    }
}
