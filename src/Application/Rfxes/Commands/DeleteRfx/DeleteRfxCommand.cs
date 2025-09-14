using MediatR;
namespace Application.Rfxes.Commands.DeleteRfx;
public sealed record DeleteRfxCommand(Guid Id) : IRequest;