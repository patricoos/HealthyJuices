import { CompaniesComponent } from './components/companies/companies.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './components/orders/orders.component';
import { UnavailabilitiesComponent } from './components/unavailabilities/unavailabilities.component';
import { CompaniesFormComponent } from './components/companies/companies-form/companies-form.component';
import { OrdersFormComponent } from './components/orders/orders-form/orders-form.component';


const ordersRoutes: Routes = [
  { path: '', redirectTo: '/orders', pathMatch: 'full' },
  {
    path: 'orders',
    data: {
      breadcrumb: 'Orders',
      canNavigateFromBreadcrumb: true,
    },
    children: [
      { path: '', component: OrdersComponent },
      { path: 'add', data: { breadcrumb: 'Add' }, component: OrdersFormComponent },
      { path: ':id', data: { breadcrumb: 'Edit' }, component: OrdersFormComponent },
    ]
  },
  {
    path: 'unavailabilities',
    data: {
      breadcrumb: 'Unavailabilities',
      canNavigateFromBreadcrumb: false,
    },
    component: UnavailabilitiesComponent,
  },
  {
    path: 'companies',
    data: {
      breadcrumb: 'Companies',
      canNavigateFromBreadcrumb: true,
    },
    children: [
      { path: '', component: CompaniesComponent },
      { path: 'add', data: { breadcrumb: 'Add' }, component: CompaniesFormComponent },
      { path: ':id', data: { breadcrumb: 'Edit' }, component: CompaniesFormComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ordersRoutes)],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
