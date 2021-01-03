import { AbstractControl, ValidatorFn } from '@angular/forms';
import { map } from 'rxjs/operators';
import { UsersService } from 'src/app/_shared/services/http/users.service';
import { RegisterComponent } from '../components/register/register.component';

export class UserValidators {
  // public emailAsyncValidator(service: UsersService, component: RegisterComponent): ValidatorFn {
  //   return (control: AbstractControl) => {
  //     return new Promise((resolve) => {
  //       if (!control.value) {
  //         return resolve(null);
  //       } else {
  //         service.IsExisting(component.registerForm.controls.email.value).subscribe((exists: boolean) => {
  //           console.log(exists);
  //           if (exists) {
  //             return resolve({ usernameExist: true });
  //           } else {
  //             return resolve(null);
  //           }
  //         });
  //       }
  //     });
  //   };
  // }

  public static checkUserNameTaken(service: UsersService): any {
    return (control: AbstractControl) => {
      return service.IsExisting(control.value)
        .pipe(map((isTaken) => (isTaken ? { usernameExist: true } : null)));
    };
  }
}
