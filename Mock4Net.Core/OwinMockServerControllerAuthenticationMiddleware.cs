using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Mock4Net.Core.Http
{
    public class OwinMockServerControllerAuthenticationMiddleware : OwinMiddleware
    {
        private readonly string _apiKey;

        public OwinMockServerControllerAuthenticationMiddleware(OwinMiddleware next, string apiKey) : base(next)
        {
            _apiKey = apiKey;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;

            var keyHeader = request.Headers["X-MockServerControllerApiKey"];

            if (!String.IsNullOrWhiteSpace(keyHeader))
            {

                if (keyHeader.Equals(_apiKey))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, "ApiUser")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    request.User = new ClaimsPrincipal(identity);

                }

            }

            await Next.Invoke(context);
        }

    }
}
