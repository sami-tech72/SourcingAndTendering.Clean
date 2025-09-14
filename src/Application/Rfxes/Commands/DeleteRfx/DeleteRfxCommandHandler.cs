using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Commands.DeleteRfx;

public sealed class DeleteRfxCommandHandler(IApplicationDbContext db) : IRequestHandler<DeleteRfxCommand>
{
    public async Task Handle(DeleteRfxCommand request, CancellationToken ct)
    {
        var rfx = await db.Rfxes.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (rfx is null) return;
        db.Rfxes.Remove(rfx);
        await db.SaveChangesAsync(ct);
    }
}