import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { OrderMenuItemsComponent } from '../order-menu-items/order-menu-items.component';
import { DailyMenuDefinitionDto } from 'src/app/menu/models/daily-menu-definition';
import { MenuItem, Order, Items } from '../models/order';
import { MenuService } from 'src/app/menu/services/menu.service';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { OrderService } from '../services/order.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { v4 as uuidv4 } from 'uuid';
import { OrderStatus } from '../enum/order-status';
import { MenuType } from 'src/app/common/category-enum';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent implements OnInit {
  order: Order | undefined;
  orderForm: FormGroup;
  selectedDate: Date | null = new Date(2024, 2, 1);
  dailyMenuDefinitions: DailyMenuDefinitionDto[] = [];
  filteredMenues: MenuItem[] = [];
  menuData: MenuItem[] = [];
  editable: boolean = false;

  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private nav: Router,
    private menuService: MenuService,
    private fb: FormBuilder,
    private orderService: OrderService,
    private snackBar: MatSnackBar
  ) {
    this.orderForm = this.fb.group({
      client: [null, [Validators.required, Validators.minLength(3)]],
      mobile: [null, romanianPhoneNumberValidator],
      address: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadInitialData();
  }

  private loadInitialData(): void {
    this.getDailyMenuDefinitions();
    this.getMenues();

    const id = this.route.snapshot.paramMap.get('id');
    this.editable = this.route.snapshot.queryParams['editable'] === 'true';

    if (!id) {
      this.initializeNewOrder();
      return;
    }

    this.orderService.getOrders().subscribe({
      next: (orders) => {
        this.order = orders.find(order => order.id === id);
        if (this.order) {
          this.updateOrderForm();
          this.calculateOrderAmount();
        }
      },
      error: (error) => console.error('Error fetching orders', error)
    });
  }

  private initializeNewOrder(): void {
    this.order = new Order(
      uuidv4(),
      1,
      new Date().toLocaleDateString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric' }),
      [],
      0,
      '',
      undefined,
      '',
      '',
      OrderStatus.Recived,
      true
    );
  }

  private updateOrderForm(): void {
    this.orderForm.setValue({
      client: this.order?.client,
      mobile: this.order?.mobile,
      address: this.order?.address
    });
  }

  private calculateOrderAmount(): void {
    if (this.order) {
      this.order.amount = this.order.items.reduce((sum, item) => sum + (item.menu.price! * item.quantity), 0);
    }
  }

  private getDailyMenuDefinitions(): void {
    const formattedDate = this.selectedDate!.toLocaleDateString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric' });
    this.menuService.getDailyMenuDefinitions(formattedDate).subscribe(
      menuDefinitions => this.dailyMenuDefinitions = menuDefinitions,
      error => console.error('Error fetching daily menu definitions', error)
    );
  }

  private getMenues(): void {
    this.menuService.getMenu().subscribe(menuData => {
      this.menuData = menuData;
      const menuIds = this.dailyMenuDefinitions
        .filter(x => x.menuType !== MenuType.DailyMenu)
        .flatMap(x => x.menuIds);
      this.filteredMenues = [...this.menuData.filter(x => menuIds.includes(x.id))];
    });
  }

  filterResults(text: string): void {
    if (!text) {
      this.filteredMenues = this.menuData;
      return;
    }

    this.filteredMenues = this.menuData.filter(
      menu => menu?.name.toLowerCase().includes(text.toLowerCase())
    );
  }

  remove(menuId: string): void {
    if (this.order) {
      const index = this.order.items.findIndex(item => item.menu.id === menuId);
      if (index >= 0) {
        this.order.items.splice(index, 1);
        this.calculateOrderAmount();
      }
    }
  }

  edit(item: Items, index: number): void {
    const dialogRef = this.dialog.open(OrderMenuItemsComponent, {
      width: '400px',
      data: { 
        items: this.filteredMenues, 
        selectedItem: new Items(item.menu, item.quantity, item.mention), 
        showdropDown: true 
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.order!.amount -= item.menu.price! * item.quantity;
        this.order!.items[index] = new Items(result.item, result.quantity, result.mentions);
        this.order!.amount += result.item.price * result.quantity;
      }
    });
  }

  add(item: MenuItem): void {
    const dialogRef = this.dialog.open(OrderMenuItemsComponent, {
      width: '400px',
      data: { 
        items: this.filteredMenues, 
        selectedItem: new Items(item, 1, "") 
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.order?.items.push(new Items(item, result.quantity, result.mentions));
        this.calculateOrderAmount();
      }
    });
  }

  saveOrder(): void {
    if (!this.orderForm.valid) {
      return;
    }
    this.order = { ...this.order, ...this.orderForm.value };
    this.orderService.addOrder(this.order!).subscribe({
      next: (orderId) => {
        this.snackBar.open('Order created successfully.', '×', {
          verticalPosition: 'bottom',
          duration: 3000,
          panelClass: ['success']
        });
        this.nav.navigate(['/order-detail', orderId], { queryParams: { editable: true } });
      },
      error: (error) => console.error('Error saving order', error)
    });
  }

  updateOrder(): void {
    if (!this.orderForm.valid) {
      return;
    }
    this.order = { ...this.order, ...this.orderForm.value };
    this.orderService.updateOrder(this.order!).subscribe({
      next: () => {
        this.snackBar.open('Order updated successfully.', '×', {
          verticalPosition: 'bottom',
          duration: 3000,
          panelClass: ['success']
        });
        this.order!.isNew = false;
      },
      error: (error) => console.error('Error updating order', error)
    });
  }

  printReceipt(): void {
    if (this.order) {
      this.orderService.downloadScreenshot(this.order.id).subscribe(
        (blob: Blob) => {
          const url = window.URL.createObjectURL(blob);
          const iframe = document.createElement('iframe');
          iframe.style.display = 'none';
          iframe.src = url;
          document.body.appendChild(iframe);
          iframe.onload = () => {
            setTimeout(() => {
              iframe.contentWindow?.print();
              document.body.removeChild(iframe);
              window.URL.revokeObjectURL(url);
            }, 100);
          };
        },
        error => console.error('Error downloading receipt', error)
      );
    }
  }

  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.replace(/\D/g, '');
    this.orderForm.get('mobile')?.setValue(input.value, { emitEvent: false });
  }
}

export function romanianPhoneNumberValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }
    const regex = /^(?:\+40|0040)?07\d{8}$/;
    return regex.test(control.value) ? null : { invalidPhoneNumber: true };
  };
}
