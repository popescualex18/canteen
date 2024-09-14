using SCNeagtovo.DataModels.Models;
using SCNeagtovo.Dtos.Models.Menu;

namespace SCNeagtovo.Converters
{
    public static class MenuConverter
    {
        public static Menu To(this MenuDto menuDto)
        {
            return new Menu
            {
                Id = Guid.Parse(menuDto.Id),
                Name = menuDto.Name,
                Price = menuDto.Price,
                CategoryId = menuDto.CategoryId,
                HasBread = menuDto.HasBread,
                HasPolenta = menuDto.HasPolenta
            };
        }
        public static MenuDto From(this Menu menu)
        {
            return new MenuDto
            {
                Id = menu.Id.ToString(),
                Name = menu.Name,
                Price = menu.Price,
                CategoryId = menu.CategoryId,
                HasBread = menu.HasBread,
                HasPolenta = menu.HasPolenta
            };
        }

    }
}
