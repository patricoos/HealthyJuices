import { Company } from 'src/app/_shared/models/user/company.model';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompaniesService } from 'src/app/_shared/services/http/companies.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { ConfirmationService } from 'primeng/api';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';

@Component({
  selector: 'app-companies-form',
  templateUrl: './companies-form.component.html',
  styleUrls: ['./companies-form.component.scss']
})
export class CompaniesFormComponent implements AfterViewInit {
  companiesFormComponentLoader = 'companiesFormComponentLoader';

  lat = 52.22;
  lng = 21.01;

  selectedCompany: Company | undefined;

  id: string | undefined;
  private sub: any;
  editForm: FormGroup = this.initForm();

  constructor(private route: ActivatedRoute, private companService: CompaniesService, private toastsService: ToastsService,
    private confirmationService: ConfirmationService, private router: Router) { }

  ngAfterViewInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      if (this.id) {
        this.getDetails(this.id);
      }
    });
  }

  getDetails(id: string): void {
    this.companService.get(id, this.companiesFormComponentLoader).subscribe(x => {
      console.log(x);
      this.selectedCompany = x;
      this.editForm = this.initForm(x);
      this.lat = x.latitude;
      this.lng = x.longitude;
    }, error => this.toastsService.showError(error));
  }

  private initForm(company: Company | null = null): FormGroup {
    const form = new FormGroup({
      id: new FormControl(company ? company.id : null),

      name: new FormControl(company ? company.name : null, Validators.required),
      comment: new FormControl(company ? company.comment : null),

      postalCode: new FormControl(company ? company.postalCode : null),
      city: new FormControl(company ? company.city : null),
      street: new FormControl(company ? company.street : null),

      latitude: new FormControl(company ? company.latitude : this.lat),
      longitude: new FormControl(company ? company.longitude : this.lng),
    });
    return form;
  }

  onDragEnd(event: any): void {
    this.editForm.markAsDirty();
    this.editForm.controls.latitude.setValue(event.coords.lat);
    this.editForm.controls.longitude.setValue(event.coords.lng);
  }

  onSave(): void {
    if (this.editForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.editForm);
      this.toastsService.showError('Invalid Form');
      return;
    }
    this.companService.addOrEdit(this.editForm.value, this.companiesFormComponentLoader).subscribe(x => {
      this.toastsService.showSuccess(this.editForm.controls.name.value + ' updated!');
      this.router.navigate(['management/companies']);
    }, error => this.toastsService.showError(error));
  }

  onDelete(): void {
    if (!this.selectedCompany || !this.selectedCompany.id) { return; }
    this.confirmationService.confirm({
      message: 'Are you sure tha you want to delete',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.delete()
    });
  }

  private delete(): void {
    if (!this.selectedCompany || !this.selectedCompany.id) { return; }
    this.companService.delete(this.selectedCompany.id, this.companiesFormComponentLoader).subscribe(x => {
      this.toastsService.showSuccess(this.selectedCompany?.name + ' deteted!');
      this.router.navigate(['management/companies']);
    }, error => this.toastsService.showError(error));
  }
}
