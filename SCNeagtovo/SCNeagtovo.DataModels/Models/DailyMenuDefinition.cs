
using Microsoft.EntityFrameworkCore;

namespace SCNeagtovo.DataModels.Models
{
    public class DailyMenuDefinition
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        public int MenuType { get; set; }
        public DateTime Date { get; set; }

        public Menu Menu { get; set; }
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyMenuDefinition>(entity =>
            {
                entity.ToTable("DailyMenuDefinitions");
                entity.HasKey(entity => new { entity.Id });
                entity.HasOne(x => x.Menu).WithMany(x => x.DailyMenyDefinitions);
            });
        }
    }
}
