import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RfxService } from '../../../../services/rfx.service';
import {
  RfxCommitteeMemberDto,
  RfxEvaluationCriterionDto,
  RfxUpsertDto,
} from '../../../../models/rfx-upsert.dto';

@Component({
  selector: 'app-create-new-rfx',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-new-rfx.component.html',
  styleUrl: './create-new-rfx.component.css',
})
export class CreateNewRfxComponent {
  constructor(private api: RfxService, private router: Router) {}

  // wizard
  step = 1;
  setStep(s: number) {
    this.step = s;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // Step 1 – basic
  rfxType = 'rfp'; // rfi|rfq|rfp
  category = '';
  title = '';
  description = '';
  estimatedBudget: number | null = null;
  hideBudget = false;
  department = '';
  publicationDate = '';
  closingDate = '';
  currency: 'QAR' | 'USD' | 'EUR' | 'GBP' = 'QAR';
  priority: 1 | 2 | 3 | 4 = 2;

  tenderBondRequired = false;
  bondAmountPercent: number | null = null;
  bondValidityDays: number | null = null;

  contactPerson = '';
  contactEmail = '';
  contactPhone = '';
  clarificationDeadline = '';

  // Step 2 – requirements
  scopeOfWork = '';
  technicalSpecifications = '';
  deliverables = '';
  timeline = '';

  // Step 3 – evaluation
  evaluationCriteria: RfxEvaluationCriterionDto[] = [
    { category: 'Technical', name: 'Technical Compliance', weightPercent: 25 },
    {
      category: 'Technical',
      name: 'Experience & Qualifications',
      weightPercent: 15,
    },
    {
      category: 'Technical',
      name: 'Methodology & Approach',
      weightPercent: 20,
    },
    { category: 'Commercial', name: 'Price', weightPercent: 30 },
    { category: 'Commercial', name: 'Payment Terms', weightPercent: 10 },
  ];
  minimumQualifyingScore = 70;
  evaluationNotes = '';

  committeeMembers: RfxCommitteeMemberDto[] = [
    { fullName: 'John Smith', role: 'Procurement Manager' },
    { fullName: 'Sarah Ahmed', role: 'Technical Expert' },
  ];

  // Step 4 – publish
  approvalChecked = false;
  publishOption: 'now' | 'scheduled' | 'draft' = 'now';
  supplierOption: 'all' | 'selected' = 'all';
  selectedSupplierIds: string[] = [];

  // attachments (names only; hook real upload later)
  attachments: { fileName: string; storagePath: string }[] = [
    // { fileName: 'Detailed_Specifications.pdf', storagePath: '/files/specs/Detailed_Specifications.pdf' }
  ];

  loading = false;
  error = '';
  successId = '';

  // derived
  totalWeight = computed(() =>
    this.evaluationCriteria.reduce((sum, c) => sum + (+c.weightPercent || 0), 0)
  );

  addTechnicalCriterion() {
    this.evaluationCriteria.push({
      category: 'Technical',
      name: '',
      weightPercent: 0,
    });
  }
  addCommercialCriterion() {
    this.evaluationCriteria.push({
      category: 'Commercial',
      name: '',
      weightPercent: 0,
    });
  }
  removeCriterion(i: number) {
    this.evaluationCriteria.splice(i, 1);
  }

  addCommitteeMember() {
    this.committeeMembers.push({ fullName: '', role: '' });
  }
  removeCommitteeMember(i: number) {
    this.committeeMembers.splice(i, 1);
  }

  private mapTypeToInt(): 1 | 2 | 3 | 4 {
    // align with your backend enum
    switch (this.rfxType) {
      case 'rfi':
        return 1 as const; // adjust if needed
      case 'rfq':
        return 3 as const;
      case 'rfp':
        return 4 as const;
      default:
        return 4 as const;
    }
  }

  private toIso(d: string): string {
    return d;
  } // already yyyy-MM-dd from <input type="date">

  publishRfx() {
    this.error = '';
    if (!this.approvalChecked) {
      this.error = 'Please confirm approval before publishing.';
      return;
    }
    if (this.totalWeight() !== 100) {
      this.error = 'Evaluation criteria weights must total 100%.';
      return;
    }

    const dto: RfxUpsertDto = {
      type: this.mapTypeToInt(),
      title: this.title,
      category: this.category,
      department: this.department,
      description: this.description,
      estimatedBudget: this.estimatedBudget,
      hideBudgetFromSuppliers: this.hideBudget,
      publicationDate: this.toIso(this.publicationDate),
      closingDate: this.toIso(this.closingDate),
      currency: this.currency,
      priority: this.priority,
      tenderBondRequired: this.tenderBondRequired,
      bondAmountPercent: this.tenderBondRequired
        ? this.bondAmountPercent ?? 0
        : null,
      bondValidityDays: this.tenderBondRequired
        ? this.bondValidityDays ?? 0
        : null,

      contactPerson: this.contactPerson,
      contactEmail: this.contactEmail,
      contactPhone: this.contactPhone,
      clarificationDeadline: this.toIso(this.clarificationDeadline),

      scopeOfWork: this.scopeOfWork,
      technicalSpecifications: this.technicalSpecifications,
      deliverables: this.deliverables,
      timeline: this.timeline,

      minimumQualifyingScore: this.minimumQualifyingScore,
      evaluationNotes: this.evaluationNotes || null,

      evaluationCriteria: this.evaluationCriteria.map((c) => ({
        category: c.category,
        name: c.name?.trim() || '',
        weightPercent: +c.weightPercent || 0,
      })),

      committeeMembers: this.committeeMembers.map((m) => ({
        fullName: m.fullName?.trim() || '',
        role: m.role?.trim() || '',
      })),

      attachments: this.attachments,
    };

    this.loading = true;
    this.api.create(dto).subscribe({
      next: (res) => {
        this.loading = false;
        this.successId = res.id;
        // go to management or show success; here we route:
        this.router.navigate(['/procurement/rfx-management']);
      },
      error: (err) => {
        this.loading = false;
        this.error =
          err?.error?.message ||
          (typeof err?.error === 'string' ? err.error : '') ||
          'Failed to publish RFx.';
        console.error(err);
      },
    });
  }
}
