using SCNeagtovo.Common.Enum;

namespace SCNeagtovo.Common
{
    public static class Constants
    {
        public static Dictionary<MenuType, string> DefineMenuCategories = new Dictionary<MenuType, string>() 
        {
            { MenuType.DailyMenu, "Meniul zilei" },
            { MenuType.ChoiceMenu, "Meniu la alegere" },
            { MenuType.SoupMenu, "Supe/Ciorbe" },
            { MenuType.SaladsMenu, "Salate" },
            { MenuType.MeatMenu, "Carne de pui & porc" },
            { MenuType.SidesMenu, "Garnituri" },
            { MenuType.OtherMenu, "Diverse" },
        };
    }

}
