import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './components/orders/orders.component';
import { UnavailabilitiesComponent } from './components/unavailabilities/unavailabilities.component';


const ordersRoutes: Routes = [
  { path: '', redirectTo: '/orders', pathMatch: 'full' },
  {
    path: 'orders',
    data: {
      breadcrumb: 'Orders',
      canNavigateFromBreadcrumb: false,
    },
    component: OrdersComponent,
  },
  {
    path: 'unavailabilities',
    data: {
      breadcrumb: 'Unavailabilities',
      canNavigateFromBreadcrumb: false,
    },
    component: UnavailabilitiesComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(ordersRoutes)],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
