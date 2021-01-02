import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmptyLayoutComponent } from './_shared/components/empty-layout/empty-layout.component';
import { MainLayoutComponent } from './_shared/components/main-layout/main-layout.component';
import { UserRole } from './_shared/models/enums/user-role.enum';
import { AuthGuardsService } from './_shared/services/auth-guards.service';

const NO_LAYOUT_ROUTES: Routes = [
  { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
  { path: 'error', loadChildren: () => import('./error-pages/error-pages.module').then(m => m.ErrorPagesModule) },
  { path: '**', redirectTo: '/error/404', pathMatch: 'full' },
];

const MAIN_LAYOUT_ROUTES: Routes = [
  { path: '', redirectTo: '/orders', pathMatch: 'full' },
  {
    path: 'orders', data: {
      breadcrumb: 'Orders',
      canNavigateFromBreadcrumb: false,
      expectedRoles: [UserRole.Customer]
    },
    loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule)
  },
  {
    path: 'management',
    canActivate: [AuthGuardsService],
    data: {
      breadcrumb: 'Management',
      canNavigateFromBreadcrumb: false,
      expectedRoles: [UserRole.BusinessOwner]
    },
    loadChildren: () => import('./management/management.module').then(m => m.ManagementModule)
  },
];

const APP_ROUTES: Routes = [
  {
    path: '',
    canLoad: [AuthGuardsService],
    canActivate: [AuthGuardsService],
    component: MainLayoutComponent,
    children: MAIN_LAYOUT_ROUTES,
    data: { expectedRoles: [UserRole.BusinessOwner, UserRole.Customer] }
  },
  {
    path: '',
    component: EmptyLayoutComponent,
    children: NO_LAYOUT_ROUTES
  }
];

@NgModule({
  imports: [RouterModule.forRoot(APP_ROUTES)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
