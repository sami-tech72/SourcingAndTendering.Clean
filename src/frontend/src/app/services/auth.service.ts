import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../environments/environment';
import { LoginRequest, LoginResponse } from '../models/auth';
import { User } from '../models/user';
import { StorageService } from '../core/storage.service';

const TOKEN_KEY = 'auth.token';
const USER_KEY = 'auth.user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private storage = inject(StorageService);

  get token(): string | null {
    return this.storage.getItem(TOKEN_KEY);
  }

  // 👇 this is what your guard calls: auth.currentUser
  get currentUser(): User | null {
    const raw = this.storage.getItem(USER_KEY);
    return raw ? (JSON.parse(raw) as User) : null;
  }

  login(req: LoginRequest) {
    return this.http.post<LoginResponse>(
      `${environment.apiBase}/api/auth/login`,
      req
    );
  }

  saveSession(res: LoginResponse) {
    this.storage.setItem(TOKEN_KEY, res.accessToken);
    const u: User = {
      emailOrUserName: this.readNameFromToken(res.accessToken),
      roles: res.roles,
      expUtc: new Date(res.expiresAtUtc),
    };
    this.storage.setItem(USER_KEY, JSON.stringify(u));
  }

  logout(redirect = true) {
    this.storage.removeItem(TOKEN_KEY);
    this.storage.removeItem(USER_KEY);
    if (redirect) this.router.navigateByUrl('/login');
  }

  isAuthenticated(): boolean {
    const u = this.currentUser;
    return !!u && new Date(u.expUtc) > new Date();
  }

  hasRole(...roles: string[]): boolean {
    const u = this.currentUser;
    return !!u && roles.some((r) => u.roles.includes(r));
  }

  private readNameFromToken(token: string): string {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload?.unique_name ?? payload?.name ?? '';
    } catch {
      return '';
    }
  }
}
