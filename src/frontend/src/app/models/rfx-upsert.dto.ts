export interface RfxEvaluationCriterionDto {
  category: 'Technical' | 'Commercial';
  name: string;
  weightPercent: number;
}

export interface RfxCommitteeMemberDto {
  fullName: string;
  role: string;
}

export interface RfxAttachmentDto {
  fileName: string;
  storagePath: string;
}

export interface RfxUpsertDto {
  type: 1 | 2 | 3 | 4; // 1=EOI, 2=RFI, 3=RFQ, 4=RFP (adjust if your enum differs)
  title: string;
  category: string;
  department: string;
  description: string;
  estimatedBudget?: number | null;
  hideBudgetFromSuppliers: boolean;
  publicationDate: string; // ISO yyyy-MM-dd
  closingDate: string; // ISO yyyy-MM-dd
  currency: 'QAR' | 'USD' | 'EUR' | 'GBP';
  priority: 1 | 2 | 3 | 4; // 1=Low 2=Medium 3=High 4=Urgent
  tenderBondRequired: boolean;
  bondAmountPercent?: number | null; // optional if tenderBondRequired
  bondValidityDays?: number | null; // optional if tenderBondRequired
  contactPerson: string;
  contactEmail: string;
  contactPhone: string;
  clarificationDeadline: string; // ISO yyyy-MM-dd
  scopeOfWork: string;
  technicalSpecifications: string;
  deliverables: string;
  timeline: string;
  minimumQualifyingScore: number;
  evaluationNotes?: string | null;
  evaluationCriteria: RfxEvaluationCriterionDto[];
  committeeMembers: RfxCommitteeMemberDto[];
  attachments: RfxAttachmentDto[];
}
