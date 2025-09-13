namespace Domain.Entities;

public enum RfxType { Rfi = 1, Rfp = 2, Rfq = 3, Eoi = 4 } // adjust order to match your app
public enum Priority { Low = 1, Medium = 2, High = 3, Urgent = 4 }

//namespace Domain.Entities;

public class Rfx
{
    public Guid Id { get; set; }

    public RfxType Type { get; set; }
    public string Title { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal EstimatedBudget { get; set; }
    public bool HideBudgetFromSuppliers { get; set; }
    public DateTime PublicationDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string Currency { get; set; } = "QAR";
    public Priority Priority { get; set; }
    public bool TenderBondRequired { get; set; }
    public string ContactPerson { get; set; } = default!;
    public string ContactEmail { get; set; } = default!;
    public string ContactPhone { get; set; } = default!;
    public DateTime? ClarificationDeadline { get; set; }
    public string? ScopeOfWork { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public string? Deliverables { get; set; }
    public string? Timeline { get; set; }
    public int MinimumQualifyingScore { get; set; }
    public string? EvaluationNotes { get; set; }

    public List<RfxEvaluationCriterion> EvaluationCriteria { get; set; } = [];
    public List<RfxCommitteeMember> CommitteeMembers { get; set; } = [];
    public List<RfxAttachment> Attachments { get; set; } = [];
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

