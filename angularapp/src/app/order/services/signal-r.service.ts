import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/enviroments/enviroments';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;

  constructor(private snackBar: MatSnackBar) {
   // this.startConnection();
    //this.addOrderListener();
  }

  // Initialize the SignalR connection
  private startConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/orderHub`) // Replace with your SignalR hub URL
      .build();
      this.hubConnection.onclose(async (error) => {
        setTimeout(() => this.addOrderListener(), 5000);
    });
    this.hubConnection
      .start()
      .catch(err => console.error('Error while starting SignalR connection: ', err));
  }

  // Add a listener for the "NewOrder" event
  private addOrderListener(): void {
    this.hubConnection.on('NewOrder', (order) => {
      this.showSnackbar(`New order created: ${order.id}`);
    });
  }

  // Display a snackbar notification
  private showSnackbar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000, // Duration in milliseconds
      verticalPosition: 'top' // Change this as needed (top, bottom)
    });
  }
}
