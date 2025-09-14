export interface RfxListItem {
  id: string;
  title: string;
  type: 'RFP' | 'RFQ' | 'RFI' | 'EOI';
  category: string;
  publicationDate: string;
  closingDate: string;
  status:
    | 'Draft'
    | 'Pending Approval'
    | 'Published'
    | 'Under Evaluation'
    | 'Awarded'
    | 'Completed'
    | 'Cancelled';
  responses: number;
}
