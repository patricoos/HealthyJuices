import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthRoutingModule } from './auth.routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../_shared/shared.module';
import { ToastModule } from 'primeng/toast';
import { ConfirmRegisterComponent } from './components/confirm-register/confirm-register.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    ConfirmRegisterComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AuthRoutingModule,
    FormsModule,
    ToastModule,
    ReactiveFormsModule,
  ]
})
export class AuthModule { }
