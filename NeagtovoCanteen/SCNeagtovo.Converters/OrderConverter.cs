using SCNeagtovo.DataModels.Models;
using SCNeagtovo.Dtos.Models.Order;

namespace SCNeagtovo.Converters
{
    public static class OrderConverter
    {
        public static OrderDto FromToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id.ToString(),
                Date = order.Date,
                Mobile = order.Delivery.MobilePhone,
                Address = order.Delivery.Adress,
                Client = order.Delivery.ClientName,
                Items = order.Items.Select(x => x.From()).ToList(),
            };
        }
        public static GetOrderDto FromToGetOrderDto(this Order order)
        {
            return new GetOrderDto
            {
                Id = order.Id.ToString(),
                Date = order.Date,
                Mobile = order.Delivery.MobilePhone,
                Address = order.Delivery.Adress,
                Items = order.Items.Select(x => x.From()).ToList(),
                OrderNumber = order.OrderNumber,
                Client = order.Delivery.ClientName,
            };
        }
        public static Order To(OrderDto order)
        {
            return new Order
            {
                Id = Guid.Parse(order.Id),
                Date = order.Date,
                Delivery = new DeliveryDetail()
                {
                    Adress = order.Address ?? "",
                    MobilePhone = order.Mobile ?? "",
                    ClientName = order.Client,
                },
                Items = order.Items.Select(x => x.To()).ToList(),
            };
        }
    }
}
