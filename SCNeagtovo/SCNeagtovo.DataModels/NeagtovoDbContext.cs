using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCNeagtovo.Common.Enum;
using SCNeagtovo.DataModels.Models;

namespace SCNeagtovo.DataModels
{
    public class NeagtovoDbContext : DbContext
    {
        private static readonly object _lock = new();
        public DbSet<Menu> Menus { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<DailyMenuDefinition> DailyMenyDefinition { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Client> Clients { get; set; }

        public NeagtovoDbContext()
        {

        }
        public NeagtovoDbContext(DbContextOptions<NeagtovoDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CategoryModel.OnModelCreating(modelBuilder);
            Menu.OnModelCreating(modelBuilder);
            Order.OnModelCreating(modelBuilder);
            OrderItem.OnModelCreating(modelBuilder);
            DailyMenuDefinition.OnModelCreating(modelBuilder);
            Client.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Order>())
            {
                if (entry.State == EntityState.Added)
                {
                    lock (entry) {
                        var maxOrderNumber = Orders.Max(o => (int?)o.OrderNumber) ?? 0;

                        // Set the new OrderNumber
                        entry.Entity.OrderNumber = maxOrderNumber + 1;
                    }

                    // Fetch the current maximum OrderNumber
                    
                }
            }
            return base.SaveChanges();
        }

    }
}
