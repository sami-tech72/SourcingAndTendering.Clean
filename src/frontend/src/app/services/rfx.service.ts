import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { RfxListItem } from '../models/rfx';
import { RfxUpsertDto } from '../models/rfx-upsert.dto';

@Injectable({ providedIn: 'root' })
export class RfxService {
  private http = inject(HttpClient);
  private base = `${environment.apiBase}/api/rfx`;

  getAll(
    params?: Record<string, string | number | boolean | undefined>
  ): Observable<RfxListItem[]> {
    let hp = new HttpParams();
    if (params) {
      for (const [k, v] of Object.entries(params)) {
        if (v !== undefined && v !== null && v !== '')
          hp = hp.set(k, String(v));
      }
    }
    return this.http.get<RfxListItem[]>(this.base, { params: hp });
  }

  getById(id: string) {
    return this.http.get<RfxListItem>(`${this.base}/${id}`);
  }

  create(dto: RfxUpsertDto): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.base, dto);
  }

  update(id: string, payload: unknown) {
    return this.http.put<void>(`${this.base}/${id}`, payload);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
