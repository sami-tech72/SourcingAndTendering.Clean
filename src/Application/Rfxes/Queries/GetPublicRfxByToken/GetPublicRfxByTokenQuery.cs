using Application.Rfxes.DTOs;
using MediatR;

namespace Application.Rfxes.Queries.GetPublicRfxByToken;

public sealed record GetPublicRfxByTokenQuery(string Token) : IRequest<PublicRfxDto>;
