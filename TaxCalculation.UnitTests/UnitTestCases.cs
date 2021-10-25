using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using TaxCalculation.Common;
using TaxCalculation.Dtos;
using TaxCalculation.ServiceFactory;

namespace TaxCalculation.UnitTests
{
    [TestClass]
    public class UnitTestCases
    {
        private static IDateTimeService _dateTimeService;
        private static IProductManager _productManager;
        private static ITaxManager _taxManager;

        private static Invoice CreateInvoice(LineItems lineItems)
        {
            return new Invoice
            {
                Id = 1,
                CustomerId = "1234567890",
                CustomerName = "Mustermann GmbH",
                BillingAddress = "Hauptstraße 20\n\r61250 Usingen",
                ShippingAddress = "In der Ecke 20\n\r61250 Usingen",
                DocDate = _dateTimeService.Now,
                LineItems = lineItems
            };
        }

        private static void AddProductData(Invoice invoice)
        {
            if (invoice?.LineItems == null)
                throw new ArgumentNullException(nameof(invoice));

            foreach (var lineItem in invoice.LineItems)
            {
                if (lineItem.LineItemType != LineItemType.Product)
                    continue;

                var product = _productManager.GetProduct(lineItem.ProductId);
                if (product == null)
                    throw new NotSupportedException($"Product {lineItem.ProductId} not found!");

                lineItem.Text = product.ProductName;
                lineItem.NetPrice = product.Price;
                lineItem.NetTotal = product.Price * lineItem.Quantity;
                lineItem.TaxCodes = product.TaxCodes;
            }
        }

        public static IEnumerable<object[]> TestDataCase1 =>
            new[] {
                new object[] { 1, 1.0m, 2, 1.0m, 3, 1.0m }
            };

        public static IEnumerable<object[]> TestDataCase2 =>
            new[] {
                new object[] { 4, 1.0m, 5, 1.0m }
            };

        public static IEnumerable<object[]> TestDataCase3 =>
            new[] {
                new object[] { 6, 5.0m }
            };


        [TestInitialize()]
        public void Initialize() { }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _productManager = Factory.CreateProductService();
            _dateTimeService = Factory.CreateDateTimeSevice(new DateTime(2021, 10, 24));
            _taxManager = Factory.CreateTaxManager(_dateTimeService);
        }


        [TestMethod]
        [DynamicData(nameof(TestDataCase1))]
        public void Case1(int productId1, decimal quantity1, int productId2, decimal quantity2, int productId3, decimal quantity3)
        {
            //Create an invoice
            var invoice = CreateInvoice(new LineItems
            {
                new LineItem { Id = 1, ProductId = productId1, Quantity = quantity1 },
                new LineItem { Id = 2, ProductId = productId2, Quantity = quantity2 },
                new LineItem { Id = 3, ProductId = productId3, Quantity = quantity3 }
            });

            //Add Product data from storage
            AddProductData(invoice);
            
            //Calc tax
            _taxManager.CalcTax(invoice);

            //Evaluate result
            for (var index = 1; index <= invoice.LineItems.Count; index++)
            {
                var lineItem = invoice.LineItems[index-1];

                switch (index)
                {
                    case 1:
                        Assert.AreEqual(lineItem.GrossTotal, 12.49m);
                        break;
                    case 2:
                        Assert.AreEqual(lineItem.GrossTotal, 16.49m);
                        break;
                    case 3:
                        Assert.AreEqual(lineItem.GrossTotal, .85m);
                        break;
                }
            }

            Assert.AreEqual(invoice.TaxTotal, 1.50m);
            Assert.AreEqual(invoice.GrossTotal, 29.83m);
        }

        
        [TestMethod]
        [DynamicData(nameof(TestDataCase2))]
        public void Case2(int productId1, decimal quantity1, int productId2, decimal quantity2)
        {
            //Create an invoice
            var invoice = CreateInvoice(new LineItems
            {
                new LineItem { Id = 1, ProductId = productId1, Quantity = quantity1 },
                new LineItem { Id = 2, ProductId = productId2, Quantity = quantity2 }
            });

            //Add Product data from storage
            AddProductData(invoice);

            //Calc tax
            _taxManager.CalcTax(invoice);

            //Evaluate result
            for (var index = 1; index <= invoice.LineItems.Count; index++)
            {
                var lineItem = invoice.LineItems[index - 1];

                switch (index)
                {
                    case 1:
                        Assert.AreEqual(10.50m, lineItem.GrossTotal);
                        break;
                    case 2:
                        Assert.AreEqual(54.65m, lineItem.GrossTotal);
                        break;
                }
            }

            Assert.AreEqual(7.65m, invoice.TaxTotal);
            Assert.AreEqual(65.15m, invoice.GrossTotal);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataCase3))]
        public void Case3(int productId1, decimal quantity1)
        {
            //Create an invoice
            var invoice = CreateInvoice(new LineItems
            {
                new LineItem { Id = 1, ProductId = productId1, Quantity = quantity1 }
            });

            //Add Product data from storage
            AddProductData(invoice);

            //Calc tax
            _taxManager.CalcTax(invoice);

            //Evaluate result
            for (var index = 1; index <= invoice.LineItems.Count; index++)
            {
                var lineItem = invoice.LineItems[index - 1];

                switch (index)
                {
                    case 1:
                        Assert.AreEqual(.75m, lineItem.GrossTotal);
                        break;
                }
            }

            Assert.AreEqual(.25m, invoice.TaxTotal);
            Assert.AreEqual(.75m, invoice.GrossTotal);
        }
    }
}
