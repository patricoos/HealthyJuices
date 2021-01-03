import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MyOrdersComponent } from './components/my-orders/my-orders.component';
import { OrdersRoutingModule } from './orders.routing.module';
import { SharedModule } from '../_shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    MyOrdersComponent
  ],
  imports: [
    CommonModule,
    OrdersRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    ConfirmDialogModule,
    SharedModule,
  ],
  providers: [
  ]
})
export class OrdersModule { }
