import { Component, OnInit } from '@angular/core';
import { MapComponent } from 'src/app/_shared/components/_abstraction/map.component';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { Company } from 'src/app/_shared/models/user/company.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent extends MapComponent implements OnInit {
  dashboardComponentLoader = 'dashboardComponentLoader';
  chartData: any;
  chartOptions: any = { legend: { position: 'right' } };

  filterDate = new Date();

  companies: Company[] = [];
  calendarLocale = FullCallendarConsts.getCallendarLocale();

  columns = [
    { field: 'name', header: 'Name' },
    { field: 'postalCode', header: 'Postal Code' },
    { field: 'city', header: 'City' },
    { field: 'street', header: 'Street' },
    { field: 'dateCreated', header: 'Created' },
  ];

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.chartData = {
      labels: ['aaaaaaaA', 'bbbbbbbB', 'ccccccccccccccccccccccC'],
      datasets: [
        {
          data: [300, 50, 100],
          backgroundColor: [
            '#FF6384',
            '#36A2EB',
            '#FFCE56'
          ]
        }]
    };
  }

}

