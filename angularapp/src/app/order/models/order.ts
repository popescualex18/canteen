import { OrderStatus } from "../enum/order-status";

export class Order {
    constructor(public id: string,
                public orderNumber: number,
                public date: string,
                public items: Items[],
                public amount: number,
                public client: string, 
                public mobile: string | undefined,
                public address: string,
                public delivery:string,
                public orderStatus: OrderStatus,
              public isNew: boolean){}
    
                
}  
export class MenuItem {
  id: string;
  name: string;
  price: number | null;
  categoryId: number | null;
  hasBread: boolean;
  hasPolenta: boolean;

  constructor(
    id: string,
    name: string,
    price: number | null,
    categoryId: number | null,
    hasBread: boolean,
    hasPolenta: boolean
  ) {
    this.id = id;
    this.name = name;
    this.price = price;
    this.categoryId = categoryId;
    this.hasBread = hasBread;
    this.hasPolenta = hasPolenta;
  }
}

export class Items {
  constructor(public menu: MenuItem,
    public quantity: number, public mention: string){};

}

export const DailyMenu: MenuItem = new MenuItem(
  '0bfe24f4-bd5b-4cd6-ba7d-59e1bfe481a8',
  "Meniul zilei",
  22,
  6,
  false,
  false
);

