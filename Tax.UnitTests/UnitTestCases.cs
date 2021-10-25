using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using Common;
using DateTimeService;
using Dtos;
using ServiceFactory;

namespace Tax.UnitTests
{
    [TestClass]
    public class UnitTestCases
    {
        private static IDateTimeService _dateTimeService;
        private static ITaxManager _taxManager;

        public static IEnumerable<object[]> TestDataCase1 =>
            new[] {
                new object[] { 1, 1.0m, 2, 1.0m, 3, 1.0m }
            };

        [TestInitialize()]
        public void Initialize() { }

        [TestCleanup()]
        public void Cleanup()
        {
            _taxManager = null;
        }

        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _dateTimeService = Factory.CreateDateTimeSevice(new DateTime(2021, 10, 24));
            _taxManager = Factory.CreateTaxManager(_dateTimeService);
        }


        [TestMethod]
        [DynamicData(nameof(TestDataCase1))]
        public void Case1(int productId1, decimal quantity1, int productId2, decimal quantity2, int productId3, decimal quantity3)
        {
            var invoice = new Invoice
            {
                Id = 1,
                
                CustomerId = "1234567890",
                CustomerName = "Markus",
                BillingAddress = "In der Laach 20\n\r61250 Usingen",
                ShippingAddress = "In der Laach 20\n\r61250 Usingen",
                DocDate = _dateTimeService.Now,
                LineItems = new LineItems
                {
                    new LineItem
                    {
                        Id = 1,
                        ProductId = 1,
                        Quantity = 1m
                    },
                    new LineItem
                    {
                        Id = 2,
                        ProductId = 2,
                        Quantity = 1m
                    },
                    new LineItem
                    {
                        Id = 3,
                        ProductId = 3,
                        Quantity = 1m
                    }
                }
            };

            foreach (var lineItem in invoice.LineItems)
            {
                if (lineItem.LineItemType != LineItemType.Product)
                    continue;

                var product = ProductService.ProductManager.Current.GetProduct(lineItem.ProductId);
                if (product == null)
                    throw new NotSupportedException($"Product {lineItem.ProductId} not found!");

                lineItem.Text = product.ProductName;
                lineItem.NetPrice = product.Price;
                lineItem.NetTotal = product.Price * lineItem.Quantity;
                lineItem.TaxCodes = product.TaxCodes;
            }

            _taxManager.CalcTax(invoice);

            Assert.IsTrue(invoice.LineItems.Count == 3);
        }
    }
}
