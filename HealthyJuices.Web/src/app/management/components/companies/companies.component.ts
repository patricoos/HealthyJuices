import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { Company } from 'src/app/_shared/models/user/company.model';
import { CompaniesService } from 'src/app/_shared/services/http/companies.service';
import { TableQueryService } from 'src/app/_shared/services/table.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.scss']
})
export class CompaniesComponent implements OnInit {
  companiesComponentLoader = 'companiesComponentLoader';

  companies: Company[] = [];
  columns = [
    { field: 'name', header: 'Name' },
    { field: 'postalCode', header: 'Postal Code' },
    { field: 'city', header: 'City' },
    { field: 'street', header: 'Street' },
    { field: 'dateCreated', header: 'Created' },
    { field: 'action', header: 'Action', width: '145px' },
  ];
  calendarLocale = FullCallendarConsts.getCallendarLocale();

  constructor(private companiesService: CompaniesService, private toastsService: ToastsService,
    private tableQueryService: TableQueryService, private router: Router) { }

  ngOnInit(): void {
    this.companiesService.getAllActive(this.companiesComponentLoader).subscribe(x => {
      console.log(x);
      this.companies = x;
    }, error => this.toastsService.showError(error));
  }

  onAddNew(): void {
    this.router.navigate(['management/companies/add']);
  }

  onEdit(order: Company): void {
    this.router.navigate(['management/companies/', order.id]);
  }
}
