import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { finalize, map } from 'rxjs/operators';
import { LoadersService } from 'src/app/_shared/services/loaders.service';
import { BaseService } from 'src/app/_shared/services/_base.service';
import { Order } from '../../../management/models/order.model';

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

  Get(id: number, loader: string): Observable<Order> {
    this.loadersService.show(loader);
    return this.http.get<Order>(this.baseUrl + 'orders/' + id)
      .pipe(finalize(() => this.loadersService.hide(loader)));
  }

  addOrEdit(dto: Order, loader: string): Observable<boolean> {
    if (dto.id == null || dto.id === 0) {
      return this.add(dto, loader);
    } else {
      return this.edit(dto, loader);
    }
  }

  add(dto: Order, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.post(this.baseUrl + 'orders', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  edit(dto: Order, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.put(this.baseUrl + 'orders', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  delete(id: number, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.delete(this.baseUrl + 'orders/' + id, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

}
