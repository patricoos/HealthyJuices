import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  static readonly AUTH_USER_INFO = 'auth.userInfo';

  store(key: string, value: any): void {
    localStorage.setItem(key, JSON.stringify(value));
  }

  load(key: string): any {
    const valueString = localStorage.getItem(key);
    if (valueString) {
      return JSON.parse(valueString);
    }
    return null;
  }

  remove(key: string): void {
    localStorage.removeItem(key);
  }
}
