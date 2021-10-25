using System;
using TaxCalculation.Common;
using TaxCalculation.ProductService;
using TaxCalculation.TaxService;

namespace TaxCalculation.ServiceFactory
{
    public class Factory
    {
        public static ITaxManager CreateTaxManager()
        {
            return new TaxManager(null);
        }
        public static ITaxManager CreateTaxManager(IDateTimeService dateTimeSvc)
        {
            return new TaxManager(dateTimeSvc);
        }

        public static IDateTimeService CreateDateTimeSevice(DateTime now)
        {
            return new DateTimeService.DateTimeService(now);
        }

        public static IProductManager CreateProductService()
        {
            return new ProductManager();
        }
    }
}
