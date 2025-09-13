using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Commands;

public record UpdateRfxCommand(Guid Id, RfxUpsertDto Rfx) : IRequest<bool>;

public class UpdateRfxHandler(IApplicationDbContext db) : IRequestHandler<UpdateRfxCommand, bool>
{
    public async Task<bool> Handle(UpdateRfxCommand request, CancellationToken ct)
    {
        var entity = await db.Rfxes
            .Include(r => r.EvaluationCriteria)
            .Include(r => r.CommitteeMembers)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (entity is null) return false;

        var dto = request.Rfx;
        entity.Type = dto.Type;
        entity.Title = dto.Title;
        entity.Category = dto.Category;
        entity.Department = dto.Department;
        entity.Description = dto.Description;
        entity.EstimatedBudget = dto.EstimatedBudget;
        entity.HideBudgetFromSuppliers = dto.HideBudgetFromSuppliers;
        entity.PublicationDate = dto.PublicationDate;
        entity.ClosingDate = dto.ClosingDate;
        entity.Currency = dto.Currency;
        entity.Priority = dto.Priority;
        entity.TenderBondRequired = dto.TenderBondRequired;
        entity.ContactPerson = dto.ContactPerson;
        entity.ContactEmail = dto.ContactEmail;
        entity.ContactPhone = dto.ContactPhone;
        entity.ClarificationDeadline = dto.ClarificationDeadline;
        entity.ScopeOfWork = dto.ScopeOfWork;
        entity.TechnicalSpecifications = dto.TechnicalSpecifications;
        entity.Deliverables = dto.Deliverables;
        entity.Timeline = dto.Timeline;
        entity.MinimumQualifyingScore = dto.MinimumQualifyingScore;
        entity.EvaluationNotes = dto.EvaluationNotes;

        // replace children (simple approach)
        entity.EvaluationCriteria.Clear();
        entity.EvaluationCriteria.AddRange(dto.EvaluationCriteria.Select(c =>
            new Domain.Entities.RfxEvaluationCriterion { Category = c.Category, Name = c.Name, WeightPercent = c.WeightPercent }));

        entity.CommitteeMembers.Clear();
        entity.CommitteeMembers.AddRange(dto.CommitteeMembers.Select(m =>
            new Domain.Entities.RfxCommitteeMember { FullName = m.FullName, Role = m.Role }));

        entity.Attachments.Clear();
        entity.Attachments.AddRange(dto.Attachments.Select(a =>
            new Domain.Entities.RfxAttachment { FileName = a.FileName, StoragePath = a.StoragePath }));

        await db.SaveChangesAsync(ct);
        return true;
    }
}
