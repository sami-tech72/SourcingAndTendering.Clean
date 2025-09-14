import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { currentRole } from '../state/auth.store';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styles: [
    `
      .sidebar {
        width: 260px;
      }
      .nav-link {
        border-radius: 0.5rem;
      }
    `,
  ],
})
export class SidebarComponent {
  role = currentRole.asReadonly();
}
