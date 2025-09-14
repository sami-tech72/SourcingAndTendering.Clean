using Application.Rfxes.DTOs;
using MediatR;

namespace Application.Rfxes.Commands.CreateRfx;

// NEW: include PublicToken so UI can build the link
public sealed record CreateRfxCommand(RfxUpsertDto Dto) : IRequest<CreateRfxResult>;
public sealed record CreateRfxResult(Guid Id, string Code, string? PublicToken);
