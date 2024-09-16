import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  constructor(private snackBar: MatSnackBar) { }

  public show(message:string, type:string){
    this.snackBar.open(message, '×', {
      verticalPosition: 'bottom',
      duration: 3000,
      panelClass: [type] 
    });
  }
}
