using Microsoft.EntityFrameworkCore;
using SCNeagtovo.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCNeagtovo.DataModels.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }    
        public string Name { get; set; }

        public IList<Menu> Menus { get; set; }


        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(entity => entity.Id);
            });
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel[] {
                    new CategoryModel {Id = (int)CategoryEnum.Soup, Name = "Soup"},
                    new CategoryModel {Id = (int)CategoryEnum.Salad, Name = "Salad"},
                    new CategoryModel {Id = (int)CategoryEnum.Sides, Name = "Sides"},
                }
            );
        }
    }
}
