using FluentValidation;
using TranslationService.Generated;

namespace TranslationService.Grpc.Validators;

public class TranslateRequestValidator : AbstractValidator<TranslateRequest>
{
    public TranslateRequestValidator()
    {
        RuleFor(request => request.Text).NotNull().NotEmpty();
        RuleFor(request => request.FromLang).NotNull().NotEmpty();
        RuleFor(request => request.FromLang).NotNull().NotEmpty();
    }
}