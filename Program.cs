using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Business.Service;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<FoodOrderContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<IOrderService, OrderService>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}