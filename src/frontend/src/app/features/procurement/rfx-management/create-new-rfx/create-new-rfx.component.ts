// import { Component } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { FormsModule } from '@angular/forms';
// import { Router } from '@angular/router';
// import { RfxService } from '../../../../services/rfx.service';
// import {
//   RfxCommitteeMemberDto,
//   RfxEvaluationCriterionDto,
//   RfxUpsertDto,
//   RfxAttachmentDto,
// } from '../../../../models/rfx-upsert.dto';

// @Component({
//   selector: 'app-create-new-rfx',
//   standalone: true,
//   imports: [CommonModule, FormsModule],
//   templateUrl: './create-new-rfx.component.html',
//   styleUrls: ['./create-new-rfx.component.css'], // <-- plural
// })
// export class CreateNewRfxComponent {
//   constructor(private api: RfxService, private router: Router) {}

//   // wizard
//   step = 1;
//   published = false;        // show success screen after publish
//   publishedId = '';         // e.g., GUID from API
//   publishedCode = '';       // e.g., "RFP-2025-021" (optional display)
//   setStep(s: number) {
//     this.step = s;
//     window.scrollTo({ top: 0, behavior: 'smooth' });
//   }

//   // Step 1 – basic
//   rfxType: 'rfi' | 'rfq' | 'rfp' = 'rfp';
//   category = '';
//   title = '';
//   description = '';
//   estimatedBudget: number | null = null;
//   hideBudget = false;
//   department = '';
//   publicationDate = '';
//   closingDate = '';
//   currency: 'QAR' | 'USD' | 'EUR' | 'GBP' = 'QAR';
//   priority: 1 | 2 | 3 | 4 = 2;

//   tenderBondRequired = false;
//   bondAmountPercent: number | null = null;
//   bondValidityDays: number | null = null;

//   contactPerson = '';
//   contactEmail = '';
//   contactPhone = '';
//   clarificationDeadline = '';

//   // Step 2 – requirements text
//   scopeOfWork = '';
//   technicalSpecifications = '';
//   deliverables = '';
//   timeline = '';

//   // Step 2 – required documents (checkboxes)
//   docCompanyProfile = true;
//   docTradeLicense = true;
//   docFinancials = true;
//   docReferences = true;
//   docCertifications = false;
//   docMethodology = false;
//   docOther = false;
//   otherDocText = '';

//   // Step 2 – attachments (names only in demo)
//   attachmentInputs = [0, 1, 2];
//   attachments: RfxAttachmentDto[] = [];
//   addAttachmentInput() { this.attachmentInputs.push(this.attachmentInputs.length); }
//   onAttachmentPicked(event: Event, index: number) {
//     const input = event.target as HTMLInputElement;
//     const file = input?.files?.[0];
//     if (!file) return;
//     const item: RfxAttachmentDto = { fileName: file.name, storagePath: `/uploads/${file.name}` };
//     this.attachments[index] = item;
//     this.attachments = [...this.attachments].filter(Boolean) as RfxAttachmentDto[];
//   }

//   // Step 2 – Terms & Conditions
//   useStandardTerms = true;
//   customTerms = '';
//   termsAttachment: RfxAttachmentDto | null = null;
//   onTermsAttachmentPicked(event: Event) {
//     const input = event.target as HTMLInputElement;
//     const file = input?.files?.[0];
//     this.termsAttachment = file
//       ? { fileName: file.name, storagePath: `/uploads/${file.name}` }
//       : null;
//   }

//   // Step 3 – evaluation
//   evaluationCriteria: RfxEvaluationCriterionDto[] = [
//     { category: 'Technical', name: 'Technical Compliance', weightPercent: 25 },
//     { category: 'Technical', name: 'Experience & Qualifications', weightPercent: 15 },
//     { category: 'Technical', name: 'Methodology & Approach', weightPercent: 20 },
//     { category: 'Commercial', name: 'Price', weightPercent: 30 },
//     { category: 'Commercial', name: 'Payment Terms', weightPercent: 10 },
//   ];
//   minimumQualifyingScore = 70;
//   evaluationNotes = '';

