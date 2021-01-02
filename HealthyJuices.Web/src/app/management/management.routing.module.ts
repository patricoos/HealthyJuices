import { BreadCrumb } from './../_shared/utils/bread-crumb.model';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './components/orders/orders.component';


const ordersRoutes: Routes = [
  { path: '', redirectTo: '/orders', pathMatch: 'full' },
  {
    path: 'orders',
    data: {
      breadcrumb: 'Orders',
      canNavigateFromBreadcrumb: false,
    },
    component: OrdersComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(ordersRoutes)],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
