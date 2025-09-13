using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Commands;

public record CreateRfxCommand(RfxUpsertDto Rfx) : IRequest<Guid>;

public class CreateRfxValidator : AbstractValidator<CreateRfxCommand>
{
    public CreateRfxValidator()
    {
        RuleFor(x => x.Rfx.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Rfx.Description).NotEmpty();
        RuleFor(x => x.Rfx.ClosingDate).GreaterThan(x => x.Rfx.PublicationDate);
        RuleFor(x => x.Rfx.MinimumQualifyingScore).InclusiveBetween(0, 100);
    }
}

public class CreateRfxHandler(IApplicationDbContext db) : IRequestHandler<CreateRfxCommand, Guid>
{
    public async Task<Guid> Handle(CreateRfxCommand request, CancellationToken ct)
    {
        var dto = request.Rfx;
        var entity = new Rfx
        {
            Type = dto.Type,
            Title = dto.Title,
            Category = dto.Category,
            Department = dto.Department,
            Description = dto.Description,
            EstimatedBudget = dto.EstimatedBudget,
            HideBudgetFromSuppliers = dto.HideBudgetFromSuppliers,
            PublicationDate = dto.PublicationDate,
            ClosingDate = dto.ClosingDate,
            Currency = dto.Currency,
            Priority = dto.Priority,
            TenderBondRequired = dto.TenderBondRequired,
            ContactPerson = dto.ContactPerson,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            ClarificationDeadline = dto.ClarificationDeadline,
            ScopeOfWork = dto.ScopeOfWork,
            TechnicalSpecifications = dto.TechnicalSpecifications,
            Deliverables = dto.Deliverables,
            Timeline = dto.Timeline,
            MinimumQualifyingScore = dto.MinimumQualifyingScore,
            EvaluationNotes = dto.EvaluationNotes,
            EvaluationCriteria = dto.EvaluationCriteria.Select(c => new RfxEvaluationCriterion { Category = c.Category, Name = c.Name, WeightPercent = c.WeightPercent }).ToList(),
            CommitteeMembers = dto.CommitteeMembers.Select(m => new RfxCommitteeMember { FullName = m.FullName, Role = m.Role }).ToList(),
            Attachments = dto.Attachments.Select(a => new RfxAttachment { FileName = a.FileName, StoragePath = a.StoragePath }).ToList()
        };

        db.Rfxes.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }
}
