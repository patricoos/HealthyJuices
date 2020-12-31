import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, interval } from 'rxjs';
import { Router } from '@angular/router';
import { DeviceDetectorService } from 'ngx-device-detector';
import { BaseService } from './_base.service';
import { WebExceptionLog } from '../models/web-exception-log.model';
import { UniversalDeviceDetectorService } from './universal-device-detector.service';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService extends BaseService {

  private logs: Array<WebExceptionLog> = [];
  private browserRetentionToken = '';

  constructor(private http: HttpClient, private router: Router, private deviceService: UniversalDeviceDetectorService) {
    super();
    this.browserRetentionToken = this.generateGuid();
  }

  saveWebExceptionLog(message: string = '', stackTrace: string = ''): Observable<any> {
    const log = this.buildModel(message, stackTrace);

    if (this.logs.map(x => x.message).includes(log.message) && this.logs.map(x => x.stackTrace).includes(log.stackTrace)) {
      return of(true);
    }
    this.logs.push(log);

    return this.http.post(this.baseUrl + 'maintenance/web-exception', log, { observe: 'response' });
  }

  private buildModel(message: string = '', stackTrace: string = ''): WebExceptionLog {
    const extraData = `browserRetentionToken: ${this.browserRetentionToken},\n
    deviceInfo: ${JSON.stringify(this.deviceService.getDeviceInfo(), null, '\t')},\n`;
    return {
      message,
      url: this.router.url,
      stackTrace: extraData + stackTrace
    };
  }

  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
      // tslint:disable-next-line:one-variable-per-declaration
      const r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}
