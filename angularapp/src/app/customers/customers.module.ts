import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomersRoutingModule } from './customers-routing.module';
import { CustomersOverviewComponent } from './customers-overview/customers-overview.component';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { ManageCustomerComponent } from './manage-customer/manage-customer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    CustomersOverviewComponent,
    ManageCustomerComponent
  ],
  imports: [
    CommonModule,
    CustomersRoutingModule,
    MatCardModule,
    MatIconModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    HttpClientModule,
  ]
})
export class CustomersModule { }
