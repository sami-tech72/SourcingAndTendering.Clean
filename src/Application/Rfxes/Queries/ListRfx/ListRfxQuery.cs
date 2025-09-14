using Application.Rfxes.DTOs;
using MediatR;

namespace Application.Rfxes.Queries.GetRfxList;

public sealed record GetRfxListQuery() : IRequest<List<RfxListItemVm>>;
