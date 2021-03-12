import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadersService {
  private loaders: string[] = [];
  private loadersObs: Subject<string[]> = new Subject<string[]>();

  show(loaderName: string): void {
    if (!loaderName || loaderName === '') {
      return;
    }
    setTimeout(() => {
      this.loaders.push(loaderName);
      this.loadersObs.next(this.loaders);
    }, 0);
  }

  hide(loaderName: string): void {
    setTimeout(() => {
      const index = this.loaders.indexOf(loaderName, 0);
      if (index > -1) {
        this.loaders.splice(index, 1);
      }
      this.loadersObs.next(this.loaders);
    }, 250);
  }

  getPendingToShow(): Observable<string[]> {
    return this.loadersObs.asObservable();
  }
}
