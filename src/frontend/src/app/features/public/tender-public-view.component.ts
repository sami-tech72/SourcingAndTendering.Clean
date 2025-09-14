import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RfxService } from '../../services/rfx.service';
import { PublicRfxDto } from '../../models/rfx-upsert.dto';
import { environment } from '../../environments/environment';

type Viewer = { name: string; avatarUrl: string };
type Attachment = { fileName: string; storagePath: string; contentType?: string; url?: string };
type PublicRfxView = PublicRfxDto & {
  views?: number;
  activeViewers?: Viewer[];
  attachments: Attachment[];
};

@Component({
  standalone: true,
  selector: 'app-tender-public-view',
  imports: [CommonModule],
  templateUrl: './tender-public-view.component.html',
  styleUrls: ['./tender-public-view.component.css'],
})
export class TenderPublicViewComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private api = inject(RfxService);

  loading = false;
  error = '';
  data: PublicRfxView | null = null;

  modalImageUrl: string | null = null;

  ngOnInit(): void {
    const token = this.route.snapshot.paramMap.get('token')!;
    this.fetch(token);
  }

  private fetch(token: string) {
    this.loading = true;
    this.error = '';
    this.data = null;

    this.api.getPublicByToken(token).subscribe({
      next: (d) => {
        // ensure attachments array
        const withDefaults: PublicRfxView = {
          ...d,
          attachments: (d as any).attachments ?? [],
          views: (d as any).views,
          activeViewers: (d as any).activeViewers,
        };
        this.data = withDefaults;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.error?.message || 'Tender not found or no longer public.';
        this.loading = false;
      }
    });
  }

  // ------- Template helpers -------
  trackByIndex = (i: number) => i;
  trackByFileName = (_: number, a: Attachment) => a?.fileName || _;

  hasImages(atts: Attachment[] | null | undefined) {
    return !!atts?.some(a => this.isImage(a));
  }
  hasNonImages(atts: Attachment[] | null | undefined) {
    return !!atts?.some(a => !this.isImage(a));
  }

  isImage(a: Attachment) {
    const ct = (a?.contentType || '').toLowerCase();
    const ext = (a?.fileName || '').split('.').pop()?.toLowerCase() || '';
    return ct.startsWith('image/') || ['png','jpg','jpeg','gif','webp','bmp','svg'].includes(ext);
  }

  getUrl(a: { fileName: string; storagePath: string; url?: string }) {
  if (a.url) return a.url;                             // already absolute
  if (!a.storagePath) return '';
  if (/^https?:\/\//i.test(a.storagePath)) return a.storagePath;  // absolute path
  // prefix relative storagePath with your API base
  return `${environment.apiBase}${a.storagePath.startsWith('/') ? '' : '/'}${a.storagePath}`;
}

  openImage(url: string) {
    this.modalImageUrl = url;
    const el = document.getElementById('imageModal');
    // If Bootstrap JS is present, use modal; otherwise fallback to new tab
    const bs = (window as any).bootstrap?.Modal;
    if (el && bs) new bs(el).show(); else window.open(url, '_blank', 'noopener');
  }
}
