import { SharedModule } from './../_shared/shared.module';
import { ManagementRoutingModule } from './management.routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './components/orders/orders.component';
import { TableModule } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { UnavailabilitiesComponent } from './components/unavailabilities/unavailabilities.component';
import { FullCalendarModule } from 'primeng/fullcalendar';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { UnavailabilitEditFormModalComponent } from './components/unavailabilities/unavailabilit-edit-form-modal/unavailabilit-edit-form-modal.component';
import { DropdownModule } from 'primeng/dropdown';

@NgModule({
  declarations: [
    OrdersComponent,
    UnavailabilitiesComponent,
    UnavailabilitEditFormModalComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ManagementRoutingModule,
    TableModule,
    FormsModule,
    ReactiveFormsModule,
    MultiSelectModule,
    CalendarModule,
    FullCalendarModule,
    DynamicDialogModule,
    DropdownModule
  ],
  providers: [
    DialogService
  ],
  entryComponents: [
    UnavailabilitEditFormModalComponent
  ]
})
export class ManagementModule { }
