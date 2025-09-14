import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class StorageService {
  private platformId = inject(PLATFORM_ID);
  private memory = new Map<string, string>(); // fallback for non-browser

  private get isBrowser() {
    return isPlatformBrowser(this.platformId);
  }

  getItem(key: string): string | null {
    if (this.isBrowser) return localStorage.getItem(key);
    return this.memory.has(key) ? this.memory.get(key)! : null;
    // or just: return null;  (if you don't want SSR fallback)
  }

  setItem(key: string, value: string): void {
    if (this.isBrowser) localStorage.setItem(key, value);
    else this.memory.set(key, value);
  }

  removeItem(key: string): void {
    if (this.isBrowser) localStorage.removeItem(key);
    else this.memory.delete(key);
  }
}
