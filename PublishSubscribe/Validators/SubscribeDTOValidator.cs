using FluentValidation;
using PublishSubscribe.DTOs;

namespace PublishSubscribe.Validators
{
    public class SubscribeDTOValidator : AbstractValidator<SubscribeDTO>
    {
        public SubscribeDTOValidator()
        {
            RuleFor(x => x.Token).NotNull().NotEmpty();
            RuleFor(x => x.Topic).NotNull().NotEmpty();
            RuleFor(x => x.ClientUrl).NotNull().NotEmpty();
            RuleFor(x => x.SubscriberId).GreaterThan(0);
          
        }
    }
}
