using Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecommerce.AccessData.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Company> Companys { get; set; }    

        public DbSet<Model> Models { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Inventory> Inventories { get; set; }   

        public DbSet<InventoryDetails> InventoryDetails { get; set; }

        public DbSet<KardexInventory> KardexInventories { get; set; }   

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ProductWarehouse> ProductWarehouses { get; set; }

        public DbSet<ShoppingCar> ShoppingCars { get; set; }    

        public DbSet<Warehouse> Warehouses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}