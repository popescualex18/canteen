using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SCNeagtovo.Api.Authorization.Helpers;
using SCNeagtovo.Api.Infrastructure;
using SCNeagtovo.BusinessLogic.Interfaces;
using SCNeagtovo.Converters;
using SCNeagtovo.DataModels.Models;
using SCNeagtovo.Dtos.Models.Order;

namespace SCNeagtovo.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IBaseBusinessService<Order> _orderBusinessService;
        private readonly IBaseBusinessService<OrderItem> _orderItemsBusinessService;
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly ITokenHelper _tokenHelper;
        public OrderController(IBaseBusinessService<Order> orderBusinessService, IBaseBusinessService<OrderItem> orderItemsBusinessService
            , IHubContext<OrderHub> hubContext, ITokenHelper tokenHelper)
        {
            _orderBusinessService = orderBusinessService;
            _orderItemsBusinessService = orderItemsBusinessService;
            _hubContext = hubContext;
            _tokenHelper = tokenHelper;
        }

        [HttpGet("get-all")]
        public IActionResult Index()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var claims = _tokenHelper.GetClaims(token.ToString());
            var result = _orderBusinessService
               .GetAllAsQueryable()
               .Include(x => x.Items)
               .ThenInclude(x => x.Menu)
               .Include(x => x.Delivery);
            if (claims.FirstOrDefault(x => x.Type == "role")?.Value == "Admin")
            {
                return Ok(result.Select(x => x.FromToGetOrderDto()));
            }
            var usersId = claims.First(x => x.Type == "unique_name")?.Value;
            var filteredOrder = result.Where(x => x.Delivery.ClientId != null && x.Delivery.ClientId.ToString() == usersId);
            return Ok(filteredOrder.Select(x => x.FromToGetOrderDto()));
        }

        [HttpGet("order/{id}")]
        public IActionResult GetOrderById(Guid id)
        {
            var entity = _orderBusinessService.GetById(id);
            if (entity != null)
            {
                _orderBusinessService.Delete(entity);
            }
            return Ok();
        }
        [HttpPost("create-order")]
        public IActionResult AddOrder([FromBody] GetOrderDto order)
        {
            var model = OrderConverter.To(order);
            _orderBusinessService.Add(model);
            return Ok(model.Id);
        }

        [HttpPost("update-order")]
        public async Task<IActionResult> UpdateOrder([FromBody] GetOrderDto order)
        {
            if (order != null)
            {
                var model = OrderConverter.To(order);
                model.OrderNumber = order.OrderNumber!.Value;
                var itemsToDelete = _orderItemsBusinessService.Get(x => x.OrderId == Guid.Parse(order.Id));
                foreach (var item in itemsToDelete)
                {
                    _orderItemsBusinessService.Delete(item);
                }

                _orderBusinessService.Update(model, ignoreProperties: new List<string> { nameof(Order.OrderNumber) });
                await _hubContext.Clients.All.SendAsync("NewOrder", order);
            }

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteOrder(Guid id)
        {
            var entity = _orderBusinessService.GetById(id);
            if (entity != null)
            {
                _orderBusinessService.Delete(entity);
            }
            return Ok();
        }
    }
}
