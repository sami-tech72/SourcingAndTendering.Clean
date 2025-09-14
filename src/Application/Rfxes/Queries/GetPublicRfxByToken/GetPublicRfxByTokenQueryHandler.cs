using System.Text.Json;
using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Queries.GetPublicRfxByToken;

public sealed class GetPublicRfxByTokenQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetPublicRfxByTokenQuery, PublicRfxDto>
{
    public async Task<PublicRfxDto> Handle(GetPublicRfxByTokenQuery request, CancellationToken ct)
    {
        var x = await db.Rfxes
            .Include(r => r.EvaluationCriteria)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.PublicToken == request.Token, ct);

        if (x is null || x.Status != RfxStatus.Published)
            throw new KeyNotFoundException("Tender not found.");

        var dto = new PublicRfxDto
        {
            Code = x.Code,
            Title = x.Title,
            Category = x.Category,
            Department = x.Department,
            Description = x.Description,
            Type = x.Type == RfxType.Rfp ? "RFP" : x.Type == RfxType.Rfq ? "RFQ" : x.Type == RfxType.Rfi ? "RFI" : "EOI",
            PublicationDate = x.PublicationDate,
            ClosingDate = x.ClosingDate,
            Currency = x.Currency,

            // hide budget if configured
            EstimatedBudget = x.HideBudgetFromSuppliers ? null : x.EstimatedBudget,

            TenderBondRequired = x.TenderBondRequired,
            BondAmountPercent = x.BondAmountPercent,
            BondValidityDays = x.BondValidityDays,

            ContactPerson = x.ContactPerson,
            ContactEmail = x.ContactEmail,
            ContactPhone = x.ContactPhone,

            ScopeOfWork = x.ScopeOfWork,
            TechnicalSpecifications = x.TechnicalSpecifications,
            Deliverables = x.Deliverables,
            Timeline = x.Timeline,

            EvaluationCriteria = x.EvaluationCriteria
                .Select(c => new RfxCriterionDto(c.Category, c.Name, c.WeightPercent)).ToList(),
            MinimumQualifyingScore = x.MinimumQualifyingScore,

            RequiredDocuments = JsonSerializer.Deserialize<List<string>>(x.RequiredDocumentsJson) ?? new(),
            OtherRequiredDocument = x.OtherRequiredDocument,

            UseStandardTerms = x.UseStandardTerms,
            CustomTerms = x.CustomTerms,

            Attachments = x.Attachments.Select(a => new RfxAttachmentDto(a.FileName, a.StoragePath)).ToList()
        };

        return dto;
    }
}
