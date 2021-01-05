import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  forgotPasswordComponentLoader = 'forgotPasswordComponentLoader';

  forgotForm: FormGroup = new FormGroup({
    email: new FormControl(null, Validators.required),
  });

  constructor(private authService: AuthService, public messageService: ToastsService, private router: Router) { }

  onSubmit(): void {
    if (this.forgotForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.forgotForm);
      this.messageService.showError('Invalid Form');
      return;
    }
    this.authService.forgotPassword(this.forgotPasswordComponentLoader, this.forgotForm.value).subscribe(x => {
      this.router.navigate(['/auth/login']);
    },
      error => this.messageService.showError(error));
  }

  onBack(): void {
    this.router.navigate(['/auth/login']);
  }
}
