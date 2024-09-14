namespace SCNeagtovo.Dtos.Models.Menu
{
    public class DailMenuDefinitionOverviewDto : DailyMenuDefinitionDto
    {
        public string MenuName { get; set; }
        public double Price { get; set; }
        public string DailyMenuName { get; set; }

        public static string BuildMenuName(string name, bool hasPolenta, bool hasBread)
        {
            if (hasBread)
            {
                return $"{name} si painica";
            }
            if (hasPolenta)
            {
                return $"{name} cu mamaliguta";

            }

            return name;
        }

    }
}
