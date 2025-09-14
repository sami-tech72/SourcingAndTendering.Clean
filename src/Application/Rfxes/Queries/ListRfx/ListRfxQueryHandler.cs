using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Queries.GetRfxList;

public sealed class GetRfxListQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetRfxListQuery, List<RfxListItemVm>>
{
    public async Task<List<RfxListItemVm>> Handle(GetRfxListQuery request, CancellationToken ct)
    {
        return await db.Rfxes
            .OrderByDescending(x => x.PublicationDate)
            .Select(x => new RfxListItemVm
            {
                Id = x.Id,
                Title = x.Title,
                Type = x.Type == RfxType.Rfp ? "RFP"
                     : x.Type == RfxType.Rfq ? "RFQ"
                     : x.Type == RfxType.Rfi ? "RFI"
                     : "EOI",
                Category = x.Category,
                PublicationDate = x.PublicationDate,
                ClosingDate = x.ClosingDate,
                Status =
                    x.Status == RfxStatus.PendingApproval ? "Pending Approval" :
                    x.Status == RfxStatus.Published ? "Published" :
                    x.Status == RfxStatus.UnderEvaluation ? "Under Evaluation" :
                    x.Status == RfxStatus.Awarded ? "Awarded" :
                    x.Status == RfxStatus.Completed ? "Completed" :
                    x.Status == RfxStatus.Cancelled ? "Cancelled" :
                    "Draft",
                Responses = 0 // TODO: replace with db.Submissions.Count(s => s.RfxId == x.Id)
            })
            .ToListAsync(ct);
    }
}
