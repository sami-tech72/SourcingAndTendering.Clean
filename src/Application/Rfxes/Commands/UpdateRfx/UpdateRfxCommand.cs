using Application.Rfxes.DTOs;
using MediatR;

namespace Application.Rfxes.Commands.UpdateRfx;

public sealed record UpdateRfxCommand(Guid Id, RfxUpsertDto Dto) : IRequest;
