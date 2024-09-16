export class DailyMenuDefinitionDto {
    menuIds: string[];
    menuType: number;
    dateTime: string;


    constructor(menuIds: string[], menuType: number, date: string) {
        this.menuIds = menuIds;
        this.menuType = menuType;
        this.dateTime = date;
    }
}

export class DailyMenuDefinitionOverviewDto extends DailyMenuDefinitionDto {
    menuName: string;
    dailyMenuName:string;
    price: number;

    constructor(menuIds: string[], menuType: number, dateTime: string, menuName: string, dailyMenuName:string, price: number) {
        super(menuIds, menuType, dateTime);
        this.menuName = menuName;
        this.price = price;
        this.dailyMenuName = dailyMenuName;
    }
}
