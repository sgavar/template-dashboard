using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingSystem.Middleware
{
    public class EnforceHttpsMiddleware
    {
        /**
         * 1. Checks if the request is done over HTTPS
         * 2. If not, makes a permanent redirect (301) to the secure version of the URL
         * 3. If yes, just allows the request to continue
         * */
        private readonly RequestDelegate _next;

        public EnforceHttpsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequest req = context.Request;
            if (req.IsHttps == false)
            {
                string url = "https://" + req.Host + req.Path + req.QueryString;
                context.Response.Redirect(url, permanent: true);
            }
            else
            {
                await _next(context);
            }
        }

    }
}
