// import { inject, Injectable } from '@angular/core';
// import { HttpClient, HttpParams } from '@angular/common/http';
// import { Observable } from 'rxjs';

// import { environment } from '../environments/environment';
// import { RfxListItem } from '../models/rfx-list-item';
// import { RfxUpsertDto } from '../models/rfx-upsert.dto';
// import { RfxDetails } from '../models/rfx-details';

// @Injectable({ providedIn: 'root' })
// export class RfxService {
//   private http = inject(HttpClient);
//   private base = `${environment.apiBase}/api/rfx`;

//   // LIST
//   getAll(params?: Record<string, string | number | boolean | undefined>): Observable<RfxListItem[]> {
//     let hp = new HttpParams();
//     if (params) {
//       for (const [k, v] of Object.entries(params)) {
//         if (v !== undefined && v !== null && v !== '') hp = hp.set(k, String(v));
//       }
//     }
//     return this.http.get<RfxListItem[]>(this.base, { params: hp });
//   }

//   // DETAILS
//   getById(id: string): Observable<RfxDetails> {
//     return this.http.get<RfxDetails>(`${this.base}/${id}`);
//   }

//   // CREATE
//   create(dto: RfxUpsertDto): Observable<{ id: string; code: string }> {
//     return this.http.post<{ id: string; code: string }>(this.base, dto);
//   }

//   // UPDATE
//   update(id: string, dto: RfxUpsertDto): Observable<void> {
//     return this.http.put<void>(`${this.base}/${id}`, dto);
//   }

//   // DELETE
//   delete(id: string): Observable<void> {
//     return this.http.delete<void>(`${this.base}/${id}`);
//   }
// }



import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { RfxUpsertDto, PublicRfxDto } from '../models/rfx-upsert.dto';

export type RfxTypeLabel = 'RFP' | 'RFQ' | 'RFI' | 'EOI';
export type RfxStatusLabel =
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
  type: RfxTypeLabel;
  category: string;
  publicationDate: string;
  closingDate: string;
  status: RfxStatusLabel;
  responses: number;
}

@Injectable({ providedIn: 'root' })
export class RfxService {
  private http = inject(HttpClient);
  private base = `${environment.apiBase}/api/rfx`;
  private pubBase = `${environment.apiBase}/api/public/rfx`;

  getAll(
    params?: Record<string, string | number | boolean | undefined>
  ): Observable<RfxListItem[]> {
    let hp = new HttpParams();
    if (params) {
      for (const [k, v] of Object.entries(params)) {
        if (v !== undefined && v !== null && v !== '') hp = hp.set(k, String(v));
      }
    }
    return this.http.get<RfxListItem[]>(this.base, { params: hp });
  }

  getById(id: string) {
    return this.http.get<RfxListItem>(`${this.base}/${id}`);
  }

  // IMPORTANT: backend now returns { id, code, publicToken }
  create(dto: RfxUpsertDto): Observable<{ id: string; code?: string; publicToken?: string }> {
    return this.http.post<{ id: string; code?: string; publicToken?: string }>(this.base, dto);
  }

  update(id: string, payload: unknown) {
    return this.http.put<void>(`${this.base}/${id}`, payload);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.base}/${id}`);
  }

  // Public read
  getPublicByToken(token: string): Observable<PublicRfxDto> {
    return this.http.get<PublicRfxDto>(`${this.pubBase}/${token}`);
  }
}