//   committeeMembers: RfxCommitteeMemberDto[] = [
//     { fullName: 'John Smith', role: 'Procurement Manager' },
//     { fullName: 'Sarah Ahmed', role: 'Technical Expert' },
//   ];

//   // Step 4 – publish
//   approvalChecked = false;
//   publishOption: 'now' | 'scheduled' | 'draft' = 'now';
//   supplierOption: 'all' | 'selected' = 'all';
//   selectedSupplierIds: string[] = [];

//   loading = false;
//   error = '';

//   // simple method (not signal) so it always reflects latest array state
//   totalWeight(): number {
//     return this.evaluationCriteria.reduce((sum, c) => sum + (+c.weightPercent || 0), 0);
//   }

//   requiredDocsList(): string[] {
//     const out: string[] = [];
//     if (this.docCompanyProfile) out.push('Company Profile');
//     if (this.docTradeLicense) out.push('Trade License');
//     if (this.docFinancials) out.push('Financial Statements');
//     if (this.docReferences) out.push('References/Past Projects');
//     if (this.docCertifications) out.push('Certifications');
//     if (this.docMethodology) out.push('Methodology');
//     if (this.docOther && this.otherDocText.trim()) out.push(this.otherDocText.trim());
//     return out;
//   }

//   addTechnicalCriterion() {
//     this.evaluationCriteria.push({ category: 'Technical', name: '', weightPercent: 0 });
//   }
//   addCommercialCriterion() {
//     this.evaluationCriteria.push({ category: 'Commercial', name: '', weightPercent: 0 });
//   }
//   removeCriterion(i: number) {
//     this.evaluationCriteria.splice(i, 1);
//   }
//   addCommitteeMember() { this.committeeMembers.push({ fullName: '', role: '' }); }
//   removeCommitteeMember(i: number) { this.committeeMembers.splice(i, 1); }

//   private mapTypeToInt(): 1 | 2 | 3 | 4 {
//     // align with backend enum (Rfi=1, Rfp=2, Rfq=3, Eoi=4)
//     switch (this.rfxType) {
//       case 'rfi': return 1;
//       case 'rfp': return 2;
//       case 'rfq': return 3;
//       default:    return 2;
//     }
//   }

//   // if you want full ISO: return d ? `${d}T00:00:00` : '';
//   private toIso(d: string): string { return d || ''; }

//   publishRfx() {
//     this.error = '';
//     if (!this.approvalChecked) {
//       this.error = 'Please confirm approval before publishing.';
//       return;
//     }
//     if (this.totalWeight() !== 100) {
//       this.error = 'Evaluation criteria weights must total 100%.';
//       return;
//     }

//     const dto: RfxUpsertDto = {
//       // BASIC
//       type: this.mapTypeToInt(),
//       title: this.title,
//       category: this.category,
//       department: this.department,
//       description: this.description,
//       estimatedBudget: this.estimatedBudget,
//       hideBudgetFromSuppliers: this.hideBudget,
//       publicationDate: this.toIso(this.publicationDate),
//       closingDate: this.toIso(this.closingDate),
//       currency: this.currency,
//       priority: this.priority,

//       // TENDER BOND
//       tenderBondRequired: this.tenderBondRequired,
//       bondAmountPercent: this.tenderBondRequired ? this.bondAmountPercent ?? 0 : null,
//       bondValidityDays:  this.tenderBondRequired ? this.bondValidityDays  ?? 0 : null,

//       // CONTACTS
//       contactPerson: this.contactPerson,
//       contactEmail: this.contactEmail,
//       contactPhone: this.contactPhone,
//       clarificationDeadline: this.toIso(this.clarificationDeadline),

//       // REQUIREMENTS
//       scopeOfWork: this.scopeOfWork,
//       technicalSpecifications: this.technicalSpecifications,
//       deliverables: this.deliverables,
//       timeline: this.timeline,

//       // REQUIRED DOCS
//       requiredDocuments: this.requiredDocsList(),
//       otherRequiredDocument: this.docOther ? (this.otherDocText.trim() || null) : null,

