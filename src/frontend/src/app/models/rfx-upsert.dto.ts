// Keep your existing DTOs and add the public-view DTO below.

export type RfxType = 'RFP' | 'RFQ' | 'RFI' | 'EOI';

export interface RfxAttachmentDto {
  fileName: string;
  storagePath: string;
}

export interface RfxEvaluationCriterionDto {
  category: 'Technical' | 'Commercial';
  name: string;
  weightPercent: number;
}

export interface RfxCommitteeMemberDto {
  fullName: string;
  role: string;
}

export interface RfxUpsertDto {
  // BASIC
  type: 1 | 2 | 3 | 4; // map: rfi=1, rfp=2, rfq=3, eoi=4 (your backend enum)
  title: string;
  category: string;
  department: string;
  description: string;
  estimatedBudget: number | null;
  hideBudgetFromSuppliers: boolean;
  publicationDate: string; // ISO (yyyy-MM-dd or yyyy-MM-ddTHH:mm:ss)
  closingDate: string;
  currency: 'QAR' | 'USD' | 'EUR' | 'GBP';
  priority: 1 | 2 | 3 | 4;

  // TENDER BOND
  tenderBondRequired: boolean;
  bondAmountPercent: number | null;
  bondValidityDays: number | null;

  // CONTACTS
  contactPerson: string;
  contactEmail: string;
  contactPhone: string;
  clarificationDeadline: string; // ISO or empty string

  // REQUIREMENTS
  scopeOfWork: string;
  technicalSpecifications: string;
  deliverables: string;
  timeline: string;

  // REQUIRED DOCS + TERMS
  requiredDocuments: string[];
  otherRequiredDocument: string | null;
  useStandardTerms: boolean;
  customTerms: string | null;
  termsAttachment: RfxAttachmentDto | null;

  // EVALUATION
  minimumQualifyingScore: number;
  evaluationNotes: string | null;
  evaluationCriteria: RfxEvaluationCriterionDto[];

  // COMMITTEE
  committeeMembers: RfxCommitteeMemberDto[];

  // ATTACHMENTS
  attachments: RfxAttachmentDto[];

  // PUBLISH
  publishOption: 'now' | 'scheduled' | 'draft';
  supplierOption: 'all' | 'selected';
  selectedSupplierIds: string[];
}

// Public read-only tender DTO (from /api/public/rfx/:token)
export interface PublicRfxDto {
  code: string;
  title: string;
  category: string;
  department: string;
  description: string;
  type: string; // "RFP"/"RFQ"/"RFI"/"EOI"
  publicationDate: string;
  closingDate: string;
  currency: string;

  estimatedBudget: number | null; // null when hidden
  tenderBondRequired: boolean;
  bondAmountPercent?: number | null;
  bondValidityDays?: number | null;

  contactPerson: string;
  contactEmail: string;
  contactPhone: string;

  scopeOfWork?: string | null;
  technicalSpecifications?: string | null;
  deliverables?: string | null;
  timeline?: string | null;

  evaluationCriteria: { category: string; name: string; weightPercent: number }[];
  minimumQualifyingScore: number;

  requiredDocuments: string[];
  otherRequiredDocument?: string | null;

  useStandardTerms: boolean;
  customTerms?: string | null;

  attachments: { fileName: string; storagePath: string }[];
}
