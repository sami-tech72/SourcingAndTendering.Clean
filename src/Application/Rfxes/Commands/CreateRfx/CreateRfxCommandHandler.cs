using System.Text.Json;
using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Commands.CreateRfx;

public sealed class CreateRfxCommandHandler(IApplicationDbContext db)
    : IRequestHandler<CreateRfxCommand, CreateRfxResult>
{
    public async Task<CreateRfxResult> Handle(CreateRfxCommand request, CancellationToken ct)
    {
        var d = request.Dto;

        var entity = new Rfx
        {
            Id = Guid.NewGuid(),
            Type = d.Type,
            Title = d.Title,
            Category = d.Category,
            Department = d.Department,
            Description = d.Description,
            EstimatedBudget = d.EstimatedBudget,
            HideBudgetFromSuppliers = d.HideBudgetFromSuppliers,
            PublicationDate = d.PublicationDate,
            ClosingDate = d.ClosingDate,
            Currency = d.Currency,
            Priority = d.Priority,

            TenderBondRequired = d.TenderBondRequired,
            BondAmountPercent = d.TenderBondRequired ? d.BondAmountPercent : null,
            BondValidityDays = d.TenderBondRequired ? d.BondValidityDays : null,

            ContactPerson = d.ContactPerson,
            ContactEmail = d.ContactEmail,
            ContactPhone = d.ContactPhone,
            ClarificationDeadline = d.ClarificationDeadline,

            ScopeOfWork = d.ScopeOfWork,
            TechnicalSpecifications = d.TechnicalSpecifications,
            Deliverables = d.Deliverables,
            Timeline = d.Timeline,

            MinimumQualifyingScore = d.MinimumQualifyingScore,
            EvaluationNotes = d.EvaluationNotes,

            RequiredDocumentsJson = JsonSerializer.Serialize(d.RequiredDocuments ?? new()),
            OtherRequiredDocument = d.OtherRequiredDocument,

            UseStandardTerms = d.UseStandardTerms,
            CustomTerms = d.UseStandardTerms ? null : d.CustomTerms,
            TermsFileName = d.UseStandardTerms ? null : d.TermsAttachment?.FileName,
            TermsStoragePath = d.UseStandardTerms ? null : d.TermsAttachment?.StoragePath,

            PublishOption = d.PublishOption,
            SupplierOption = d.SupplierOption,
            SelectedSupplierIdsJson = JsonSerializer.Serialize(d.SelectedSupplierIds ?? new())
        };

        // Status from publish option
        entity.Status = d.PublishOption switch
        {
            "now" => RfxStatus.Published,
            "scheduled" => RfxStatus.PendingApproval,
            _ => RfxStatus.Draft
        };
        entity.PublishedAt = entity.Status == RfxStatus.Published ? DateTime.UtcNow : null;

        // Children
        entity.EvaluationCriteria = (d.EvaluationCriteria ?? new()).Select(c => new RfxEvaluationCriterion
        {
            Id = Guid.NewGuid(),
            RfxId = entity.Id,
            Category = c.Category,
            Name = c.Name,
            WeightPercent = c.WeightPercent
        }).ToList();

        entity.CommitteeMembers = (d.CommitteeMembers ?? new()).Select(m => new RfxCommitteeMember
        {
            Id = Guid.NewGuid(),
            RfxId = entity.Id,
            FullName = m.FullName,
            Role = m.Role
        }).ToList();

        entity.Attachments = (d.Attachments ?? new()).Select(a => new RfxAttachment
        {
            Id = Guid.NewGuid(),
            RfxId = entity.Id,
            FileName = a.FileName,
            StoragePath = a.StoragePath
        }).ToList();

        await db.Rfxes.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);

        // Assign readable code
        var year = DateTime.UtcNow.Year;
        var prefix = entity.Type switch { RfxType.Rfi => "RFI", RfxType.Rfq => "RFQ", RfxType.Rfp => "RFP", _ => "EOI" };
        var seq = await db.Rfxes.CountAsync(x => x.PublicationDate.Year == year, ct);
        entity.Code = $"{prefix}-{year}-{seq:D3}";

        // If published, generate a public token
        if (entity.Status == RfxStatus.Published)
            entity.PublicToken = GenerateToken(16);

        await db.SaveChangesAsync(ct);

        return new CreateRfxResult(entity.Id, entity.Code, entity.PublicToken);
    }
    private static string GenerateToken(int len)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var bytes = new byte[len];
        rng.GetBytes(bytes);
        return new string(bytes.Select(b => alphabet[b % alphabet.Length]).ToArray());
    }
}
