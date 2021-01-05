import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize, map } from 'rxjs/operators';
import { Unavailability } from 'src/app/management/models/unavailability.model';
import { LoadersService } from '../loaders.service';
import { BaseService } from '../_base.service';

@Injectable({
  providedIn: 'root'
})
export class UnavailabilitiesService extends BaseService {

  constructor(private http: HttpClient, private loadersService: LoadersService) {
    super();
  }

  getAll(loader: string, from?: Date, to?: Date): Observable<Array<Unavailability>> {
    this.loadersService.show(loader);

    let params = new HttpParams();
    if (from) {
      params = params.set('from', from.toISOString());
    }
    if (to) {
      params = params.set('to', to.toISOString());
    }

    return this.http.get<Array<Unavailability>>(this.baseUrl + '/unavailabilities', { params }).pipe(
      finalize(() => this.loadersService.hide(loader))
    );
  }

  addOrEdit(dto: Unavailability, loader: string): Observable<boolean> {
    if (dto.id == null || dto.id === 0) {
      return this.add(dto, loader);
    } else {
      return this.edit(dto, loader);
    }
  }

  get(id: number, loader: string): Observable<Unavailability> {
    this.loadersService.show(loader);
    return this.http.get<Unavailability>(this.baseUrl + '/unavailabilities/' + id)
      .pipe(finalize(() => this.loadersService.hide(loader)));
  }

  add(dto: Unavailability, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.post(this.baseUrl + '/unavailabilities', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  edit(dto: Unavailability, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.put(this.baseUrl + '/unavailabilities', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  delete(id: number, loader: string): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.delete(this.baseUrl + '/unavailabilities/' + id, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }
}
