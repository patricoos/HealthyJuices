import { ErrorHandler, Injectable } from '@angular/core';
import { MaintenanceService } from '../services/maintenance.service';


@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private maintenanceService: MaintenanceService) { }

  handleError(error: any): void {
    if (this.maintenanceService) {
      const message = (!error.message && !error.stack) ? error.toString() : error.message;
      this.maintenanceService.saveWebExceptionLog(message, error.stack).subscribe(x => { });
    }
    throw error;
  }

}
