import { Injectable } from '@angular/core';
import { SwUpdate } from '@angular/service-worker';
import { MessageService } from 'primeng/api';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PwaService {

  promptEvent: any;
  isUpdateAvailable = false;

  private internalConnectionChanged = new Subject<boolean>();

  get connectionChanged(): Observable<boolean> {
    return this.internalConnectionChanged.asObservable();
  }

  get isOnline(): boolean {
    return !!window.navigator.onLine;
  }

  get isInInstalledApp(): () => any {
    return () => (window.matchMedia('(display-mode: standalone)').matches) ||
      ((window.navigator as any).standalone) ||
      document.referrer.includes('android-app://');
  }

  constructor(updates: SwUpdate, public messageService: MessageService) {
    this.initEventListeners();
    updates.available.subscribe(e => {
      this.isUpdateAvailable = true;
      if (confirm('New wersion available. Load new Version?')) {
        updates.activateUpdate().then(() => document.location.reload());
        this.isUpdateAvailable = false;
      }
    });
    setTimeout(() => {
      if (this.isInInstalledApp()) {
        this.messageService.add({ severity: 'info', summary: 'Info', detail: 'App is in PWA Mode!' });
        this.updateOnlineStatus(false);
      }
    }, 1000);
  }


  initEventListeners(): void {
    window.addEventListener('online', () => this.updateOnlineStatus());
    window.addEventListener('offline', () => this.updateOnlineStatus());
    window.addEventListener('beforeinstallprompt', (e) => {
      // Prevent Chrome 76 and later from showing the mini-infobar
      e.preventDefault();
      // Stash the event so it can be triggered later.
      this.promptEvent = e;
      // if (confirm('Do you want to install ShopController app?')) {
      //   setTimeout(() => this.installPwa(), 1000);
      // }
    });
  }

  private updateOnlineStatus(showOnline = true): void {
    this.internalConnectionChanged.next(window.navigator.onLine);
    if (window.navigator.onLine) {
      this.messageService.add({ severity: 'info', summary: 'Info', detail: 'App is in Online Mode' });
    } else {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'App is in Offline Mode' });
    }
  }

  installPwa() {
    window.addEventListener('appinstalled', (evt) => {
      console.log('a2hs installed');
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'App Installed' });
    });
    this.promptEvent.prompt();
    this.promptEvent = null;
  }
}

// https://offering.solutions/blog/articles/2018/11/21/online-and-offline-sync-with-angular-and-indexeddb/
