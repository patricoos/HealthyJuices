
import { Page401Component } from './components/page401/page401.component';
import { Page404Component } from './components/page404/page404.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

const errorRoutes: Routes = [
    { path: '', redirectTo: '/404', pathMatch: 'full' },
    { path: '401', component: Page401Component },
    { path: '404', component: Page404Component },
    { path: 'unauthorized', redirectTo: '/401', pathMatch: 'full' },
    { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forChild(errorRoutes)],
    exports: [RouterModule]
})
export class ErrorPagesRoutingModule { }
