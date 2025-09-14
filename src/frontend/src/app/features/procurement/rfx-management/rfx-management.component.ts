// src/app/features/procurement/rfx-management/rfx-management.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

type RfxType = 'RFP' | 'RFQ' | 'RFI' | 'EOI';
type RfxStatus =
  | 'Draft'
  | 'Pending Approval'
  | 'Published'
  | 'Under Evaluation'
  | 'Awarded'
  | 'Completed'
  | 'Cancelled';

export interface RfxListItem {
  id: string;
  title: string;
  type: RfxType;
  category: string;
  publicationDate: string; // ISO string from API
  closingDate: string; // ISO string from API
  status: RfxStatus;
  responses: number;
}

@Component({
  selector: 'app-rfx-management',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './rfx-management.component.html',
  styleUrls: ['./rfx-management.component.css'],
})
export class RfxManagementComponent implements OnInit {
  private http = inject(HttpClient);

  // raw list from API
  allRfxes: RfxListItem[] = [];

  // filtered list bound to the table (what the template needs)
  filteredRfxes: RfxListItem[] = [];

  // simple filter state
  filterType: 'all' | RfxType = 'all';
  filterStatus: 'all' | RfxStatus = 'all';
  filterCategory: 'all' | string = 'all';
  searchText = '';

  // UX state
  loading = false;
  error = '';

  ngOnInit(): void {
    this.loadRfxes();
  }

  // ----- API -----
  private loadRfxes() {
    this.loading = true;
    this.error = '';

    this.http.get<RfxListItem[]>(`${environment.apiBase}/api/rfx`).subscribe({
      next: (list) => {
        // If your API returns different property names, map here.
        this.allRfxes = (list ?? []).map((r) => ({
          ...r,
          // ensure types are consistent if backend sends lowercase, etc.
          type: (r.type as RfxType) ?? 'RFP',
          status: (r.status as RfxStatus) ?? 'Published',
        }));
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error =
          err?.error?.message ||
          (typeof err?.error === 'string' ? err.error : '') ||
          'Failed to load RFx list';
        this.loading = false;
      },
    });
  }

  // ----- Filters (called from template) -----
  onTypeChange(value: string) {
    this.filterType = (value as any) || 'all';
    this.applyFilters();
  }

  onStatusChange(value: string) {
    this.filterStatus = (value as any) || 'all';
    this.applyFilters();
  }

  onCategoryChange(value: string) {
    this.filterCategory = value || 'all';
    this.applyFilters();
  }

  onSearch(value: string) {
    this.searchText = (value || '').trim().toLowerCase();
    this.applyFilters();
  }

  // ----- Core filtering -----
  private applyFilters() {
    const t = this.filterType;
    const s = this.filterStatus;
    const c = this.filterCategory;
    const q = this.searchText;

    this.filteredRfxes = this.allRfxes.filter((r) => {
      const byType = t === 'all' || r.type === t;
      const byStatus = s === 'all' || r.status === s;
      const byCategory = c === 'all' || r.category === c;

      const bySearch =
        !q ||
        r.title.toLowerCase().includes(q) ||
        r.category.toLowerCase().includes(q) ||
        r.type.toLowerCase().includes(q) ||
        r.status.toLowerCase().includes(q) ||
        r.id.toLowerCase().includes(q);

      return byType && byStatus && byCategory && bySearch;
    });
  }
}
