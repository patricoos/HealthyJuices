import { SharedModule } from './../_shared/shared.module';
import { ManagementRoutingModule } from './management.routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './components/orders/orders.component';
import { TableModule } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';

@NgModule({
  declarations: [
    OrdersComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ManagementRoutingModule,
    TableModule,
    FormsModule,
    ReactiveFormsModule,
    MultiSelectModule,
    CalendarModule
  ]
})
export class ManagementModule { }
