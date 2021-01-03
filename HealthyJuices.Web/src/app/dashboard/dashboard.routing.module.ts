import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const ordersRoutes: Routes = [
  {
    path: '',
    data: {
      breadcrumb: 'Unavailabilities',
      canNavigateFromBreadcrumb: false,
    },
    component: DashboardComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(ordersRoutes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
