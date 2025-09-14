import { inject } from '@angular/core';
import { CanMatchFn, Router, UrlSegment, Route } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Role } from '../models/role';

export const roleGuard: CanMatchFn = (route: Route, segments: UrlSegment[]) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const expected: Role[] = (route.data?.['roles'] ?? []) as Role[];

  const user = auth.currentUser;
  if (!user) {
    router.navigate(['/login']);
    return false;
  }

  if (expected.length && !expected.some((r) => user.roles.includes(r))) {
    router.navigate(['/forbidden']);
    return false;
  }

  return true;
};
