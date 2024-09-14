namespace SCNeagtovo.Dtos.Models.Order
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Client { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
