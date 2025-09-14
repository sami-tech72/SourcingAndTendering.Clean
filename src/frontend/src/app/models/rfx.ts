export interface RfxListItem {
  id: string;
  title: string;
  type: number;          // 1..4
  category: string;
  department: string;
  closingDate: string;   // ISO
  priority: number;      // 1..4
  status: number;        // enum index
}
