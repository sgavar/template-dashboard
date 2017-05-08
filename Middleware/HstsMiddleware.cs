﻿using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace TrackingSystem.Middleware
{
    public class HstsMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName = "Strict-Transport-Security";
        private readonly string _headerValue;

        public HstsMiddleware(RequestDelegate next, HstsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (options.Seconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Seconds), "Expiry time must be positive");
            }

            _next = next;

            string headerValue = "max-age=" + options.Seconds;
            if (options.IncludeSubDomains)
            {
                headerValue += "; includeSubDomains";
            }
            if (options.Preload)
            {
                headerValue += "; preload";
            }
            _headerValue = headerValue;
        }

        public async Task Invoke(HttpContext context)
        {
            //HSTS can only be applied to secure requests according to spec
            // there really is no point adding it to insecure ones since MiTM can just strip the header
            if (context.Request.IsHttps)
            {
                context.Response.Headers.Add(HeaderName, _headerValue);
            }
            await _next(context);
        }
    }
}