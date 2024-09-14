using SCNeagtovo.Dtos.Models.Menu;

namespace SCNeagtovo.Dtos.Models.Order
{
    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public string? Mention { get; set; }
        public required MenuDto Menu { get; set; }
    }
}
