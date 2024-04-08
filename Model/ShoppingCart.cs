using System;
using System.Collections.Generic;

namespace PRN221_PROJECT_FoodOrder.Model
{
    public partial class ShoppingCart
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
