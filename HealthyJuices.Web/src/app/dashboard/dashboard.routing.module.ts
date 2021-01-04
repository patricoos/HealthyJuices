import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const dashboardRoutes: Routes = [
  {
    path: '',
    data: {
      breadcrumb: 'Dashboard',
      canNavigateFromBreadcrumb: false,
    },
    component: DashboardComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(dashboardRoutes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
