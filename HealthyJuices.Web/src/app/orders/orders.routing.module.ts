import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MyOrdersComponent } from './components/my-orders/my-orders.component';


const authRoutes: Routes = [
  {
    path: '',
    component: MyOrdersComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(authRoutes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
