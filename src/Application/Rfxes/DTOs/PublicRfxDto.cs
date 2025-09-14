using Domain.Entities;

namespace Application.Rfxes.DTOs;

public sealed class PublicRfxDto
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Type { get; set; } = default!; // RFP/RFQ/RFI/EOI
    public DateTime PublicationDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string Currency { get; set; } = "QAR";

    public decimal? EstimatedBudget { get; set; } // null if hidden
    public bool TenderBondRequired { get; set; }
    public int? BondAmountPercent { get; set; }
    public int? BondValidityDays { get; set; }

    public string ContactPerson { get; set; } = default!;
    public string ContactEmail { get; set; } = default!;
    public string ContactPhone { get; set; } = default!;

    public string? ScopeOfWork { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public string? Deliverables { get; set; }
    public string? Timeline { get; set; }

    public List<RfxCriterionDto> EvaluationCriteria { get; set; } = new();
    public int MinimumQualifyingScore { get; set; }

    public List<string> RequiredDocuments { get; set; } = new();
    public string? OtherRequiredDocument { get; set; }

    public bool UseStandardTerms { get; set; }
    public string? CustomTerms { get; set; }

    public List<RfxAttachmentDto> Attachments { get; set; } = new();
}
