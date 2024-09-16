import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AddComponent } from './add/add.component';
import { CreateDailyMenuComponent } from './create-daily-menu/create-daily-menu.component';
import { DailyMenuOverviewComponent } from './daily-menu-overview/daily-menu-overview.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full'  },
  { path: 'daily-menu', component: CreateDailyMenuComponent}, 
  { path: 'daily-menu-overview', component: DailyMenuOverviewComponent}, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MenuRoutingModule { }
