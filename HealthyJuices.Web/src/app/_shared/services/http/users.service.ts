import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { LoadersService } from '../loaders.service';
import { BaseService } from '../_base.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends BaseService {

  constructor(private http: HttpClient, private loadersService: LoadersService) {
    super();
  }

  IsExisting(username: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}/users/existing/${username}`);
  }
}
