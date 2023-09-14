// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Original file: https://github.com/IdentityServer/IdentityServer4.Quickstart.UI
// Modified by Jan Škoruba

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Account
{
    public class LoggedOutViewModel
    {
        public bool AutomaticRedirectAfterSignOut { get; set; } = false;
        public string ClientName { get; set; }
        public string ExternalAuthenticationScheme { get; set; }
        public string LogoutId { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string SignOutIframeUrl { get; set; }
        public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
    }
}