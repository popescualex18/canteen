using Microsoft.EntityFrameworkCore;

namespace SCNeagtovo.DataModels.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<DeliveryDetail> DeliveryDetails { get; set; }
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(x => x.Id);
            });
        }
    }
}
