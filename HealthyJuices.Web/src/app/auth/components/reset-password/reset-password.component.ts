import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordComponentLoader = 'resetPasswordComponentLoader';

  restForm: FormGroup = new FormGroup({
    email: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
    connfirmPassword: new FormControl(null, Validators.required),
    token: new FormControl(null),
  }, this.passwordConfirmingValidator);

  constructor(private authService: AuthService, public messageService: ToastsService, private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.restForm.disable();
    const token = this.activatedRoute.snapshot.queryParamMap.get('token');
    const email = this.activatedRoute.snapshot.queryParamMap.get('email');

    if (token && email) {
      this.restForm.controls.token.setValue(token);
      this.restForm.controls.email.setValue(email);
      this.restForm.enable();
    }
  }

  onSubmit(): void {
    if (this.restForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.restForm);
      this.messageService.showError('Invalid Form');
      return;
    }
    this.authService.resetPassword(this.resetPasswordComponentLoader, this.restForm.value).subscribe(x => {
      this.router.navigate(['/auth/login']);
    },
      error => this.messageService.showError(error));
  }



  onBack(): void {
    this.router.navigate(['/auth/login']);
  }

  passwordConfirmingValidator(control: AbstractControl): any {
    if (control && control.get('password')?.value !== control.get('connfirmPassword')?.value) {
      return { invalidConnfirmPassword: true };
    }
  }
}
