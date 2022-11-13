using FluentValidation;
using PublishSubscribe.DTOs;

namespace PublishSubscribe.Validators
{
    public class PublishDTOValidator : AbstractValidator<PublishDTO>
    {
        public PublishDTOValidator()
        {
            RuleFor(x => x.Token).NotNull().NotEmpty();
            RuleFor(x => x.Topic).NotNull().NotEmpty();
            RuleFor(x => x.Message).NotNull().NotEmpty();
            RuleFor(x => x.PublisherId).GreaterThan(0);
        }
    }
}
