//namespace Domain.Entities;

//public enum RfxType { Rfi = 1, Rfp = 2, Rfq = 3, Eoi = 4 }
//public enum Priority { Low = 1, Medium = 2, High = 3, Urgent = 4 }
//public enum RfxStatus { Draft = 0, PendingApproval = 1, Published = 2, Closed = 3, Cancelled = 4 }

//public class Rfx
//{
//    public Guid Id { get; set; }

//    // Basics
//    public RfxType Type { get; set; }
//    public string Title { get; set; } = default!;
//    public string Category { get; set; } = default!;
//    public string Department { get; set; } = default!;
//    public string Description { get; set; } = default!;

//    // Money
//    public decimal? EstimatedBudget { get; set; }
//    public bool HideBudgetFromSuppliers { get; set; }
//    public string Currency { get; set; } = "QAR";

//    // Dates / priority
//    public DateTime PublicationDate { get; set; }
//    public DateTime ClosingDate { get; set; }
//    public Priority Priority { get; set; }

//    // Tender bond
//    public bool TenderBondRequired { get; set; }
//    public int? BondAmountPercent { get; set; }   // 0..100
//    public int? BondValidityDays { get; set; }    // >0

//    // Contacts
//    public string ContactPerson { get; set; } = default!;
//    public string ContactEmail { get; set; } = default!;
//    public string ContactPhone { get; set; } = default!;
//    public DateTime? ClarificationDeadline { get; set; }

//    // Requirements
//    public string? ScopeOfWork { get; set; }
//    public string? TechnicalSpecifications { get; set; }
//    public string? Deliverables { get; set; }
//    public string? Timeline { get; set; }

//    // Evaluation
//    public int MinimumQualifyingScore { get; set; }
//    public string? EvaluationNotes { get; set; }

//    // Required documents (JSON blob)
//    public string RequiredDocumentsJson { get; set; } = "[]";
//    public string? OtherRequiredDocument { get; set; }

//    // Terms
//    public bool UseStandardTerms { get; set; } = true;
//    public string? CustomTerms { get; set; }
//    public string? TermsFileName { get; set; }
//    public string? TermsStoragePath { get; set; }

//    // Publish options + supplier selection (stored)
//    public string PublishOption { get; set; } = "now";   // now | scheduled | draft
//    public string SupplierOption { get; set; } = "all";  // all | selected
//    public string SelectedSupplierIdsJson { get; set; } = "[]";

//    // Status lifecycle
//    public RfxStatus Status { get; set; } = RfxStatus.Draft;
//    public DateTime? PublishedAt { get; set; }

//    // Children
//    public List<RfxEvaluationCriterion> EvaluationCriteria { get; set; } = [];
//    public List<RfxCommitteeMember> CommitteeMembers { get; set; } = [];
//    public List<RfxAttachment> Attachments { get; set; } = [];
//}

//public class RfxEvaluationCriterion
//{
//    public Guid Id { get; set; }
//    public Guid RfxId { get; set; }
//    public string Category { get; set; } = default!;
//    public string Name { get; set; } = default!;
//    public int WeightPercent { get; set; }
//    public Rfx? Rfx { get; set; }
//}

//public class RfxCommitteeMember
//{
//    public Guid Id { get; set; }
//    public Guid RfxId { get; set; }
//    public string FullName { get; set; } = default!;
//    public string Role { get; set; } = default!;
//    public Rfx? Rfx { get; set; }
//}

//public class RfxAttachment
//{
//    public Guid Id { get; set; }
//    public Guid RfxId { get; set; }
//    public string FileName { get; set; } = default!;
//    public string StoragePath { get; set; } = default!;
//    public Rfx? Rfx { get; set; }
//}




namespace Domain.Entities;

public enum RfxType { Rfi = 1, Rfp = 2, Rfq = 3, Eoi = 4 }
public enum Priority { Low = 1, Medium = 2, High = 3, Urgent = 4 }
public enum RfxStatus { Draft = 0, PendingApproval = 1, Published = 2, UnderEvaluation = 3, Awarded = 4, Completed = 5, Cancelled = 6 }

public class Rfx
{
    public Guid Id { get; set; }

    // NEW: friendly code like RFP-2025-003
    public string Code { get; set; } = string.Empty;

    // Basics
    public RfxType Type { get; set; }
    public string Title { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string Description { get; set; } = default!;

    // Money
    public decimal? EstimatedBudget { get; set; }
    public bool HideBudgetFromSuppliers { get; set; }

    // Dates / priority
    public DateTime PublicationDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string Currency { get; set; } = "QAR";
    public Priority Priority { get; set; }

    // Tender bond
    public bool TenderBondRequired { get; set; }
    public int? BondAmountPercent { get; set; }
    public int? BondValidityDays { get; set; }

    // Contacts
    public string ContactPerson { get; set; } = default!;
    public string ContactEmail { get; set; } = default!;
    public string ContactPhone { get; set; } = default!;
    public DateTime? ClarificationDeadline { get; set; }

    // Requirements
    public string? ScopeOfWork { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public string? Deliverables { get; set; }
    public string? Timeline { get; set; }

    // Evaluation
    public int MinimumQualifyingScore { get; set; }
    public string? EvaluationNotes { get; set; }

    // Required docs + terms
    public string RequiredDocumentsJson { get; set; } = "[]";
    public string? OtherRequiredDocument { get; set; }

    public bool UseStandardTerms { get; set; } = true;
    public string? CustomTerms { get; set; }
    public string? TermsFileName { get; set; }
    public string? TermsStoragePath { get; set; }

    // Publish & audience (stored for audit)
    public string PublishOption { get; set; } = "now";  // now | scheduled | draft
    public string SupplierOption { get; set; } = "all"; // all | selected
    public string SelectedSupplierIdsJson { get; set; } = "[]";

    // Status workflow
    public RfxStatus Status { get; set; } = RfxStatus.Draft;
    public DateTime? PublishedAt { get; set; }

   

    // NEW: random, opaque token used in public URLs (null when not public)
    public string? PublicToken { get; set; }

    // Children
    public List<RfxEvaluationCriterion> EvaluationCriteria { get; set; } = new();
    public List<RfxCommitteeMember> CommitteeMembers { get; set; } = new();
    public List<RfxAttachment> Attachments { get; set; } = new();
}

public class RfxEvaluationCriterion
{
    public Guid Id { get; set; }
    public Guid RfxId { get; set; }
    public string Category { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int WeightPercent { get; set; }
    public Rfx? Rfx { get; set; }
}

public class RfxCommitteeMember
{
    public Guid Id { get; set; }
    public Guid RfxId { get; set; }
    public string FullName { get; set; } = default!;
    public string Role { get; set; } = default!;
    public Rfx? Rfx { get; set; }
}

public class RfxAttachment
{
    public Guid Id { get; set; }
    public Guid RfxId { get; set; }
    public string FileName { get; set; } = default!;
    public string StoragePath { get; set; } = default!;
    public Rfx? Rfx { get; set; }
}
