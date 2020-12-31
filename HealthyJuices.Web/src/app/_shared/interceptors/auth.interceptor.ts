import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add token to request
    const token = this.authService.getAuthorizationToken();
    if (token) {
      request = request.clone({
        setHeaders: {
          access_token: token
        }
      });
    }

    if (!request.headers.has('Content-Type')) {
      request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
    }
    if (!request.headers.has('Accept')) {
      request = request.clone({ headers: request.headers.set('Accept', 'application/json') });
    }

    // get token from response
    const responseToken = request.headers.get('X-Access-Token');
    if (responseToken && responseToken != null && responseToken !== '') {
      this.authService.setToken(responseToken);
    }

    return next.handle(request);
  }
}
