using NLog;
using PublishSubscribe.Auth;
using System.Reflection;

namespace PublishSubscribe.Middlewares
{
    public class AuthenticationMiddleware : IMiddleware
    {
        private readonly IAuthContext _authContext;

        public AuthenticationMiddleware(IAuthContext authContext)
        {
            _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Token"];
            bool authorized= _authContext.Authenticate(token);
            if (authorized)
            {
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}

