using Microsoft.EntityFrameworkCore;

namespace SCNeagtovo.DataModels.Models
{
    public class DeliveryDetail
    {
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid OrderId { get; set; }
        public string Adress { get; set; }
        public string MobilePhone { get; set; }
        public Client? Client { get; set; }
        public Order Order { get; set; }
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryDetail>(entity =>
            {
                entity.ToTable("DeliveryDetails");

                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Client)
                       .WithMany(x => x.DeliveryDetails)
                       .HasForeignKey(x => x.ClientId);

                entity.HasOne(d => d.Order)
                    .WithOne(o => o.Delivery)
                    .HasForeignKey<DeliveryDetail>(d => d.OrderId);

            });
        }
    }
}
