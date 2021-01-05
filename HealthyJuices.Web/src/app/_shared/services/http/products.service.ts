import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { finalize, map } from 'rxjs/operators';
import { Product } from '../../models/products/product.model';
import { LoadersService } from '../loaders.service';
import { BaseService } from '../_base.service';

@Injectable({
  providedIn: 'root'
})
export class ProductsService extends BaseService {

  constructor(private http: HttpClient, private loadersService: LoadersService) {
    super();
  }

  getAllActive(loader: string): Observable<Array<Product>> {
    this.loadersService.show(loader);
    return this.http.get<Array<Product>>(this.baseUrl + '/products/active').pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }

  addOrEdit(dto: Product, loader: string): Observable<boolean> {
    if (dto.id == null || dto.id === 0) {
      return this.add(dto, loader);
    } else {
      return this.edit(dto, loader);
    }
  }

  add(dto: Product, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.post(this.baseUrl + '/products', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  edit(dto: Product, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.put(this.baseUrl + '/products', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  delete(id: number, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.delete(this.baseUrl + '/products/' + id, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

}
