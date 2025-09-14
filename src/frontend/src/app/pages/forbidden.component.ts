import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forbidden',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-5 text-center">
      <h2 class="text-danger">Access Denied</h2>
      <p>You don’t have permission to view this page.</p>
    </div>
  `,
})
export class ForbiddenPage {}
