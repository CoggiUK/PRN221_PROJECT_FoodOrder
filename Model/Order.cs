using System;
using System.Collections.Generic;

namespace PRN221_PROJECT_FoodOrder.Model
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverPhone { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string? DeliveryAddress { get; set; }
        public decimal? Total { get; set; }

        public virtual Customer? Receiver { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
