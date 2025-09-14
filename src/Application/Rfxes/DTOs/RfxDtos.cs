//using Domain.Entities;

//namespace Application.Rfxes.DTOs;

//public record RfxListItemDto(
//     Guid Id,
//    string Title,
//    RfxType Type,
//    string Category,
//    string Department,
//    DateTime ClosingDate,
//    Priority Priority,
//    RfxStatus Status
//);

//public class RfxUpsertDto
//{
//    // BASIC
//    public RfxType Type { get; set; }
//    public string Title { get; set; } = default!;
//    public string Category { get; set; } = default!;
//    public string Department { get; set; } = default!;
//    public string Description { get; set; } = default!;
//    public decimal? EstimatedBudget { get; set; }
//    public bool HideBudgetFromSuppliers { get; set; }
//    public DateTime PublicationDate { get; set; }
//    public DateTime ClosingDate { get; set; }
//    public string Currency { get; set; } = "QAR";
//    public Priority Priority { get; set; } = Priority.Medium;

//    // TENDER BOND
//    public bool TenderBondRequired { get; set; }
//    public int? BondAmountPercent { get; set; }
//    public int? BondValidityDays { get; set; }

//    // CONTACTS
//    public string ContactPerson { get; set; } = default!;
//    public string ContactEmail { get; set; } = default!;
//    public string ContactPhone { get; set; } = default!;
//    public DateTime? ClarificationDeadline { get; set; }

//    // REQUIREMENTS
//    public string? ScopeOfWork { get; set; }
//    public string? TechnicalSpecifications { get; set; }
//    public string? Deliverables { get; set; }
//    public string? Timeline { get; set; }

//    // EVALUATION
//    public int MinimumQualifyingScore { get; set; } = 70;
//    public string? EvaluationNotes { get; set; }
//    public List<RfxCriterionDto> EvaluationCriteria { get; set; } = new();
//    public List<RfxMemberDto> CommitteeMembers { get; set; } = new();

//    // REQUIRED DOCS + TERMS
//    public List<string> RequiredDocuments { get; set; } = new();
//    public string? OtherRequiredDocument { get; set; }
//    public bool UseStandardTerms { get; set; } = true;
//    public string? CustomTerms { get; set; }
//    public RfxAttachmentDto? TermsAttachment { get; set; }

//    // ATTACHMENTS
//    public List<RfxAttachmentDto> Attachments { get; set; } = new();

//    // PUBLISH (arrives from Angular)
//    public string PublishOption { get; set; } = "now";   // now | scheduled | draft
//    public string SupplierOption { get; set; } = "all";  // all | selected
//    public List<string> SelectedSupplierIds { get; set; } = new();
//}

//public record RfxCriterionDto(string Category, string Name, int WeightPercent);
//public record RfxMemberDto(string FullName, string Role);
//public record RfxAttachmentDto(string FileName, string StoragePath);
