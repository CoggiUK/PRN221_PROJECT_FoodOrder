using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace PRN221_PROJECT_FoodOrder.Model
{
    [Serializable]
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
		public bool EmailConfirm { get; set; }
		public string Role { get; set; } = null!;
		public string? Avatar { get; set; }
        public bool UserStatus { get; set; }
		public virtual Customer? Customer { get; set; }
    }
}
