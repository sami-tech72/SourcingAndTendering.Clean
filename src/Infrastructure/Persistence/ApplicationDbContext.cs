//using Application.Common.Interfaces;
//using Domain.Entities;
//using Infrastructure.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Infrastructure.Persistence;

//public class ApplicationDbContext
//    : IdentityDbContext<AppUser, IdentityRole, string>, IApplicationDbContext
//{
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//    public DbSet<Rfx> Rfxes => Set<Rfx>();
//    public DbSet<RfxEvaluationCriterion> RfxEvaluationCriteria => Set<RfxEvaluationCriterion>();
//    public DbSet<RfxCommitteeMember> RfxCommitteeMembers => Set<RfxCommitteeMember>();
//    public DbSet<RfxAttachment> RfxAttachments => Set<RfxAttachment>();

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        modelBuilder.Entity<Rfx>(MapRfx);

//        modelBuilder.Entity<RfxEvaluationCriterion>(b =>
//        {
//            b.HasKey(x => x.Id);
//            b.Property(x => x.Category).HasMaxLength(50).IsRequired();
//            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
//            b.Property(x => x.WeightPercent).IsRequired();
//        });

//        modelBuilder.Entity<RfxCommitteeMember>(b =>
//        {
//            b.HasKey(x => x.Id);
//            b.Property(x => x.FullName).HasMaxLength(200).IsRequired();
//            b.Property(x => x.Role).HasMaxLength(200).IsRequired();
//        });

//        modelBuilder.Entity<RfxAttachment>(b =>
//        {
//            b.HasKey(x => x.Id);
//            b.Property(x => x.FileName).HasMaxLength(255).IsRequired();
//            b.Property(x => x.StoragePath).HasMaxLength(1000).IsRequired();
//        });
//    }

//    private static void MapRfx(EntityTypeBuilder<Rfx> b)
//    {
//        b.HasKey(x => x.Id);

//        // Basics
//        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
//        b.Property(x => x.Category).HasMaxLength(150).IsRequired();
//        b.Property(x => x.Department).HasMaxLength(150).IsRequired();
//        b.Property(x => x.Description).IsRequired();

//        // Money / currency
//        b.Property(x => x.EstimatedBudget).HasColumnType("decimal(18,2)");
//        b.Property(x => x.Currency).HasMaxLength(10).IsRequired();

//        // Required docs JSON
//        b.Property(x => x.RequiredDocumentsJson).HasColumnType("nvarchar(max)").HasDefaultValue("[]").IsRequired();
//        b.Property(x => x.OtherRequiredDocument).HasMaxLength(500);

//        // Terms
//        b.Property(x => x.CustomTerms);
//        b.Property(x => x.TermsFileName).HasMaxLength(255);
//        b.Property(x => x.TermsStoragePath).HasMaxLength(1000);

//        // Publish fields
//        b.Property(x => x.PublishOption).HasMaxLength(20).HasDefaultValue("now").IsRequired();
//        b.Property(x => x.SupplierOption).HasMaxLength(20).HasDefaultValue("all").IsRequired();
//        b.Property(x => x.SelectedSupplierIdsJson).HasColumnType("nvarchar(max)").HasDefaultValue("[]").IsRequired();

//        // Status
//        b.Property(x => x.Status)
//         .HasConversion<int>()
//         .HasDefaultValue(RfxStatus.Draft)  // <-- enum value, not (int)...
//         .IsRequired();


//        // Indexes
//        b.HasIndex(x => x.ClosingDate);
//        b.HasIndex(x => new { x.Status, x.ClosingDate });
//        b.HasIndex(x => new { x.Type, x.PublicationDate });

//        // Children
//        b.HasMany(x => x.EvaluationCriteria)
//         .WithOne(x => x.Rfx)
//         .HasForeignKey(x => x.RfxId)
//         .OnDelete(DeleteBehavior.Cascade);

//        b.HasMany(x => x.CommitteeMembers)
//         .WithOne(x => x.Rfx)
//         .HasForeignKey(x => x.RfxId)
//         .OnDelete(DeleteBehavior.Cascade);

//        b.HasMany(x => x.Attachments)
//         .WithOne(x => x.Rfx)
//         .HasForeignKey(x => x.RfxId)
//         .OnDelete(DeleteBehavior.Cascade);
//    }
//}




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

    public Task<int> SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        // NEW
        b.Property(x => x.Code).HasMaxLength(40).IsRequired();
        b.HasIndex(x => x.Code).IsUnique();

        // Basics
        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Category).HasMaxLength(150).IsRequired();
        b.Property(x => x.Department).HasMaxLength(150).IsRequired();
        b.Property(x => x.Description).IsRequired();

        // Money / currency
        b.Property(x => x.EstimatedBudget).HasColumnType("decimal(18,2)");
        b.Property(x => x.Currency).HasMaxLength(10).IsRequired();

        // Docs JSON + free text
        b.Property(x => x.RequiredDocumentsJson)
            .HasColumnType("nvarchar(max)")
            .HasDefaultValue("[]")
            .IsRequired();
        b.Property(x => x.OtherRequiredDocument).HasMaxLength(500);

        // Terms metadata
        b.Property(x => x.CustomTerms); // nvarchar(max)
        b.Property(x => x.TermsFileName).HasMaxLength(255);
        b.Property(x => x.TermsStoragePath).HasMaxLength(1000);

        // Publish fields
        b.Property(x => x.PublishOption).HasMaxLength(20).HasDefaultValue("now").IsRequired();
        b.Property(x => x.SupplierOption).HasMaxLength(20).HasDefaultValue("all").IsRequired();
        b.Property(x => x.SelectedSupplierIdsJson)
            .HasColumnType("nvarchar(max)")
            .HasDefaultValue("[]")
            .IsRequired();

        //        // Status
        b.Property(x => x.Status)
            .HasConversion<int>()
            .HasDefaultValue(RfxStatus.Draft)  // <-- enum value, not (int)...
            .IsRequired();
        b.Property(x => x.PublishedAt); // nullable

        // NEW: public token (unique when not null)
        b.Property(x => x.PublicToken).HasMaxLength(40);
        b.HasIndex(x => x.PublicToken).IsUnique().HasFilter("[PublicToken] IS NOT NULL");

        // Helpful indexes
        b.HasIndex(x => x.ClosingDate);
        b.HasIndex(x => new { x.Status, x.ClosingDate });
        b.HasIndex(x => new { x.Type, x.PublicationDate });

        // Children cascade
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
