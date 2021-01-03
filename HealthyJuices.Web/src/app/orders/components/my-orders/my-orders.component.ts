import { Company } from './../../../_shared/models/user/company.model';
import { Component, OnInit } from '@angular/core';
import { MapComponent } from 'src/app/_shared/components/_abstraction/map.component';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss']
})
export class MyOrdersComponent extends MapComponent implements OnInit {
  myOrdersComponentLoader = 'myOrdersComponentLoader';

  companies: Company[] = [];

  constructor() {
    super();
  }

  ngOnInit(): void {
  }

}
