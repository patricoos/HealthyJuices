import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MapComponent } from 'src/app/_shared/components/_abstraction/map.component';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { OrderProduct } from 'src/app/_shared/models/orders/order-product.model';
import { OrderReport } from 'src/app/_shared/models/orders/order-report.model';
import { Company } from 'src/app/_shared/models/user/company.model';
import { OrdersService } from 'src/app/_shared/services/http/orders.service';
import { TableQueryService } from 'src/app/_shared/services/table.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { TimeConverter } from 'src/app/_shared/utils/time.converter';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent extends MapComponent implements AfterViewInit {
  dashboardComponentLoader = 'dashboardComponentLoader';
  chartData: any;
  chartOptions: any = { legend: { position: 'right' } };

  filterDate = new Date();

  orderReports: OrderReport[] = [];
  products: OrderProduct[] = [];
  calendarLocale = FullCallendarConsts.getCallendarLocale();

  columns = [
    { field: 'name', header: 'Name' },
    { field: 'postalCode', header: 'Postal Code' },
    { field: 'city', header: 'City' },
    { field: 'street', header: 'Street' },
    { field: 'dateCreated', header: 'Created' },
  ];

  constructor(private ordersService: OrdersService, private toastsService: ToastsService,
    private tableQueryService: TableQueryService) {
    super();
  }
  ngAfterViewInit(): void {
    this.ordersService.GetAllActiveByCompanyAsync(this.dashboardComponentLoader,
      TimeConverter.getMinTime(this.filterDate), TimeConverter.getMaxTime(this.filterDate)).subscribe(x => {
        this.orderReports = x.orderReports;
        this.products = x.products;
        this.generateChartData(this.products);
        this.setMapCenter(x.orderReports.map(z => z.company));
      }, error => this.toastsService.showError(error));
  }

  generateChartData(products: OrderProduct[]): void {
    this.chartData = {
      labels: products.map(p => `${p.product.name} - ${p.amount}`),
      datasets: [
        {
          data: products.map(p => p.amount),
          backgroundColor: products.map(p => '#' + Math.floor(Math.random() * 16777215).toString(16))
        }]
    };
  }

}
