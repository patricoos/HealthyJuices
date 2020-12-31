import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadersService {

  showSignals: Subject<string>;
  hideSignals: Subject<string>;

  constructor() {
    this.showSignals = new Subject<string>();
    this.hideSignals = new Subject<string>();
  }

  show(loaderName: string): void {
    setTimeout(() => this.showSignals.next(loaderName), 0);
  }

  hide(loaderName: string): void {
    setTimeout(() => this.hideSignals.next(loaderName), 250);
  }


  getPendingToShow(): Observable<any> {
    return this.showSignals.asObservable();
  }

  getPendingToHide(): Observable<any> {
    return this.hideSignals.asObservable();
  }
}
