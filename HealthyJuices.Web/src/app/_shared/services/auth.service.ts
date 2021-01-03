import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService } from './_base.service';
import { LocalStorageService } from './local-storage.service';
import { EnumExtension } from '../utils/enum.extension';
import { User } from '../models/user/user.model';
import { finalize, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { LoginUser } from '../models/auth/login-user.model';
import { LoginResponse } from '../models/auth/login-response.model';
import { UserRole } from '../models/enums/user-role.enum';
import { Observable } from 'rxjs';
import { LoadersService } from './loaders.service';
import { RegisterUser } from 'src/app/auth/models/register-user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {

  private userInfo?: LoginResponse | null;
  private get token(): string | null {
    return this.userInfo ? this.userInfo.accessToken : null;
  }

  constructor(private http: HttpClient, private router: Router, private localStorageService: LocalStorageService,
    private loadersService: LoadersService) {
    super();
    this.userInfo = localStorageService.load(LocalStorageService.AUTH_USER_INFO);
  }

  login(loader: string, email: string, password: string, rememberMe: boolean): Observable<User | null> {
    this.loadersService.show(loader);
    return this.http.post<LoginResponse>(this.baseUrl + '/auth/login', new LoginUser(email, password)).pipe(
      map(response => this.saveUser(response, rememberMe)),
      finalize(() => this.loadersService.hide(loader))
    );
  }

  register(loader: string, dto: RegisterUser): Observable<boolean> {
    this.loadersService.show(loader);
    return this.http.post(this.baseUrl + '/auth/register', dto, { observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader))
    );
  }

  confirmRegister(loader: string, email: string, token: string): Observable<boolean> {
    this.loadersService.show(loader);
    let params = new HttpParams();
    params = params.set('email', email);
    params = params.set('token', token);

    return this.http.get(this.baseUrl + '/auth/confirm-register', { params, observe: 'response' }).pipe(
      map(response => this.isStatusSucceed(response.status)),
      finalize(() => this.loadersService.hide(loader)));
  }

  getAuthorizationToken(): string {
    if (!this.token) { return ''; }
    return this.token.toString();
  }

  setToken(token: string): void {
    if (this.userInfo) {
      this.userInfo.accessToken = token;
      this.localStorageService.store(LocalStorageService.AUTH_USER_INFO, this.userInfo);
    }
  }

  getUserName(): string {
    return this.userInfo ? this.userInfo.user.firstName : '';
  }


  getUserRoles(): Array<UserRole> {
    const userRoles = Array<UserRole>();
    if (this.userInfo && this.userInfo.user.roles) {
      let code = this.userInfo.user.roles.valueOf();
      EnumExtension.getLabelAndValues(UserRole)
        .sort((a, b) => 0 - (a.value > b.value ? 1 : -1))
        .forEach(x => {
          if (code - x.value >= 0 && (x.value > 0 || !userRoles.length)) {
            code = code - x.value;
            userRoles.push(x.value);
          }
        });
    }
    return userRoles;
  }

  isUserInRole(role: UserRole): boolean {
    return this.getUserRoles().includes(role);
  }

  isUserLogin(): boolean {
    if (this.userInfo) {
      return true;
    }
    return false;
  }

  logout(): void {
    this.userInfo = null;
    this.localStorageService.remove(LocalStorageService.AUTH_USER_INFO);
    this.router.navigate(['/auth/login']);
  }

  private saveUser(response: LoginResponse, rememberMe: boolean): User | null {
    if (response && response.accessToken && response.user) {
      this.userInfo = response;
      this.localStorageService.store(LocalStorageService.AUTH_USER_INFO, response);
      this.navigateByUserRole();
      return response.user;
    }
    return null;
  }

  navigateByUserRole(): void {
    if (this.getUserRoles().includes(UserRole.BusinessOwner)) {
      this.router.navigate(['/management/orders']);
      return;
    }
    if (this.getUserRoles().includes(UserRole.Customer)) {
      this.router.navigate(['/orders']);
    }
  }
}
