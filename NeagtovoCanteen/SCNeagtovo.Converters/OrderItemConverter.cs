using SCNeagtovo.DataModels.Models;
using SCNeagtovo.Dtos.Models.Order;

namespace SCNeagtovo.Converters
{
    public static class OrderItemConverter
    {
        public static OrderItemDto From(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
                Mention = orderItem.Mention,
                Menu = orderItem.Menu.From(),
                Quantity = orderItem.Quantity,
            };
        }

        public static OrderItem To(this OrderItemDto orderItem)
        {
            return new OrderItem
            {
                Mention = orderItem.Mention ?? "",
                MenuId = Guid.Parse(orderItem.Menu.Id!),
                Quantity = orderItem.Quantity,
            };
        }
    }
}
