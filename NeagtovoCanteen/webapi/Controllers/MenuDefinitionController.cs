using Microsoft.AspNetCore.Mvc;
using SCNeagtovo.BusinessLogic.Interfaces;
using SCNeagtovo.DataModels.Models;
using SCNeagtovo.Dtos.Models.Menu;
using System.Globalization;
using System.Web;
namespace SCNeagtovo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuDefinitionController : Controller
    {
        private readonly IBaseBusinessService<DailyMenuDefinition> _dailyMenuBusinessService;
        public MenuDefinitionController(IBaseBusinessService<DailyMenuDefinition> dailyMenuBusinessService)
        {
            _dailyMenuBusinessService = dailyMenuBusinessService;
        }

        [HttpGet("daily-menu-overview/{selectedDate}")]
        public IActionResult Menu(string selectedDate)
        {
            string decodedSelectedDate = HttpUtility.UrlDecode(selectedDate); // or WebUtility.UrlDecode(selectedDate) for .NET Core/.NET 5+

            // Parse the decoded selectedDate using the custom format "dd/MM/yyyy"
            DateTime parsedDate = DateTime.ParseExact(decodedSelectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var result = _dailyMenuBusinessService.Get(x => x.Date.Date == parsedDate, include: x => x.Menu).Select(x => new DailMenuDefinitionOverviewDto
            {
                MenuName = DailMenuDefinitionOverviewDto.BuildMenuName(x.Menu.Name, x.Menu.HasPolenta, x.Menu.HasBread),
                DailyMenuName = x.Menu.Name,
                Price = x.Menu.Price,
                MenuType = x.MenuType
            });
            return Ok(result);
        }
        [HttpGet("{selectedDate}")]
        public IActionResult GetByDate(string selectedDate)
        {
            try
            {
                // Decode the URL-encoded selectedDate
                string decodedSelectedDate = HttpUtility.UrlDecode(selectedDate); // or WebUtility.UrlDecode(selectedDate) for .NET Core/.NET 5+

                // Parse the decoded selectedDate using the custom format "dd/MM/yyyy"
                DateTime parsedDate = DateTime.ParseExact(decodedSelectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var result = _dailyMenuBusinessService.Get(x => x.Date == parsedDate).ToList();
                var dailyMenuDefinitions = new List<DailyMenuDefinitionDto>();
                foreach (var item in result)
                {
                    var existingDefinition = dailyMenuDefinitions.FirstOrDefault(x => x.MenuType == item.MenuType);
                    if (existingDefinition != null)
                    {
                        existingDefinition.MenuIds.Add(item.MenuId.ToString());
                    }
                    else
                    {
                        existingDefinition = new DailyMenuDefinitionDto()
                        {
                            MenuIds = new List<string>() { item.MenuId.ToString() },
                            MenuType = item.MenuType,
                            DateTime = decodedSelectedDate
                        };
                        dailyMenuDefinitions.Add(existingDefinition);
                    }
                }
                return Ok(dailyMenuDefinitions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("define-daily-menu")]
        public IActionResult DefineDailyMenu(DailyMenuDefinitionDto data)
        {
            DateTime parsedDate = DateTime.ParseExact(data.DateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var menuType = data.MenuType;
            var recordsToDelete = _dailyMenuBusinessService.Get(x => x.MenuType == menuType && x.Date.Date == parsedDate);
            foreach (var record in recordsToDelete)
            {
                _dailyMenuBusinessService.Delete(record);
            }

            foreach (var record in data.MenuIds)
            {
                var recordToadd = new DailyMenuDefinition()
                {
                    Id = Guid.NewGuid(),
                    Date = parsedDate,
                    MenuType = menuType,
                    MenuId = Guid.Parse(record)
                };
                _dailyMenuBusinessService.Add(recordToadd);

            }

            return Ok();
        }

    }
}
