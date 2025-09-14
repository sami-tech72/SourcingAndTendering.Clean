import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { currentRole, setRole, Role } from '../state/auth.store';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  role = currentRole.asReadonly();

  // local UI state for dropdowns
  isNotifOpen = false;
  isUserOpen = false;

  constructor(private router: Router) {}

  change(r: Role) {
    setRole(r);
    this.router.navigate(['/' + r]);
  }

  toggleNotifDropdown(e: Event) {
    e.preventDefault();
    e.stopPropagation();
    this.isNotifOpen = !this.isNotifOpen;
    this.isUserOpen = false;
  }

  toggleUserDropdown(e: Event) {
    e.preventDefault();
    e.stopPropagation();
    this.isUserOpen = !this.isUserOpen;
    this.isNotifOpen = false;
  }

  @HostListener('document:click')
  closeAll() {
    this.isNotifOpen = false;
    this.isUserOpen = false;
  }

  @HostListener('document:keydown.escape')
  closeOnEsc() {
    this.closeAll();
  }

  logout() {
    // If your Role type doesn’t allow null, add 'none' to it:
    // export type Role = 'admin' | 'supplier' | 'procurement' | 'none';
    setRole('none' as Role);

    this.closeAll();
    this.router.navigate(['/login']);
  }
}
