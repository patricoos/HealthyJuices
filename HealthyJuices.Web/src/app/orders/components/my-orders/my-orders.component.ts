import { Unavailability } from './../../../management/models/unavailability.model';
import { TableQueryService } from './../../../_shared/services/table.service';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { OrdersService } from 'src/app/_shared/services/http/orders.service';
import { Company } from './../../../_shared/models/user/company.model';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MapComponent } from 'src/app/_shared/components/_abstraction/map.component';
import { TimeConverter } from 'src/app/_shared/utils/time.converter';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { Order } from 'src/app/management/models/order.model';
import { ProductsService } from 'src/app/_shared/services/http/products.service';
import { Product } from 'src/app/_shared/models/products/product.model';
import { Message, SelectItem } from 'primeng/api';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';
import { UnavailabilitiesService } from 'src/app/_shared/services/http/unavailabilities.service';
import { UnavailabilityReason } from 'src/app/_shared/models/enums/unavailability-reason.enum';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss']
})
export class MyOrdersComponent extends MapComponent implements AfterViewInit {
  myOrdersTableComponentLoader = 'myOrdersTableComponentLoader';
  myOrdersFormComponentLoader = 'myOrdersFormComponentLoader';

  calendarLocale = FullCallendarConsts.getCallendarLocale();
  orders: Order[] = [];
  products: SelectItem[] = [];
  unavailabilities: Message[] = [];

  orderForm: FormGroup = this.initForm();
  productForm: FormGroup = this.initProductForm();

  columns = [
    { field: 'deliveryDate', header: 'Delivery Date' },
    { field: 'created', header: 'Created' },
    { field: 'productsNames', header: 'Products' },
  ];

  constructor(private orderService: OrdersService, private toastsService: ToastsService, private productsService: ProductsService,
    private tableService: TableQueryService, private unavailabilitiesService: UnavailabilitiesService, private datePipe: DatePipe) {
    super();
  }
  ngAfterViewInit(): void {
    this.getOrders();
    this.productsService.getAllActive(this.myOrdersFormComponentLoader).subscribe(x => {
      this.products = x.map(p => ({ label: p.name, value: p.id }));
      console.log(x);
    }, error => this.toastsService.showError(error));
    this.unavailabilitiesService.getAll(this.myOrdersFormComponentLoader,
      TimeConverter.getMinTime(new Date()), TimeConverter.getMaxTime(TimeConverter.tomorrow)).subscribe(x => {
        this.unavailabilities = x.map(p =>
        ({
          detail: `${this.datePipe.transform(p.from, 'dd.MM.yyyy')} - ${this.datePipe.transform(p.to, 'dd.MM.yyyy')}`,
          severity: 'warn', summary: 'Unavailability'
        }));
        x.forEach(u => {
          const fDate = new Date(u.from);
          const lDate = new Date(u.to);
          const cDate = this.orderForm.controls.deliveryDate.value;
          if ((cDate <= lDate && cDate >= fDate)) {
            this.productForm.disable();
          }
        });
        console.log(x);
      }, error => this.toastsService.showError(error));
  }


  getOrders(): void {
    this.orderService.getAllMyActive(this.myOrdersTableComponentLoader,
      TimeConverter.getMinTime(new Date()), TimeConverter.getMaxTime(TimeConverter.tomorrow)).subscribe(x => {
        console.log(x);
        this.orders = x;
        this.orders.forEach(o => o.productsNames = o.orderProducts.map(z => z.product.name).join(', '));
      }, error => this.toastsService.showError(error));
  }

  onAdd(): void {
    if (this.productForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.orderForm);
      this.toastsService.showError('Invalid Form');
      return;
    }
    this.productForm.controls.productName.setValue(this.products.find(x => x.value === this.productForm.controls.productId.value)?.label);
    (this.orderForm.controls.orderProducts as FormArray).push(this.productForm);
    this.productForm = this.initProductForm();
    FormGroupExtension.markFormAssDirtyAndTouched(this.orderForm);
  }

  onSave(): void {
    if (this.orderForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.orderForm);
      this.toastsService.showError('Invalid Form');
      return;
    }

    this.orderService.addOrEdit(this.orderForm.value, this.myOrdersFormComponentLoader).subscribe(x => {
      this.toastsService.showSuccess('Order added!');
      this.getOrders();
      this.orderForm = this.initForm();
      this.productForm = this.initProductForm();
    }, error => this.toastsService.showError(error));
  }

  private initForm(order: Order | null = null): FormGroup {
    const form = new FormGroup({
      deliveryDate: new FormControl(order ? order.deliveryDate : TimeConverter.tomorrow, Validators.required),
      orderProducts: new FormArray([], Validators.required),
    });
    return form;
  }

  private initProductForm(): FormGroup {
    const form = new FormGroup({
      productId: new FormControl(null, Validators.required),
      productName: new FormControl(null),
      amount: new FormControl(null, [Validators.required, Validators.min(0.01), Validators.max(100)]),
    });
    return form;
  }
}
