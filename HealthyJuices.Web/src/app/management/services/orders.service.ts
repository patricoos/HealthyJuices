import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { finalize } from 'rxjs/operators';
import { LoadersService } from 'src/app/_shared/services/loaders.service';
import { BaseService } from 'src/app/_shared/services/_base.service';
import { Order } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService extends BaseService {

  constructor(private http: HttpClient, private loadersService: LoadersService) {
    super();
  }

  getAllActive(loader: string): Observable<Array<Order>> {
    this.loadersService.show(loader);
    return this.http.get<Array<Order>>(this.baseUrl + '/orders/active').pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }
}
