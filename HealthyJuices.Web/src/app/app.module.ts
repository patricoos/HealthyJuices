import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { DeviceDetectorService } from 'ngx-device-detector';
import { GlobalErrorHandler } from './_shared/interceptors/global-error.handler';
import { AuthInterceptor } from './_shared/interceptors/auth.interceptor';
import { HttpErrorInterceptor } from './_shared/interceptors/http-error.interceptor';
import { UniversalDeviceDetectorService } from './_shared/services/universal-device-detector.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    //    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
  ],
  providers: [
    // { provide: DeviceDetectorService, useClass: UniversalDeviceDetectorService },
    //    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    //   { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    //  { provide: ErrorHandler, useClass: GlobalErrorHandler },
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
