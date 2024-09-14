using Microsoft.EntityFrameworkCore;

namespace SCNeagtovo.DataModels.Models
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public bool HasBread { get; set; }
        public bool HasPolenta { get; set; }

        public IList<OrderItem> Orders { get; set; }
        public IList<DailyMenuDefinition> DailyMenyDefinitions { get; set; }
        public CategoryModel Category { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menus");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(x => x.Category).WithMany(x => x.Menus);
            });

        }
    }
}
