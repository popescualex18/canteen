import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-daily-menu-order-dialog',
  templateUrl: './daily-menu-order-dialog.component.html',
  styleUrls: ['./daily-menu-order-dialog.component.css']
})
export class DailyMenuOrderDialogComponent<T> {


  constructor(public dialog: MatDialog, 
    public dialogRef: MatDialogRef<DailyMenuOrderDialogComponent<T>>,
    @Inject(MAT_DIALOG_DATA) public data: { items: T[], selector: (item:T) => string }) {}

  onNoClick(): void {
    this.dialogRef.close();

  }
  dragStart(event: DragEvent, index: number) {
    event!.dataTransfer!.setData('text/plain', index.toString());
  }
  dragOver(event: DragEvent) {
    event.preventDefault();
  }

  drop(event: DragEvent, index: number) {
    event.preventDefault();
    const fromIndex = parseInt(event.dataTransfer!.getData('text/plain'), 10);
    
    if (fromIndex !== index) {
      const temp = this.data.items[fromIndex];
      this.data.items.splice(fromIndex, 1);
      this.data.items.splice(index, 0, temp);
    }
  }
}
