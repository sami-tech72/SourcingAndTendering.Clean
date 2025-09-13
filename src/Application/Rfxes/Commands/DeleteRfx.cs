using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rfxes.Commands;

public record DeleteRfxCommand(Guid Id) : IRequest<bool>;

public class DeleteRfxHandler(IApplicationDbContext db) : IRequestHandler<DeleteRfxCommand, bool>
{
    public async Task<bool> Handle(DeleteRfxCommand request, CancellationToken ct)
    {
        var entity = await db.Rfxes.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (entity is null) return false;

        db.Rfxes.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
