import { TableModule } from 'primeng/table';
import { CalendarModule } from 'primeng/calendar';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SharedModule } from '../_shared/shared.module';
import { AgmCoreModule } from '@agm/core';
import { environment } from 'src/environments/environment';
import { FormsModule } from '@angular/forms';
import { DashboardRoutingModule } from './dashboard.routing.module';
import { ChartModule } from 'primeng/chart';


@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    SharedModule,
    CalendarModule,
    DashboardRoutingModule,
    FormsModule,
    TableModule,
    ChartModule,
    AgmCoreModule.forRoot({
      apiKey: environment.googleMapsApiKey
    }),
  ]
})
export class DashboardModule { }
