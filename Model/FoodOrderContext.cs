using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN221_PROJECT_FoodOrder.Model
{
    public partial class FoodOrderContext : DbContext
    {
        public FoodOrderContext()
        {
        }

        public FoodOrderContext(DbContextOptions<FoodOrderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Rate> Rates { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Comment>(entity =>
			{
				entity.ToTable("comment");

				entity.Property(e => e.CommentId).HasColumnName("comment_id");

				entity.Property(e => e.CommentContent).HasColumnName("comment_content");

				entity.Property(e => e.CreatedAt)
					.HasColumnType("datetime")
					.HasColumnName("created_at")
					.HasDefaultValueSql("(getdate())");

				entity.Property(e => e.CustomerId).HasColumnName("customer_id");

				entity.Property(e => e.ProductId).HasColumnName("product_id");

				entity.HasOne(d => d.Customer)
					.WithMany(p => p.Comments)
					.HasForeignKey(d => d.CustomerId)
					.HasConstraintName("FK__comment__custome__403A8C7D");

				entity.HasOne(d => d.Product)
					.WithMany(p => p.Comments)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.Cascade)
					.HasConstraintName("FK__comment__product__412EB0B6");
			});

			modelBuilder.Entity<Customer>(entity =>
			{
				entity.ToTable("customer");

				entity.HasIndex(e => e.UserId, "UQ__customer__B9BE370EFFA495F1")
					.IsUnique();

				entity.Property(e => e.CustomerId).HasColumnName("customer_id");

				entity.Property(e => e.CustomerAddress)
					.HasMaxLength(255)
					.HasColumnName("customer_address");

				entity.Property(e => e.CustomerName)
					.HasMaxLength(100)
					.HasColumnName("customer_name");

				entity.Property(e => e.CustomerPhone)
					.HasMaxLength(20)
					.IsUnicode(false)
					.HasColumnName("customer_phone");

				entity.Property(e => e.UserId).HasColumnName("user_id");

				entity.HasOne(d => d.User)
					.WithOne(p => p.Customer)
					.HasForeignKey<Customer>(d => d.UserId)
					.HasConstraintName("FK__customer__user_i__3A81B327");
			});

			modelBuilder.Entity<Order>(entity =>
			{
				entity.ToTable("order");

				entity.Property(e => e.OrderId).HasColumnName("order_id");

				entity.Property(e => e.DeliveryAddress).HasColumnName("delivery_address");

				entity.Property(e => e.OrderDate)
					.HasColumnType("datetime")
					.HasColumnName("order_date")
					.HasDefaultValueSql("(getdate())");

				entity.Property(e => e.OrderStatus)
					.HasMaxLength(255)
					.HasColumnName("order_status");

				entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

				entity.Property(e => e.ReceiverName)
					.HasMaxLength(100)
					.HasColumnName("receiver_name");

				entity.Property(e => e.ReceiverPhone)
					.HasMaxLength(20)
					.IsUnicode(false)
					.HasColumnName("receiver_phone");

				entity.Property(e => e.Total)
					.HasColumnType("decimal(18, 0)")
					.HasColumnName("total");

				entity.HasOne(d => d.Receiver)
					.WithMany(p => p.Orders)
					.HasForeignKey(d => d.ReceiverId)
					.HasConstraintName("FK__order__receiver___3D5E1FD2");
			});

			modelBuilder.Entity<OrderItem>(entity =>
			{
				entity.ToTable("order_item");

				entity.HasIndex(e => new { e.OrderId, e.ItemId }, "order_item_index_0")
					.IsUnique();

				entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");

				entity.Property(e => e.Amount).HasColumnName("amount");

				entity.Property(e => e.ItemId).HasColumnName("item_id");

				entity.Property(e => e.ItemName)
					.HasMaxLength(100)
					.HasColumnName("item_name");

				entity.Property(e => e.ItemType)
					.HasMaxLength(100)
					.HasColumnName("item_type");

				entity.Property(e => e.OrderId).HasColumnName("order_id");

				entity.Property(e => e.Subtotal)
					.HasColumnType("decimal(18, 0)")
					.HasColumnName("subtotal");

				entity.Property(e => e.UnitCost)
					.HasColumnType("decimal(18, 0)")
					.HasColumnName("unit_cost");

				entity.HasOne(d => d.Item)
					.WithMany(p => p.OrderItems)
					.HasForeignKey(d => d.ItemId)
					.OnDelete(DeleteBehavior.SetNull)
					.HasConstraintName("FK__order_ite__item___3F466844");

				entity.HasOne(d => d.Order)
					.WithMany(p => p.OrderItems)
					.HasForeignKey(d => d.OrderId)
					.HasConstraintName("FK__order_ite__order__3E52440B");
			});

			modelBuilder.Entity<Product>(entity =>
			{
				entity.ToTable("product");

				entity.Property(e => e.ProductId).HasColumnName("product_id");

				entity.Property(e => e.Description).HasColumnName("description");

				entity.Property(e => e.ProductImage)
					.HasMaxLength(255)
					.HasColumnName("product_image");

				entity.Property(e => e.ProductName)
					.HasMaxLength(100)
					.HasColumnName("product_name");

				entity.Property(e => e.ProductType)
					.HasMaxLength(100)
					.HasColumnName("product_type");

				entity.Property(e => e.Rating)
					.HasColumnType("decimal(18, 2)")
					.HasColumnName("rating");
                entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");

                entity.Property(e => e.UnitPrice)
					.HasColumnType("decimal(18, 0)")
					.HasColumnName("unit_price");
			});

			modelBuilder.Entity<Rate>(entity =>
			{
				entity.HasKey(e => new { e.CustomerId, e.ProductId })
					.HasName("PK__rate__8915EC5ABDBC85A8");

				entity.ToTable("rate");

				entity.Property(e => e.CustomerId).HasColumnName("customer_id");

				entity.Property(e => e.ProductId).HasColumnName("product_id");

				entity.Property(e => e.RateStar)
					.HasColumnName("rate_star");

				entity.HasOne(d => d.Customer)
					.WithMany(p => p.Rates)
					.HasForeignKey(d => d.CustomerId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__rate__customer_i__4222D4EF");

				entity.HasOne(d => d.Product)
					.WithMany(p => p.Rates)
					.HasForeignKey(d => d.ProductId)
					.HasConstraintName("FK__rate__product_id__4316F928");
			});

			modelBuilder.Entity<ShoppingCart>(entity =>
			{
				entity.HasKey(e => new { e.CustomerId, e.ProductId })
					.HasName("PK__shopping__8915EC5A20F2D7E5");

				entity.ToTable("shopping_cart");

				entity.Property(e => e.CustomerId).HasColumnName("customer_id");

				entity.Property(e => e.ProductId).HasColumnName("product_id");

				entity.Property(e => e.Amount)
					.HasColumnName("amount")
					.HasDefaultValueSql("((1))");

				entity.HasOne(d => d.Customer)
					.WithMany(p => p.ShoppingCarts)
					.HasForeignKey(d => d.CustomerId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__shopping___custo__3B75D760");

				entity.HasOne(d => d.Product)
					.WithMany(p => p.ShoppingCarts)
					.HasForeignKey(d => d.ProductId)
					.HasConstraintName("FK__shopping___produ__3C69FB99");
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("user");

				entity.HasIndex(e => e.Email, "UQ__user__AB6E616407FA687E")
					.IsUnique();

				entity.HasIndex(e => e.Username, "UQ__user__F3DBC572CC09237F")
					.IsUnique();

				entity.Property(e => e.UserId).HasColumnName("user_id");

				entity.Property(e => e.Avatar)
					.HasMaxLength(255)
					.HasColumnName("avatar");

				entity.Property(e => e.Email)
					.HasMaxLength(255)
					.HasColumnName("email");

				entity.Property(e => e.EmailConfirm).HasColumnName("email_confirm");

				entity.Property(e => e.Password)
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasColumnName("password");

				entity.Property(e => e.Role)
					.HasMaxLength(255)
					.HasColumnName("role");

				entity.Property(e => e.Username)
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasColumnName("username");
				entity.Property(e => e.UserStatus).HasColumnName("user_status");
			});

			OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
