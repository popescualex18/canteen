import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators'; // Adjust the import path as necessary
import { Order } from '../models/order';
import { HttpService } from 'src/app/shared/services/http/http.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private ordersUrl = 'order';  // URL to web API
  screenshortUrl = 'Screenshot/print-order';
  constructor(private http: HttpService) { }

  /** GET orders from the server */
  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.ordersUrl}/get-all`)
      .pipe(
        catchError(this.handleError<Order[]>('getOrders', []))
      );
  }

  /** GET order by id. Will 404 if id not found */
  getOrder(id: number): Observable<Order> {
    const url = `${this.ordersUrl}/${id}`;
    return this.http.get<Order>(url).pipe(
      catchError(this.handleError<Order>(`getOrder id=${id}`))
    );
  }

  /** POST: add a new order to the server */
  addOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(`${this.ordersUrl}/create-order`, order).pipe(
      catchError(this.handleError<Order>('addOrder'))
    );
  }

  /** PUT: update the order on the server */
  updateOrder(order: Order): Observable<any> {
    return this.http.post(`${this.ordersUrl}/update-order`, order).pipe(
      catchError(this.handleError<any>('updateOrder'))
    );
  }

  /** DELETE: delete the order from the server */
  deleteOrder(id: string): Observable<Order> {
    const url = `${this.ordersUrl}/delete/${id}`;
    return this.http.delete<Order>(url).pipe(
      catchError(this.handleError<Order>('deleteOrder'))
    );
  }
  downloadScreenshot(id: string): Observable<Blob> {
    return this.http.getImage(`${this.screenshortUrl}/${id}`);
  }
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      throw error; // Let the app keep running by returning an empty result.
    };
  }

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
}
