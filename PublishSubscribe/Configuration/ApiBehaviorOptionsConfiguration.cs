using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PublishSubscribe.Configuration
{
    public class ApiBehaviorOptionsConfiguration : IConfigureOptions<ApiBehaviorOptions>
    {
        public void Configure(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = (context) =>
            {
                var validationError = context.ModelState.Keys.Where(i => context.ModelState[i]?.Errors?.Count > 0).Select(k => context.ModelState[k]?.Errors?.First().ErrorMessage).FirstOrDefault();

                return new BadRequestObjectResult(validationError);
            };
        }
    }
}
