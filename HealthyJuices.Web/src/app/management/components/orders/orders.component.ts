import { ToastsService } from './../../../_shared/services/toasts.service';
import { Component, OnInit } from '@angular/core';
import { Order } from '../../models/order.model';
import { OrdersService } from '../../../_shared/services/http/orders.service';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { TableQueryService } from 'src/app/_shared/services/table.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  ordersComponentLoader = 'ordersComponentLoader';

  orders: Order[] = [];
  columns = [
    { field: 'userName', header: 'User' },
    { field: 'destinationCompanyName', header: 'Destination Company' },
    { field: 'dateCreated', header: 'Created' },
    { field: 'deliveryDate', header: 'Delivery Date' },
    { field: 'action', header: 'Action', width: '145px' },
  ];
  calendarLocale = FullCallendarConsts.getCallendarLocale();

  constructor(private ordersService: OrdersService, private toastsService: ToastsService, private tableQueryService: TableQueryService) { }

  ngOnInit(): void {
    this.ordersService.getAllActive(this.ordersComponentLoader).subscribe(x => {
      console.log(x);
      this.orders = x;
      this.orders.forEach(o => {
        o.userName = o.user.firstName;
        o.destinationCompanyName = o.destinationCompany.name;
      });
    }, error => this.toastsService.showError(error));
  }

  onAddNew(): void { }

  onEdit(order: Order): void { }
}
