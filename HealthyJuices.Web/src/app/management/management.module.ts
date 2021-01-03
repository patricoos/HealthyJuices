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
import { CompaniesComponent } from './components/companies/companies.component';
import { CompaniesFormComponent } from './components/companies/companies-form/companies-form.component';
import { OrdersFormComponent } from './components/orders/orders-form/orders-form.component';
import { environment } from 'src/environments/environment';
import { AgmCoreModule } from '@agm/core';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';

@NgModule({
  declarations: [
    OrdersComponent,
    UnavailabilitiesComponent,
    UnavailabilitEditFormModalComponent,
    CompaniesComponent,
    CompaniesFormComponent,
    OrdersFormComponent
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
    DropdownModule,
    AgmCoreModule.forRoot({
      apiKey: environment.googleMapsApiKey
    }),
    ConfirmDialogModule
  ],
  providers: [
    DialogService,
    ConfirmationService
  ],
  entryComponents: [
    UnavailabilitEditFormModalComponent
  ]
})
export class ManagementModule { }