//       // TERMS
//       useStandardTerms: this.useStandardTerms,
//       customTerms: this.useStandardTerms ? null : (this.customTerms.trim() || null),
//       termsAttachment: this.useStandardTerms ? null : this.termsAttachment ?? null,

//       // EVALUATION
//       minimumQualifyingScore: this.minimumQualifyingScore,
//       evaluationNotes: this.evaluationNotes || null,
//       evaluationCriteria: this.evaluationCriteria.map(c => ({
//         category: c.category,
//         name: c.name?.trim() || '',
//         weightPercent: +c.weightPercent || 0,
//       })),

//       // COMMITTEE
//       committeeMembers: this.committeeMembers.map(m => ({
//         fullName: m.fullName?.trim() || '',
//         role: m.role?.trim() || '',
//       })),

//       // ATTACHMENTS
//       attachments: this.attachments,

//       // PUBLISH (passed through, even if backend ignores for now)
//       publishOption: this.publishOption,
//       supplierOption: this.supplierOption,
//       selectedSupplierIds: this.selectedSupplierIds,
//     };

//     this.loading = true;
//     this.api.create(dto).subscribe({
//       next: (res) => {
//         this.loading = false;
//         this.publishedId = res.id;
//         this.publishedCode = (res as any).code ?? ''; // if service returns {id, code}
//         this.published = true;
//         window.scrollTo({ top: 0, behavior: 'smooth' });
//       },
//       error: (err) => {
//         this.loading = false;
//         this.error =
//           err?.error?.message ||
//           (typeof err?.error === 'string' ? err.error : '') ||
//           'Failed to publish RFx.';
//         console.error(err);
//       },
//     });
//   }
// }

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RfxService } from '../../../../services/rfx.service';
import {
  RfxCommitteeMemberDto,
  RfxEvaluationCriterionDto,
  RfxUpsertDto,
  RfxAttachmentDto,
} from '../../../../models/rfx-upsert.dto';

@Component({
  selector: 'app-create-new-rfx',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-new-rfx.component.html',
  styleUrls: ['./create-new-rfx.component.css'], // plural
})
export class CreateNewRfxComponent {
  constructor(private api: RfxService, private router: Router) {}

  // wizard
  step = 1;
  published = false;        // show success screen after publish
  publishedId = '';         // GUID from API
  publishedCode = '';       // e.g., "RFP-2025-021"
  publishedToken: string | null = null; // public token from API

  get publicUrl(): string | null {
    return this.publishedToken ? `${window.location.origin}/tenders/${this.publishedToken}` : null;
  }

