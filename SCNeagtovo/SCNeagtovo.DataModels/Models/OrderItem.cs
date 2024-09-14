using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCNeagtovo.DataModels.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid MenuId { get; set; } 
        public int Quantity { get; set; }   
        public string Mention { get; set; }

        public Order Order { get; set; }
        public Menu Menu { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            modelBuilder.Entity<OrderItem>()
                .ToTable("OrderItems")
      .HasKey(i => i.Id);

            modelBuilder.Entity<OrderItem>()

                .HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>()

                .HasOne(i => i.Menu)
                .WithMany(o => o.Orders)
                .HasForeignKey(i => i.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
