using System;
using System.Collections.Generic;

namespace PRN221_PROJECT_FoodOrder.Model
{
    public partial class Customer
    {
        public Customer()
        {
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
            Rates = new HashSet<Rate>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerPhone { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
