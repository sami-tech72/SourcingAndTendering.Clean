import { Component } from '@angular/core';
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
  constructor(private router: Router) {}
  change(r: Role) {
    setRole(r);
    this.router.navigate(['/' + r]);
  }
}
