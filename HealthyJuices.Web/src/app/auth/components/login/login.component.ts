import { ToastsService } from './../../../_shared/services/toasts.service';
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginLoader = 'loginLoader';

  loginForm: FormGroup = new FormGroup({
    email: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
    rememberme: new FormControl(true),
  });

  constructor(private authService: AuthService, public messageService: ToastsService, private router: Router) { }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.markControls();
      return;
    }
    this.authService.login(this.loginLoader, this.loginForm.controls.email.value,
      this.loginForm.controls.password.value, this.loginForm.controls.rememberme.value).subscribe(x => { },
        error => this.messageService.showError(error));
  }

  private markControls(): void {
    this.loginForm.controls.email.markAsDirty();
    this.loginForm.controls.email.markAsTouched();
    this.loginForm.controls.password.markAsDirty();
    this.loginForm.controls.password.markAsTouched();
  }

  onRegister(): void {
    this.router.navigate(['/auth/register']);
  }
}
