using Microsoft.AspNetCore.Mvc;
using SCNeagtovo.BusinessLogic.Interfaces;
using SCNeagtovo.Converters;
using SCNeagtovo.DataModels.Models;
using SCNeagtovo.Dtos.Models.Menu;


namespace SCNeagtovo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IBaseBusinessService<Menu> _menuModelBusinessService;
        public MenuController(IBaseBusinessService<Menu> menuModelBusinessService)
        {
            _menuModelBusinessService = menuModelBusinessService;
        }
        [HttpGet]
        public IActionResult GetMenuItems()
        {
            var result = new List<MenuDto>();
            var menus = _menuModelBusinessService.GetAll();
            foreach (var menu in menus)
            {
                result.Add(menu.From());
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<MenuDto> GetMenuItemById(string id)
        {
            var menuItem = _menuModelBusinessService.GetById(Guid.Parse(id));
            if (menuItem == null)
            {
                return NotFound();
            }

            return Ok(menuItem.From());
        }

        [HttpDelete("{id}")]
        public ActionResult<MenuDto> DeleteMenu(string id)
        {
            var menuItem = _menuModelBusinessService.GetById(Guid.Parse(id));
            if (menuItem != null)
            {
                _menuModelBusinessService.Delete(menuItem);
            }

            return Ok();
        }


        [HttpPost("create-menu")]
        public IActionResult AddMenu([FromBody] MenuDto menuDto)
        {
            if (string.IsNullOrEmpty(menuDto.Id))
            {
                menuDto.Id = Guid.NewGuid().ToString();
                _menuModelBusinessService.Add(menuDto.To());
                return Ok();
            }

            _menuModelBusinessService.Update(menuDto.To());
            return Ok();
        }
    }
}
