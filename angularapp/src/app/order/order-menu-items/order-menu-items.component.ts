import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Items, MenuItem } from '../models/order';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

@Component({
  selector: 'app-order-menu-items',
  templateUrl: './order-menu-items.component.html',
  styleUrls: ['./order-menu-items.component.css']
})
export class OrderMenuItemsComponent {
  itemForm: FormGroup;
  items: MenuItem[];
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<OrderMenuItemsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { items: MenuItem[], selectedItem: Items | undefined, showdropDown: boolean, }
  ) {
    this.items = data.items;
    let menuItemIndex = data.items.findIndex(x => x.id == data.selectedItem?.menu.id)
    this.itemForm = this.fb.group({
      item: [menuItemIndex == -1? null : data.items[menuItemIndex], Validators.required],
      quantity: [data.selectedItem?.quantity ?? 1, [Validators.required, Validators.min(1)]],
      mentions: [data.selectedItem?.mention],
    });

  }
  selected(event: MatAutocompleteSelectedEvent): void {
    const selectedMenuId = event.option.value;
    const index = this.items.findIndex(x => x.id == selectedMenuId);
    if (index >= 0) {
      
    }


  }
  onSubmit(): void {
    if (this.itemForm.valid) {
      this.dialogRef.close(this.itemForm.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
