using System;
using TaxCalculation.Common;
using TaxCalculation.Dtos;
using TaxCalculation.ServiceFactory;

namespace Tax.Console
{
    class Program
    {
        private static IDateTimeService _dateTimeService;
        private static IProductManager _productManager;
        private static ITaxManager _taxManager;

        private static void Main(string[] args)
        {
            _productManager = Factory.CreateProductService();
            _dateTimeService = Factory.CreateDateTimeService(DateTime.Now);
            _taxManager = Factory.CreateTaxManager(_dateTimeService);

            //Create an invoice
            var invoice = CreateInvoice(1, new LineItems
            {
                new LineItem { Id = 1, ProductId = 1, Quantity = 1 },
                new LineItem { Id = 2, ProductId = 2, Quantity = 1 },
                new LineItem { Id = 3, ProductId = 3, Quantity = 1 }
            });
            RunInvoice(invoice);

            //Create an invoice
            invoice = CreateInvoice(2, new LineItems
            {
                new LineItem { Id = 1, ProductId = 4, Quantity = 1 },
                new LineItem { Id = 2, ProductId = 5, Quantity = 1 }
            });
            RunInvoice(invoice);


            //Create an invoice
            invoice = CreateInvoice(3, new LineItems
            {
                new LineItem { Id = 1, ProductId = 6, Quantity = 5 }
            });
            RunInvoice(invoice);

            System.Console.Read();
        }

        private static void RunInvoice(Invoice invoice)
        {
            LogInfo("");
            LogInfo($"Test Case {invoice.Id}");

            //Add Product data from storage
            AddProductData(invoice);
            //Calc tax
            _taxManager.CalcTax(invoice);
            LogInfo($"Your invoice {invoice.CustomerName} {invoice.Id}");
            LogVerbose($"Line items:");
            foreach (var lineItem in invoice.LineItems)
            {
                LogVerbose($"Lineitem {lineItem.Text}\tQuantity: {lineItem.Quantity}");
                LogVerbose($"\tNetprice: {lineItem.NetPrice}$\tGrossprice: {lineItem.GrossPrice}$");
                LogVerbose($"\tNetTotal: {lineItem.NetTotal}$\tGrossTotal: {lineItem.GrossTotal}$\tTaxTotal: {lineItem.TaxTotal}$");
            }
            LogVerbose($"Totals:");
            LogVerbose($"\tNetTotal: {invoice.NetTotal}$");
            LogVerbose($"\tGrossTotal: {invoice.GrossTotal}$");
            LogVerbose($"\tTaxTotal: {invoice.TaxTotal}$");
        }

        private static Invoice CreateInvoice(int invoiceId, LineItems lineItems)
        {
            return new Invoice
            {
                Id = invoiceId,
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

        private static void LogError(string ex)
        {
            var color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(ex);
            System.Console.WriteLine("Press any key to continue!");
            System.Console.ForegroundColor = color;
            System.Console.Read();
        }
        private static void LogInfo(string message)
        {
            var color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = color;
        }

        private static void LogVerbose(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
