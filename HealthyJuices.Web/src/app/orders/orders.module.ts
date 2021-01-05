import { TableModule } from 'primeng/table';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MyOrdersComponent } from './components/my-orders/my-orders.component';
import { OrdersRoutingModule } from './orders.routing.module';
import { SharedModule } from '../_shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';


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
    CalendarModule,
    TableModule,
    DropdownModule
  ],
  providers: [
  ]
})
export class OrdersModule { }
