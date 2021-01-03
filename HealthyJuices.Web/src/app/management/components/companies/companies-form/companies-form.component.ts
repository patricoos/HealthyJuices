import { Company } from 'src/app/_shared/models/user/company.model';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompaniesService } from 'src/app/_shared/services/http/companies.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { NULL_EXPR } from '@angular/compiler/src/output/output_ast';
import { ConfirmationService } from 'primeng/api';

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

  id: number | undefined;
  private sub: any;
  editForm: FormGroup = this.initForm();

  constructor(private route: ActivatedRoute, private companService: CompaniesService, private toastsService: ToastsService,
    private confirmationService: ConfirmationService) { }

  ngAfterViewInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id'];
      if (this.id) {
        this.getDetails(this.id);
      }
    });
  }

  getDetails(id: number): void {
    this.companService.Get(id, this.companiesFormComponentLoader).subscribe(x => {
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

      postalCode: new FormControl(company ? company.comment : null),
      city: new FormControl(company ? company.comment : null),
      street: new FormControl(company ? company.comment : null),

      latitude: new FormControl(company ? company.comment : null),
      longitude: new FormControl(company ? company.comment : null),
    });
    return form;
  }

  onDelete(): void { }
  onDragEnd(event: any): void { }
  onSave(): void { }
  onCancel(): void { }
}
