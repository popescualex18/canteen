import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order';

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.css']
})
export class InvoiceComponent implements OnInit {
  order: Order | undefined = undefined;
  constructor(private route: ActivatedRoute, private orderService: OrderService){}
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.orderService.getOrders().subscribe({
      next : (result) => {
        this.order = result.find(order => order.id === id);
        console.log(this.order)
        let sum = 0;
        this.order!.items.forEach(element => {
          sum += element!.menu!.price! * element!.quantity
        });
        this.order!.amount = sum;
        
      }
    })
  }

}
