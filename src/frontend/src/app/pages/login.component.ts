import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Role } from '../models/role';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="login-container">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-md-8 col-lg-6">
            <div class="card login-card shadow-sm">
              <div class="text-center mb-4 pt-4">
                <img
                  src="img/udc-logo.png"
                  alt="UDC Logo"
                  height="60"
                  class="mb-2"
                />
                <p class="text-muted mb-0">Sourcing & Tendering Solution</p>
              </div>

              <!-- Angular tabs (no Bootstrap JS) -->
              <ul class="nav nav-tabs mb-3 px-4">
                <li class="nav-item" *ngFor="let r of roles">
                  <button
                    type="button"
                    class="nav-link"
                    [class.active]="role === r"
                    (click)="setRole(r)"
                  >
                    {{ r }}
                  </button>
                </li>
              </ul>

              <!-- Prevent native form submit -->
              <form
                class="px-4 pb-4"
                #f="ngForm"
                (submit)="$event.preventDefault()"
                novalidate
              >
                <div class="mb-3">
                  <label class="form-label">Email</label>
                  <input
                    class="form-control"
                    type="email"
                    name="usernameOrEmail"
                    [(ngModel)]="usernameOrEmail"
                    required
                    [placeholder]="placeholders[role]"
                    (keydown.enter)="onEnter(f, $event)"
                  />
                </div>

                <div class="mb-3">
                  <label class="form-label">Password</label>
                  <div class="input-group">
                    <input
                      class="form-control"
                      [type]="showPassword ? 'text' : 'password'"
                      name="password"
                      [(ngModel)]="password"
                      required
                      placeholder="P@ssw0rd!"
                      (keydown.enter)="onEnter(f, $event)"
                    />
                    <button
                      class="btn btn-outline-secondary"
                      type="button"
                      (click)="showPassword = !showPassword"
                    >
                      <i
                        class="bi"
                        [ngClass]="showPassword ? 'bi-eye-slash' : 'bi-eye'"
                      ></i>
                    </button>
                  </div>
                </div>

                <div class="mb-3 form-check">
                  <input
                    type="checkbox"
                    class="form-check-input"
                    id="remember"
                    [(ngModel)]="remember"
                    name="remember"
                  />
                  <label class="form-check-label" for="remember"
                    >Remember me</label
                  >
                </div>

                <div class="text-danger mb-2" *ngIf="error">{{ error }}</div>

                <div class="d-grid gap-2">
                  <!-- IMPORTANT: never submit natively; cancel click event -->
                  <button
                    class="btn btn-primary"
                    type="button"
                    (click)="submit(f, $event)"
                  >
                    <span *ngIf="!loading">Login</span>
                    <span
                      *ngIf="loading"
                      class="spinner-border spinner-border-sm"
                    ></span>
                  </button>

                  <a
                    class="text-center"
                    href="javascript:void(0)"
                    *ngIf="role !== 'Admin'"
                  >
                    Forgot password?
                  </a>
                </div>
              </form>

              <hr class="mx-4" *ngIf="role === 'Supplier'" />
              <div class="text-center pb-4" *ngIf="role === 'Supplier'">
                <p class="mb-2">New supplier?</p>
                <a
                  routerLink="/supplier/register"
                  class="btn btn-outline-primary"
                  >Register Now</a
                >
              </div>
            </div>

            <div class="text-center mt-3 text-muted small">
              © 2025 UDC. All rights reserved.
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .login-container {
        padding: 2rem 0;
      }
      .login-card {
        border-radius: 12px;
        overflow: hidden;
      }
      .nav-tabs .nav-link {
        border: none;
        border-bottom: 2px solid transparent;
      }
      .nav-tabs .nav-link.active {
        font-weight: 600;
        border-bottom-color: currentColor;
      }
    `,
  ],
})
export class LoginComponent {
  roles: Role[] = ['Procurement', 'Supplier', 'Admin'];
  role: Role = 'Procurement';

  usernameOrEmail = '';
  password = '';
  remember = false;
  showPassword = false;

  loading = false;
  error = '';

  placeholders: Record<Role, string> = {
    Procurement: 'procurement@udc.qa',
    Supplier: 'supplier@udc.qa',
    Admin: 'admin@udc.qa',
  };

  constructor(private auth: AuthService, private router: Router) {}

  setRole(r: Role) {
    this.role = r;
    // (optional) prefill so the form is valid for quick testing
    this.usernameOrEmail = this.placeholders[r];
    this.password = 'P@ssw0rd!';
  }

  onEnter(f: NgForm, ev: Event) {
    ev.preventDefault();
    ev.stopPropagation();
    this.submit(f, ev);
  }

  submit(f: NgForm, ev?: Event) {
    // HARD stop any native submit / bubbling
    if (ev) {
      ev.preventDefault();
      ev.stopPropagation();
    }

    // simple validation (don’t rely on disabled buttons)
    if (!this.usernameOrEmail || !this.password) {
      this.error = 'Email and password are required.';
      return;
    }

    this.error = '';
    this.loading = true;

    const payload = {
      usernameOrEmail: this.usernameOrEmail,
      password: this.password,
    };

    console.log('Login payload', payload, 'role:', this.role);

    this.auth.login(payload).subscribe({
      next: (res) => {
        console.log('Login success', res);
        this.auth.saveSession(res);
        this.router.navigate([`/${this.role.toLowerCase()}`]);
      },
      error: (err) => {
        console.error('Login error', err);
        this.error =
          err?.error?.message ||
          (typeof err?.error === 'string' ? err.error : '') ||
          (err?.status
            ? `HTTP ${err.status} ${err.statusText}`
            : 'Login failed');
        this.loading = false;
      },
    });
  }
}
