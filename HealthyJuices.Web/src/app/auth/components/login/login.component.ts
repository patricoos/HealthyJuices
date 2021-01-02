import { ToastsService } from './../../../_shared/services/toasts.service';
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loader = false;
  error = false;

  loginForm: FormGroup = new FormGroup({
    login: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
    rememberme: new FormControl(true),
  });

  constructor(private authService: AuthService, public messageService: ToastsService) {
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.markControls();
      return;
    }
    this.loader = true;
    this.loginForm.disable();
    this.authService.login(this.loginForm.controls.login.value,
      this.loginForm.controls.password.value, this.loginForm.controls.rememberme.value).subscribe(x => {
        this.loader = false;
        this.loginForm.enable();
      }, error => {
        this.loader = false;
        this.loginForm.enable();
        this.error = true;
        this.messageService.showError(error);
      });
  }

  private markControls(): void {
    this.loginForm.controls.login.markAsDirty();
    this.loginForm.controls.login.markAsTouched();
    this.loginForm.controls.password.markAsDirty();
    this.loginForm.controls.password.markAsTouched();
  }
}
