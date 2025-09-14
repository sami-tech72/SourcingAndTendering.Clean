import { Role } from './role';

// src/app/models/auth.ts
export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresAtUtc: string; // ISO datetime from API
  roles: string[]; // e.g. ["Admin"]
}
