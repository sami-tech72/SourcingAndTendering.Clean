import { signal } from '@angular/core';
export type Role = 'procurement' | 'supplier' | 'admin';
export const currentRole = signal<Role>('procurement');
export const setRole = (r: Role) => currentRole.set(r);
