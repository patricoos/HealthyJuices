import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { finalize, map } from 'rxjs/operators';
import { LoadersService } from 'src/app/_shared/services/loaders.service';
import { BaseService } from 'src/app/_shared/services/_base.service';
import { Order } from '../../../management/models/order.model';
import { DashboardOrderReport } from '../../models/orders/dashboard-order-report.model';
import { OrderReport } from '../../models/orders/order-report.model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService extends BaseService {

  constructor(private http: HttpClient, private loadersService: LoadersService) {
    super();
  }

  getAllActive(loader: string, from?: Date, to?: Date): Observable<Array<Order>> {
    this.loadersService.show(loader);

    let params = new HttpParams();
    if (from) {
      params = params.set('from', from.toISOString());
    }
    if (to) {
      params = params.set('to', to.toISOString());
    }

    return this.http.get<Array<Order>>(this.baseUrl + '/orders/active', { params }).pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }

  getAllMyActive(loader: string, from?: Date, to?: Date): Observable<Array<Order>> {
    this.loadersService.show(loader);

    let params = new HttpParams();
    if (from) {
      params = params.set('from', from.toISOString());
    }
    if (to) {
      params = params.set('to', to.toISOString());
    }

    return this.http.get<Array<Order>>(this.baseUrl + '/orders/my', { params }).pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }

  getAllActiveByCompanyAsync(loader: string, from: Date, to: Date): Observable<DashboardOrderReport> {
    this.loadersService.show(loader);
    let params = new HttpParams();
    params = params.set('from', from.toISOString());
    params = params.set('to', to.toISOString());

    return this.http.get<DashboardOrderReport>(this.baseUrl + '/orders/dashboard-report', { params }).pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }


  get(id: number, loader: string): Observable<Order> {
    this.loadersService.show(loader);
    return this.http.get<Order>(this.baseUrl + '/orders/' + id)
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
    return this.http.post(this.baseUrl + '/orders', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  edit(dto: Order, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.put(this.baseUrl + '/orders', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  delete(id: number, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.delete(this.baseUrl + '/orders/' + id, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }
}
