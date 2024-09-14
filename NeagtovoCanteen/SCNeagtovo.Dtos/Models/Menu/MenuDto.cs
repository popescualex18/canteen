namespace SCNeagtovo.Dtos.Models.Menu
{
    public class MenuDto
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required int CategoryId { get; set; }
        public required bool HasBread { get; set; }
        public required bool HasPolenta { get; set; }
    }
}