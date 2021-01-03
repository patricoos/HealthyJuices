import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/management/models/order.model';
import { OrdersService } from 'src/app/_shared/services/http/orders.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';

@Component({
  selector: 'app-orders-form',
  templateUrl: './orders-form.component.html',
  styleUrls: ['./orders-form.component.scss']
})
export class OrdersFormComponent implements AfterViewInit {
  ordersFormComponentLoader = 'ordersFormComponentLoader';

  selectedOrder: Order | undefined;

  id: number | undefined;
  private sub: any;
  editForm: FormGroup = this.initForm();

  constructor(private route: ActivatedRoute, private ordersService: OrdersService, private toastsService: ToastsService) { }

  ngAfterViewInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id'];
      if (this.id) {
        this.getDetails(this.id);
      }
    });
  }

  getDetails(id: number): void {
    this.ordersService.Get(id, this.ordersFormComponentLoader).subscribe(x => {
      this.selectedOrder = x;
      this.editForm = this.initForm(x);
    }, error => this.toastsService.showError(error));
  }

  private initForm(company: Order | null = null): FormGroup {
    const form = new FormGroup({
      id: new FormControl(company ? company.id : null),
    });
    return form;
  }

}
