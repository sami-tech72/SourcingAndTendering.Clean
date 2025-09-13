using Application.Common.Interfaces;
using Application.Rfxes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Queries;

public record GetRfxListQuery : IRequest<List<RfxListItemDto>>;

public class GetRfxListHandler(IApplicationDbContext db) : IRequestHandler<GetRfxListQuery, List<RfxListItemDto>>
{
    public async Task<List<RfxListItemDto>> Handle(GetRfxListQuery request, CancellationToken ct)
    {
        return await db.Rfxes
            .OrderByDescending(x => x.PublicationDate)
            .Select(x => new RfxListItemDto(
                x.Id, x.Title, x.Type, x.Category, x.Department, x.ClosingDate, x.Priority.ToString()))
            .ToListAsync(ct);
    }
}
