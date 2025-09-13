using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Queries;

public record GetRfxByIdQuery(Guid Id) : IRequest<RfxUpsertDto?>;

public class GetRfxByIdHandler(IApplicationDbContext db) : IRequestHandler<GetRfxByIdQuery, RfxUpsertDto?>
{
    public async Task<RfxUpsertDto?> Handle(GetRfxByIdQuery request, CancellationToken ct)
    {
        var x = await db.Rfxes
            .Include(r => r.EvaluationCriteria)
            .Include(r => r.CommitteeMembers)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (x is null) return null;

        return new RfxUpsertDto
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
            EvaluationCriteria = x.EvaluationCriteria
                .Select(c => new RfxCriterionDto(c.Category, c.Name, c.WeightPercent)).ToList(),
            CommitteeMembers = x.CommitteeMembers
                .Select(m => new RfxMemberDto(m.FullName, m.Role)).ToList(),
            Attachments = x.Attachments
                .Select(a => new RfxAttachmentDto(a.FileName, a.StoragePath)).ToList()
        };
    }
}
