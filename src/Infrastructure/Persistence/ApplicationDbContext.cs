using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<AppUser, IdentityRole, string>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Rfx> Rfxes => Set<Rfx>();
    public DbSet<RfxEvaluationCriterion> RfxEvaluationCriteria => Set<RfxEvaluationCriterion>();
    public DbSet<RfxCommitteeMember> RfxCommitteeMembers => Set<RfxCommitteeMember>();
    public DbSet<RfxAttachment> RfxAttachments => Set<RfxAttachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity tables

        // Rfx (use the generic overload)
        modelBuilder.Entity<Rfx>(MapRfx);

        modelBuilder.Entity<RfxEvaluationCriterion>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Category).HasMaxLength(50).IsRequired();
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.WeightPercent).IsRequired();
        });

        modelBuilder.Entity<RfxCommitteeMember>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            b.Property(x => x.Role).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<RfxAttachment>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            b.Property(x => x.StoragePath).HasMaxLength(1000).IsRequired();
        });
    }

    private static void MapRfx(EntityTypeBuilder<Rfx> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).IsRequired();

        b.HasMany(x => x.EvaluationCriteria)
         .WithOne(x => x.Rfx)
         .HasForeignKey(x => x.RfxId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.CommitteeMembers)
         .WithOne(x => x.Rfx)
         .HasForeignKey(x => x.RfxId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.Attachments)
         .WithOne(x => x.Rfx)
         .HasForeignKey(x => x.RfxId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
