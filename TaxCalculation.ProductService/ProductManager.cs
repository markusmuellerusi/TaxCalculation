using System.Linq;
using TaxCalculation.Common;
using TaxCalculation.Dtos;

namespace TaxCalculation.ProductService
{
    public class ProductManager: IProductManager
    {
        // ReSharper disable once InconsistentNaming
        private Products _products;
        
        public ProductManager()
        {
            Init();
        }

        /// <summary>
        /// Create inmemory data, may be data is stored in a database
        /// </summary>
        private void Init()
        {
            _products = new Products
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "book",
                    Price = 12.49m,
                    TaxCodes = new TaxCodes
                    {
                        new TaxCode { TaxCodeId = 1 }
                    }
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "music CD",
                    Price = 14.99m,
                    TaxCodes = new TaxCodes
                    {
                        new TaxCode { TaxCodeId = 2 }
                    }
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "chocolate bar",
                    Price = .85m,
                    TaxCodes = new TaxCodes
                    {
                        new TaxCode { TaxCodeId = 1 }
                    }
                },
                new Product
                {
                    ProductId = 4,
                    ProductName = "imported box of chocolates",
                    Price = 10.00m,
                    IsImported = true,
                    TaxCodes = new TaxCodes
                    {
                        new TaxCode { TaxCodeId = 1 }
                    }
                },
                new Product
                {
                    ProductId = 5,
                    ProductName = "imported bottle of perfume",
                    Price = 47.50m,
                    IsImported = true,
                    TaxCodes = new TaxCodes
                    {
                        new TaxCode { TaxCodeId = 2 }
                    }
                },
                new Product
                {
                    ProductId = 6,
                    ProductName = "imported medical plaster",
                    Price = .10m,
                    IsImported = true,
                    TaxCodes = new TaxCodes
                    {
                        new TaxCode { TaxCodeId = 1 }
                    }
                }
            };

            //Add additional tax for imported products!
            foreach (var product in _products.Where(product => product.IsImported))
            {
                product.TaxCodes.Add(new TaxCode { TaxCodeId = 3 });
            }
        }

        public Product GetProduct(int id)
        {
            return _products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}
