using System;
using System.Collections.Generic;

namespace PRN221_PROJECT_FoodOrder.Model
{
    public partial class Product
    {
        public Product()
        {
            Comments = new HashSet<Comment>();
            OrderItems = new HashSet<OrderItem>();
            Rates = new HashSet<Rate>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Description { get; set; }
        public decimal? Rating { get; set; }
        public string? ProductImage { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