  setStep(s: number) {
    this.step = s;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // Step 1 – basic
  rfxType: 'rfi' | 'rfq' | 'rfp' = 'rfp';
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

  // Step 2 – requirements text
  scopeOfWork = '';
  technicalSpecifications = '';
  deliverables = '';
  timeline = '';

  // Step 2 – required documents (checkboxes)
  docCompanyProfile = true;
  docTradeLicense = true;
  docFinancials = true;
  docReferences = true;
  docCertifications = false;
  docMethodology = false;
  docOther = false;
  otherDocText = '';

  // Step 2 – attachments (names only in demo)
  attachmentInputs = [0, 1, 2];
  attachments: RfxAttachmentDto[] = [];
  addAttachmentInput() { this.attachmentInputs.push(this.attachmentInputs.length); }
  onAttachmentPicked(event: Event, index: number) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];
    if (!file) return;
    const item: RfxAttachmentDto = { fileName: file.name, storagePath: `/uploads/${file.name}` };
    this.attachments[index] = item;
    this.attachments = [...this.attachments].filter(Boolean) as RfxAttachmentDto[];
  }

  // Step 2 – Terms & Conditions
  useStandardTerms = true;
  customTerms = '';
  termsAttachment: RfxAttachmentDto | null = null;
  onTermsAttachmentPicked(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];
    this.termsAttachment = file
      ? { fileName: file.name, storagePath: `/uploads/${file.name}` }
      : null;
  }

  // Step 3 – evaluation
  evaluationCriteria: RfxEvaluationCriterionDto[] = [
    { category: 'Technical', name: 'Technical Compliance', weightPercent: 25 },
    { category: 'Technical', name: 'Experience & Qualifications', weightPercent: 15 },
    { category: 'Technical', name: 'Methodology & Approach', weightPercent: 20 },
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

  loading = false;
  error = '';

  // derived
  totalWeight(): number {
    return this.evaluationCriteria.reduce((sum, c) => sum + (+c.weightPercent || 0), 0);
  }

  requiredDocsList(): string[] {
    const out: string[] = [];
    if (this.docCompanyProfile) out.push('Company Profile');
    if (this.docTradeLicense) out.push('Trade License');
    if (this.docFinancials) out.push('Financial Statements');
    if (this.docReferences) out.push('References/Past Projects');
    if (this.docCertifications) out.push('Certifications');
    if (this.docMethodology) out.push('Methodology');
    if (this.docOther && this.otherDocText.trim()) out.push(this.otherDocText.trim());
    return out;
  }

  addTechnicalCriterion() { this.evaluationCriteria.push({ category: 'Technical', name: '', weightPercent: 0 }); }
  addCommercialCriterion() { this.evaluationCriteria.push({ category: 'Commercial', name: '', weightPercent: 0 }); }
  removeCriterion(i: number) { this.evaluationCriteria.splice(i, 1); }
  addCommitteeMember() { this.committeeMembers.push({ fullName: '', role: '' }); }
  removeCommitteeMember(i: number) { this.committeeMembers.splice(i, 1); }

  private mapTypeToInt(): 1 | 2 | 3 | 4 {
    // align with backend enum (Rfi=1, Rfp=2, Rfq=3, Eoi=4)
    switch (this.rfxType) {
      case 'rfi': return 1;
      case 'rfp': return 2;
      case 'rfq': return 3;
      default:    return 2;
    }
  }

  private toIso(d: string): string { return d || ''; }

  copyPublicLink() {
    if (this.publicUrl) navigator.clipboard.writeText(this.publicUrl);
  }

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
      // BASIC
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

      // TENDER BOND
      tenderBondRequired: this.tenderBondRequired,
      bondAmountPercent: this.tenderBondRequired ? this.bondAmountPercent ?? 0 : null,
      bondValidityDays:  this.tenderBondRequired ? this.bondValidityDays  ?? 0 : null,

      // CONTACTS
      contactPerson: this.contactPerson,
      contactEmail: this.contactEmail,
      contactPhone: this.contactPhone,
      clarificationDeadline: this.toIso(this.clarificationDeadline),

      // REQUIREMENTS
      scopeOfWork: this.scopeOfWork,
      technicalSpecifications: this.technicalSpecifications,
      deliverables: this.deliverables,
      timeline: this.timeline,

      // REQUIRED DOCS + TERMS
      requiredDocuments: this.requiredDocsList(),
      otherRequiredDocument: this.docOther ? (this.otherDocText.trim() || null) : null,
      useStandardTerms: this.useStandardTerms,
      customTerms: this.useStandardTerms ? null : (this.customTerms.trim() || null),
      termsAttachment: this.useStandardTerms ? null : this.termsAttachment ?? null,

      // EVALUATION
      minimumQualifyingScore: this.minimumQualifyingScore,
      evaluationNotes: this.evaluationNotes || null,
      evaluationCriteria: this.evaluationCriteria.map(c => ({
        category: c.category,
        name: c.name?.trim() || '',
        weightPercent: +c.weightPercent || 0,
      })),

      // COMMITTEE
      committeeMembers: this.committeeMembers.map(m => ({
        fullName: m.fullName?.trim() || '',
        role: m.role?.trim() || '',
      })),

      // ATTACHMENTS
      attachments: this.attachments,

      // PUBLISH (passed through)
      publishOption: this.publishOption,
      supplierOption: this.supplierOption,
      selectedSupplierIds: this.selectedSupplierIds,
    };

    this.loading = true;
    this.api.create(dto).subscribe({
      next: (res) => {
        this.loading = false;
        this.publishedId = res.id;
        this.publishedCode = res.code ?? '';
        this.publishedToken = res.publicToken ?? null; // needs backend support
        this.published = true;
        window.scrollTo({ top: 0, behavior: 'smooth' });
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
