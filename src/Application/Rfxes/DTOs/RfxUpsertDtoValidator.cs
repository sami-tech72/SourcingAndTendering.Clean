using FluentValidation;

namespace Application.Rfxes.DTOs;

public sealed class RfxUpsertDtoValidator : AbstractValidator<RfxUpsertDto>
{
    public RfxUpsertDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Department).NotEmpty();
        RuleFor(x => x.Currency).NotEmpty().Must(c => new[] { "QAR", "USD", "EUR", "GBP" }.Contains(c));

        RuleFor(x => x.PublicationDate).NotEmpty();
        RuleFor(x => x.ClosingDate).NotEmpty();

        RuleFor(x => x).Custom((dto, ctx) =>
        {
            if (dto.ClosingDate <= dto.PublicationDate)
                ctx.AddFailure(nameof(dto.ClosingDate), "'Closing Date' must be greater than 'Publication Date'.");

            if (dto.ClarificationDeadline.HasValue && dto.ClarificationDeadline.Value >= dto.ClosingDate)
                ctx.AddFailure(nameof(dto.ClarificationDeadline), "'Clarification Deadline' must be before 'Closing Date'.");
        });

        When(x => x.TenderBondRequired, () =>
        {
            RuleFor(x => x.BondAmountPercent).NotNull().InclusiveBetween(0, 100);
            RuleFor(x => x.BondValidityDays).NotNull().GreaterThan(0);
        });

        RuleFor(x => x.MinimumQualifyingScore).InclusiveBetween(0, 100);

        RuleForEach(x => x.EvaluationCriteria).ChildRules(c =>
        {
            c.RuleFor(y => y.Category).NotEmpty();
            c.RuleFor(y => y.Name).NotEmpty().MaximumLength(200);
            c.RuleFor(y => y.WeightPercent).InclusiveBetween(0, 100);
        });

        RuleFor(x => x.EvaluationCriteria.Sum(c => c.WeightPercent))
            .Equal(100)
            .WithMessage("Evaluation criteria weights must total 100%.");

        RuleForEach(x => x.CommitteeMembers).ChildRules(m =>
        {
            m.RuleFor(y => y.FullName).NotEmpty().MaximumLength(200);
            m.RuleFor(y => y.Role).NotEmpty().MaximumLength(200);
        });

        When(x => !x.UseStandardTerms, () =>
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.CustomTerms) || x.TermsAttachment != null)
                .WithMessage("Provide custom terms text or attach a terms file when not using standard terms.");
        });
    }
}
