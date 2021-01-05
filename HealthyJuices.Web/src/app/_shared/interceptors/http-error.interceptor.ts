import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(0),
        catchError((error: HttpErrorResponse) => {
          console.error(error);
          let message = '';
          switch (error.status) {
            case 406:
            case 400:
            case 409:
            case 500:
              message = error.error;
              break;
            case 404:
              message = '404 Server not found';
              break;
            case 401:
              message = 'Unauthorized operation';
              this.authService.logout();
              break;
            case 0:
              message = 'Server can\'t be reached';
              break;
            default:
              message = 'Unknown error';
              break;
          }
          return throwError(message);
        })
      );
  }
}
