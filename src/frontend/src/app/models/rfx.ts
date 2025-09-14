export interface RfxListItem {
  id: string;
  title: string;
  type: string; // "RFP" | "RFQ" | "RFI" | "EOI" | etc
  category: string;
  publicationDate: string; // ISO
  closingDate: string; // ISO
  status?: string; // optional
  responses?: number; // optional
}
