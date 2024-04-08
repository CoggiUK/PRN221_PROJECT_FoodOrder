using System;
using System.Collections.Generic;

namespace PRN221_PROJECT_FoodOrder.Model
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public decimal? UnitCost { get; set; }
        public int? Amount { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual Product? Item { get; set; }
        public virtual Order? Order { get; set; }
    }
}
