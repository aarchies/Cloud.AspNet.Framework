﻿using Microsoft.AspNetCore.Http.Extensions;
using Skoruba.AuditLogging.Events;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Configuration.AuditLogging
{
    public class ApiAuditAction : IAuditAction
    {
        public ApiAuditAction(IHttpContextAccessor accessor)
        {
            Action = new
            {
                accessor.HttpContext.TraceIdentifier,
                RequestUrl = accessor.HttpContext.Request.GetDisplayUrl(),
                HttpMethod = accessor.HttpContext.Request.Method
            };
        }

        public object Action { get; set; }
    }
}