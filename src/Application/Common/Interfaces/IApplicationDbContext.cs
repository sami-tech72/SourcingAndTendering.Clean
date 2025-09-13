using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Rfx> Rfxes { get; }

    DbSet<RfxEvaluationCriterion> RfxEvaluationCriteria { get; }
    DbSet<RfxCommitteeMember> RfxCommitteeMembers { get; }
    DbSet<RfxAttachment> RfxAttachments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    //Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
