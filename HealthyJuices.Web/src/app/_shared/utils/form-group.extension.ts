import { FormGroup } from '@angular/forms';

export class FormGroupExtension {
  static markFormAssDirtyAndTouched(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      form.controls[key].markAsDirty();
      form.controls[key].markAsTouched();
      if (((form.controls[key] as FormGroup).controls)) {
        this.markFormAssDirtyAndTouched(form.controls[key] as FormGroup);
      }
    });
  }

  static markFormAssDirty(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      form.controls[key].markAsDirty();
      if (((form.controls[key] as FormGroup).controls)) {
        this.markFormAssDirty(form.controls[key] as FormGroup);
      }
    });
  }

  static markFormAssTouched(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      form.controls[key].markAsTouched();
      if (((form.controls[key] as FormGroup).controls)) {
        this.markFormAssTouched(form.controls[key] as FormGroup);
      }
    });
  }
}
