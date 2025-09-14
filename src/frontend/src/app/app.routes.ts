import { Routes } from '@angular/router';
import { LayoutComponent } from './core/layout.component';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login.component').then((m) => m.LoginComponent),
  },
  {
    path: '',
    component: LayoutComponent,
    canMatch: [roleGuard], // protect whole layout
    children: [
      {
        path: 'procurement/dashboard',
        canMatch: [roleGuard],
        data: { roles: ['Procurement', 'Admin'] },
        loadComponent: () =>
          import('./features/procurement/procurement-dashboard.component').then(
            (m) => m.ProcurementDashboardComponent
          ),
      },
      {
        path: 'procurement/rfx-management',
        canMatch: [roleGuard],
        data: { roles: ['Procurement', 'Admin'] },
        loadComponent: () =>
          import(
            './features/procurement/rfx-management/rfx-management.component'
          ).then((m) => m.RfxManagementComponent),
      },
      {
        path: 'procurement/rfx-management/create',
        canMatch: [roleGuard],
        data: { roles: ['Procurement'] },
        loadComponent: () =>
          import(
            './features/procurement/rfx-management/create-new-rfx/create-new-rfx.component'
          ).then((m) => m.CreateNewRfxComponent),
      },
      {
        path: 'admin',
        canMatch: [roleGuard],
        data: { roles: ['Admin'] },
        loadComponent: () =>
          import('./features/admin/admin-dashboard.component').then(
            (m) => m.AdminDashboardComponent
          ),
      },
      {
        path: 'supplier',
        canMatch: [roleGuard],
        data: { roles: ['Supplier'] },
        loadComponent: () =>
          import('./features/supplier/supplier-dashboard.component').then(
            (m) => m.SupplierDashboardComponent
          ),
      },
      { path: '', redirectTo: 'procurement/dashboard', pathMatch: 'full' },
    ],
  },
  { path: '**', redirectTo: '' },
];
