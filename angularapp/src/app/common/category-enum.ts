export enum CategoryEnum {
    Soup = 1,
    Salad = 2,
    Sides = 3,
    Meat = 4,
    Others = 5,
}
export enum MenuType
{
    DailyMenu,
    ChoiceMenu,
    SoupMenu,
    SidesMenu, 
    SaladsMenu,
    MeatMenu,
    OtherMenu
}
export const MenuTypeDescriptions: { [key in MenuType]: string } = {
    [MenuType.DailyMenu]: 'Meniul zilei',
    [MenuType.ChoiceMenu]: 'Meniu la alegere',
    [MenuType.SoupMenu]: 'Ciorbe/Supe',
    [MenuType.SidesMenu]: 'Garnituri',
    [MenuType.SaladsMenu]: 'Salate',
    [MenuType.MeatMenu]: 'Pui&Porc',
    [MenuType.OtherMenu]: 'Diverse'
  };