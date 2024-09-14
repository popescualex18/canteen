using Microsoft.EntityFrameworkCore;

namespace SCNeagtovo.DataModels.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public int OrderNumber { get; set; }
        public Guid DeliveryId { get; set; }

        public DeliveryDetail Delivery { get; set; }
        public List<OrderItem> Items { get; set; } // Assuming `Item` is a class that represents items

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Items)
                        .WithOne()
                        .HasForeignKey(item => item.OrderId) // Assuming you have an OrderId in Items
                        .IsRequired();
                entity.HasOne(o => o.Delivery)
                        .WithOne(d => d.Order)
                        .HasForeignKey<Order>(o => o.DeliveryId);

            });
        }
    }

}
