import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { finalize, map } from 'rxjs/operators';
import { Company } from '../../models/user/company.model';
import { LoadersService } from '../loaders.service';
import { BaseService } from '../_base.service';

@Injectable({
  providedIn: 'root'
})
export class CompaniesService extends BaseService {

  constructor(private http: HttpClient, private loadersService: LoadersService) {
    super();
  }

  getAll(loader: string): Observable<Array<Company>> {
    this.loadersService.show(loader);
    return this.http.get<Array<Company>>(this.baseUrl + '/companies/').pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }

  getAllActive(loader: string): Observable<Array<Company>> {
    this.loadersService.show(loader);
    return this.http.get<Array<Company>>(this.baseUrl + '/companies/active').pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }

  get(id: number, loader: string): Observable<Company> {
    this.loadersService.show(loader);
    return this.http.get<Company>(this.baseUrl + '/companies/' + id)
      .pipe(finalize(() => this.loadersService.hide(loader)));
  }

  addOrEdit(dto: Company, loader: string): Observable<boolean> {
    if (dto.id == null || dto.id === '') {
      return this.add(dto, loader);
    } else {
      return this.edit(dto, loader);
    }
  }

  add(dto: Company, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.post(this.baseUrl + '/companies', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  edit(dto: Company, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.put(this.baseUrl + '/companies', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  delete(id: string, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.delete(this.baseUrl + '/companies/' + id, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

}
