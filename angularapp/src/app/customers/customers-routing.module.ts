import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomersOverviewComponent } from './customers-overview/customers-overview.component';

const routes: Routes = [ { path: '', component: CustomersOverviewComponent, pathMatch: 'full'  },];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule { }
