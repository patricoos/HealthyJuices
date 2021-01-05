import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MainLayoutComponent } from './components/main-layout/main-layout.component';
import { HeaderComponent } from './components/main-layout/header/header.component';
import { FooterComponent } from './components/main-layout/footer/footer.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LoaderComponent } from './components/loader/loader.component';
import { BreadcrumbComponent } from './components/main-layout/breadcrumb/breadcrumb.component';
import { SpaceBeforeCapitalPipe } from './pipes/space-before-capital.pipe';



@NgModule({
  declarations: [
    MainLayoutComponent,
    HeaderComponent,
    FooterComponent,
    EmptyLayoutComponent,
    LoaderComponent,
    BreadcrumbComponent,
    SpaceBeforeCapitalPipe
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    LoaderComponent,
    SpaceBeforeCapitalPipe
  ]
})
export class SharedModule { }
