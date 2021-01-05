import { SelectItem } from 'primeng/api';
import { Company } from './../../../_shared/models/user/company.model';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { UsersService } from 'src/app/_shared/services/http/users.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';
import { RegisterUser } from '../../models/register-user.model';
import { UserValidators } from '../../validators/user.validator';
import { CompaniesService } from 'src/app/_shared/services/http/companies.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements AfterViewInit {
  registerLoader = 'loginLoader';

  registerForm: FormGroup = new FormGroup({
    email: new FormControl(null, [Validators.required]),
    password: new FormControl(null, Validators.required),
    firstName: new FormControl(null),
    lastName: new FormControl(null),
    connfirmPassword: new FormControl(null, Validators.required),
    companyId: new FormControl(null, Validators.required),
  }, this.passwordConfirmingValidator);

  companies: SelectItem[] = [];

  constructor(private authService: AuthService, public messageService: ToastsService, private router: Router,
    private companiesService: CompaniesService) { }

  ngAfterViewInit(): void {
    this.companiesService.getAllActive(this.registerLoader).subscribe(x => {
      this.companies = x.map(p => ({ label: p.name, value: p.id }));;
    }, error => this.messageService.showError(error));
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.registerForm);
      this.messageService.showError('Invalid Form');
      return;
    }
    this.authService.register(this.registerLoader, this.registerForm.value).subscribe(x => {
      this.router.navigate(['/auth/login']);
    }, error => this.messageService.showError(error));
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
