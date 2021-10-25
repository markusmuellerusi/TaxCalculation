using System;

namespace TaxCalculation.Dtos
{
    public abstract class TransactionDocument
    {
        private LineItems _lineItems;

        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime DocDate;
        public LineItems LineItems
        {
            get => _lineItems ?? new LineItems();
            set => _lineItems = value;
        }

        public decimal TaxTotal => GrossTotal - NetTotal;
        public decimal NetTotal { get; set; }
        public decimal GrossTotal { get; set; }

    }
}
