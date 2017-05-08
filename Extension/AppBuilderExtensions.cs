using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackingSystem.Middleware;

namespace TrackingSystem.Extension
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseHttpsEnforcement(this IApplicationBuilder app)
        {
            if(app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<EnforceHttpsMiddleware>();
        }

        /// <summary>
        /// Adds a HTTP Strict Transport Security header
        /// to the response.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/></param>
        /// <param name="options"></param>
        /// <returns>The <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseHsts(this IApplicationBuilder app, HstsOptions options)
        {
            return app.UseMiddleware<HstsMiddleware>(options);
        }
    }
}
