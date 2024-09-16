export enum OrderStatus {
    Pending = 1,
    Recived = 2,
    InTransport = 3,
    Delivered = 4,
}
export const OrderStatusDescirptions: { [key in OrderStatus]: string } = {
    [OrderStatus.Pending]: 'In asteptare',
    [OrderStatus.Recived]: 'Comanda acceptata',
    [OrderStatus.InTransport]: 'Livrare',
    [OrderStatus.Delivered]: 'Livrata'
  };