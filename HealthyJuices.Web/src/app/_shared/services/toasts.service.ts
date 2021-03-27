import { Injectable } from '@angular/core';
import { Message, MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class ToastsService {
  constructor(public messageService: MessageService) { }

  showSuccess(details: string, title: string = 'Info'): void {
    const message = { severity: 'success', summary: title, detail: details };
    this.messageService.add(message);
  }

  showInfo(details: string, title: string = 'Success'): void {
    const message = { severity: 'info', summary: title, detail: details };
    this.messageService.add(message);
  }

  showWarn(details: string, title: string = 'Warn'): void {
    const message = { severity: 'warn', summary: title, detail: details };
    this.messageService.add(message);
  }

  showError(details: string, title: string = 'Error'): void {
    const message = { severity: 'error', summary: title, detail: details };
    this.messageService.add(message);
  }
}
