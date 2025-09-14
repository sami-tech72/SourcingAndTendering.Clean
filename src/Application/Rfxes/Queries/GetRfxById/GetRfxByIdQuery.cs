using Application.Rfxes.DTOs;
using MediatR;

namespace Application.Rfxes.Queries.GetRfxById;

public sealed record GetRfxByIdQuery(Guid Id) : IRequest<RfxUpsertDto>;
