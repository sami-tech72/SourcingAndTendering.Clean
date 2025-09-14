using System.Text.Json;
using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Queries.GetRfxById;

public sealed class GetRfxByIdQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetRfxByIdQuery, RfxUpsertDto>
{
    public async Task<RfxUpsertDto> Handle(GetRfxByIdQuery request, CancellationToken ct)
    {
        var x = await db.Rfxes
            .Include(r => r.EvaluationCriteria)
            .Include(r => r.CommitteeMembers)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (x is null) throw new KeyNotFoundException($"Rfx '{request.Id}' not found.");

        var dto = new RfxUpsertDto
        {
            Type = x.Type,
            Title = x.Title,
            Category = x.Category,
            Department = x.Department,
            Description = x.Description,
            EstimatedBudget = x.EstimatedBudget,
            HideBudgetFromSuppliers = x.HideBudgetFromSuppliers,
            PublicationDate = x.PublicationDate,
            ClosingDate = x.ClosingDate,
            Currency = x.Currency,
            Priority = x.Priority,

            TenderBondRequired = x.TenderBondRequired,
            BondAmountPercent = x.BondAmountPercent,
            BondValidityDays = x.BondValidityDays,

            ContactPerson = x.ContactPerson,
            ContactEmail = x.ContactEmail,
            ContactPhone = x.ContactPhone,
            ClarificationDeadline = x.ClarificationDeadline,

            ScopeOfWork = x.ScopeOfWork,
            TechnicalSpecifications = x.TechnicalSpecifications,
            Deliverables = x.Deliverables,
            Timeline = x.Timeline,

            MinimumQualifyingScore = x.MinimumQualifyingScore,
            EvaluationNotes = x.EvaluationNotes,

            RequiredDocuments = JsonSerializer.Deserialize<List<string>>(x.RequiredDocumentsJson) ?? new(),
            OtherRequiredDocument = x.OtherRequiredDocument,
            UseStandardTerms = x.UseStandardTerms,
            CustomTerms = x.CustomTerms,
            TermsAttachment = string.IsNullOrWhiteSpace(x.TermsFileName) ? null
                : new RfxAttachmentDto(x.TermsFileName!, x.TermsStoragePath ?? ""),

            EvaluationCriteria = x.EvaluationCriteria
                .Select(c => new RfxCriterionDto(c.Category, c.Name, c.WeightPercent)).ToList(),
            CommitteeMembers = x.CommitteeMembers
                .Select(m => new RfxMemberDto(m.FullName, m.Role)).ToList(),
            Attachments = x.Attachments
                .Select(a => new RfxAttachmentDto(a.FileName, a.StoragePath)).ToList(),

            PublishOption = x.PublishOption,
            SupplierOption = x.SupplierOption,
            SelectedSupplierIds = JsonSerializer.Deserialize<List<string>>(x.SelectedSupplierIdsJson) ?? new()
        };

        return dto;
    }
}
