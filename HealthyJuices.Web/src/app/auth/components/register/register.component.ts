import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { UsersService } from 'src/app/_shared/services/http/users.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';
import { RegisterUser } from '../../models/register-user.model';
import { UserValidators } from '../../validators/user.validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerLoader = 'loginLoader';

  registerForm: FormGroup = new FormGroup({
    email: new FormControl(null, [Validators.required]),
    password: new FormControl(null, Validators.required),
    firstName: new FormControl(null),
    lastName: new FormControl(null),
    connfirmPassword: new FormControl(null, Validators.required),
  }, this.passwordConfirmingValidator);

  constructor(private authService: AuthService, public messageService: ToastsService, private router: Router) { }

  onSubmit(): void {
    console.log(this.registerForm.invalid);
    if (this.registerForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.registerForm);
      this.messageService.showError('Invalid Form');
      return;
    }
    this.authService.register(this.registerLoader, this.registerForm.value).subscribe(x => {
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
