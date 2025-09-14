import { RfxAttachmentDto, RfxCommitteeMemberDto, RfxEvaluationCriterionDto } from './rfx-upsert.dto';

// Full detail returned by GET /api/rfx/{id}
export interface RfxDetails {
  id: string;
  type: number; // 1..4
  title: string;
  category: string;
  department: string;
  description: string;
  estimatedBudget: number | null;
  hideBudgetFromSuppliers: boolean;
  publicationDate: string;
  closingDate: string;
  currency: 'QAR' | 'USD' | 'EUR' | 'GBP';
  priority: number; // 1..4

  tenderBondRequired: boolean;
  bondAmountPercent: number | null;
  bondValidityDays: number | null;

  contactPerson: string;
  contactEmail: string;
  contactPhone: string;
  clarificationDeadline: string | null;

  scopeOfWork: string | null;
  technicalSpecifications: string | null;
  deliverables: string | null;
  timeline: string | null;

  minimumQualifyingScore: number;
  evaluationNotes: string | null;

  requiredDocuments: string[];
  otherRequiredDocument: string | null;

  useStandardTerms: boolean;
  customTerms: string | null;
  termsAttachment: RfxAttachmentDto | null;

  evaluationCriteria: RfxEvaluationCriterionDto[];
  committeeMembers: RfxCommitteeMemberDto[];
  attachments: RfxAttachmentDto[];
}
