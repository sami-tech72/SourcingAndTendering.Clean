using System.Text.Json;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Commands.UpdateRfx;

public sealed class UpdateRfxCommandHandler(IApplicationDbContext db) : IRequestHandler<UpdateRfxCommand>
{
    public async Task Handle(UpdateRfxCommand request, CancellationToken ct)
    {
        var d = request.Dto;

        var rfx = await db.Rfxes
            .Include(x => x.EvaluationCriteria)
            .Include(x => x.CommitteeMembers)
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (rfx is null) throw new KeyNotFoundException($"Rfx '{request.Id}' not found.");

        // Scalars
        rfx.Type = d.Type;
        rfx.Title = d.Title;
        rfx.Category = d.Category;
        rfx.Department = d.Department;
        rfx.Description = d.Description;
        rfx.EstimatedBudget = d.EstimatedBudget;
        rfx.HideBudgetFromSuppliers = d.HideBudgetFromSuppliers;
        rfx.PublicationDate = d.PublicationDate;
        rfx.ClosingDate = d.ClosingDate;
        rfx.Currency = d.Currency;
        rfx.Priority = d.Priority;

        rfx.TenderBondRequired = d.TenderBondRequired;
        rfx.BondAmountPercent = d.TenderBondRequired ? d.BondAmountPercent : null;
        rfx.BondValidityDays = d.TenderBondRequired ? d.BondValidityDays : null;

        rfx.ContactPerson = d.ContactPerson;
        rfx.ContactEmail = d.ContactEmail;
        rfx.ContactPhone = d.ContactPhone;
        rfx.ClarificationDeadline = d.ClarificationDeadline;

        rfx.ScopeOfWork = d.ScopeOfWork;
        rfx.TechnicalSpecifications = d.TechnicalSpecifications;
        rfx.Deliverables = d.Deliverables;
        rfx.Timeline = d.Timeline;

        rfx.MinimumQualifyingScore = d.MinimumQualifyingScore;
        rfx.EvaluationNotes = d.EvaluationNotes;

        rfx.RequiredDocumentsJson = JsonSerializer.Serialize(d.RequiredDocuments ?? new());
        rfx.OtherRequiredDocument = d.OtherRequiredDocument;

        rfx.UseStandardTerms = d.UseStandardTerms;
        rfx.CustomTerms = d.UseStandardTerms ? null : d.CustomTerms;
        rfx.TermsFileName = d.UseStandardTerms ? null : d.TermsAttachment?.FileName;
        rfx.TermsStoragePath = d.UseStandardTerms ? null : d.TermsAttachment?.StoragePath;

        rfx.PublishOption = d.PublishOption;
        rfx.SupplierOption = d.SupplierOption;
        rfx.SelectedSupplierIdsJson = JsonSerializer.Serialize(d.SelectedSupplierIds ?? new());

        // Status transition based on publish option (simple example)
        var newStatus = d.PublishOption switch
        {
            "now" => RfxStatus.Published,
            "scheduled" => RfxStatus.PendingApproval,
            _ => RfxStatus.Draft
        };
        if (rfx.Status != newStatus)
        {
            rfx.Status = newStatus;
            rfx.PublishedAt = newStatus == RfxStatus.Published ? DateTime.UtcNow : null;

            // NEW: manage token
            if (newStatus == RfxStatus.Published && string.IsNullOrWhiteSpace(rfx.PublicToken))
                rfx.PublicToken = GenerateToken(16);
            if (newStatus != RfxStatus.Published)
                rfx.PublicToken = null; // unpublish = revoke link
        }

        // Replace children
        db.RfxEvaluationCriteria.RemoveRange(rfx.EvaluationCriteria);
        db.RfxCommitteeMembers.RemoveRange(rfx.CommitteeMembers);
        db.RfxAttachments.RemoveRange(rfx.Attachments);

        rfx.EvaluationCriteria = (d.EvaluationCriteria ?? new()).Select(c => new RfxEvaluationCriterion
        {
            Id = Guid.NewGuid(),
            RfxId = rfx.Id,
            Category = c.Category,
            Name = c.Name,
            WeightPercent = c.WeightPercent
        }).ToList();

        rfx.CommitteeMembers = (d.CommitteeMembers ?? new()).Select(m => new RfxCommitteeMember
        {
            Id = Guid.NewGuid(),
            RfxId = rfx.Id,
            FullName = m.FullName,
            Role = m.Role
        }).ToList();

        rfx.Attachments = (d.Attachments ?? new()).Select(a => new RfxAttachment
        {
            Id = Guid.NewGuid(),
            RfxId = rfx.Id,
            FileName = a.FileName,
            StoragePath = a.StoragePath
        }).ToList();

        await db.SaveChangesAsync(ct);
    }
    static string GenerateToken(int len)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var bytes = new byte[len];
        rng.GetBytes(bytes);
        return new string(bytes.Select(b => alphabet[b % alphabet.Length]).ToArray());
    }
}
