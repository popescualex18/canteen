import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Order } from '../models/order';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { OrderService } from '../services/order.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';
import { MatSelectChange } from '@angular/material/select';
import { SignalRService } from '../services/signal-r.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  private Orders : Order[] = [];
  constructor(private router: Router, private orderService: OrderService, private snackBar: MatSnackBar, private signalRService: SignalRService) {}
  ngOnInit(): void {
    this.orderService.getOrders().subscribe({
      next : (result) => {
        this.Orders.splice(0);
        this.Orders.push(...result);
        this.initDataSource(this.Orders);
      }
    })
  }
  getOrderTotal(order:Order) {
    let sum = 0;
    order.items.forEach(element => {
      sum += element!.menu!.price! * element!.quantity
    });
    return sum;
  }

  displayedColumns: string[] = ['orderNr', 'customer', 'date', 'total','actions'];
  dataSource!: MatTableDataSource<Order>;
  @ViewChild(MatPaginator, {static: true}) paginator!: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort!: MatSort;

  public initDataSource(data:any){
    this.dataSource = new MatTableDataSource(data);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    
  }


  view(order: Order): void {
    this.router.navigate(['/order-detail', order.id],{
      queryParams: { editable: true }
    });
  }

  delete(order: Order): void {
    this.orderService.deleteOrder(order!.id).subscribe({
      next: () => {
        this.snackBar.open('Comanda a fost stearsa cu succes.', '×', {
          verticalPosition: 'bottom',
          duration: 3000,
          panelClass: ['success'] 
        });
        this.ngOnInit();
      },
      error: () => {
        this.snackBar.open('Comanda nu a fost stearsa', '×', {
          verticalPosition: 'bottom',
          duration: 3000,
          panelClass: ['error'] 
        });
      }
    });
    
  }
  addOrder(): void {
    this.router.navigate(['/order-detail'],{
      queryParams: { editable: true }
    });
  }
}
