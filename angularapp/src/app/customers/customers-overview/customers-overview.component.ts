import { Component, ElementRef, ViewChild } from '@angular/core';
import { CustomerModel } from '../models/customer-model';
import { MatDialog } from '@angular/material/dialog';
import { ManageCustomerComponent } from '../manage-customer/manage-customer.component';
import { AuthService } from 'src/app/shared/services/auth/auth.service';

@Component({
  selector: 'app-customers-overview',
  templateUrl: './customers-overview.component.html',
  styleUrls: ['./customers-overview.component.css']
})
export class CustomersOverviewComponent {

  constructor(private dialog: MatDialog, private authService: AuthService) { }

  @ViewChild('filter') filterInput!: ElementRef<HTMLInputElement>;
  filteredMenuData: CustomerModel[] = [];
  data: CustomerModel[] = [];
  filterResults(text: string) {
    if (!text) {
      this.filteredMenuData = this.data;
      return;
    }
  }
  addNew() {
    const dialogRef = this.dialog.open(ManageCustomerComponent, {
      width: '600px',

    });

    dialogRef.afterClosed().subscribe(result => {

    });
  }
}
