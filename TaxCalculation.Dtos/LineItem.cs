using System.Collections.Generic;

namespace TaxCalculation.Dtos
{
    public class LineItem
    {
        public LineItem()
        {
            LineItemType = LineItemType.Product;
        }

        public int Id { get; set; }
        public LineItemType LineItemType { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
        public decimal Quantity { get; set; }
        public TaxCodes TaxCodes { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetTotal { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal TaxTotal { get; set; }
    }

    public class LineItems : List<LineItem>
    {
    }

    public enum LineItemType
    {
        Product,
        Text,
        GroupingStart,
        GrupingEnd,
        PageBreak
    }

}
