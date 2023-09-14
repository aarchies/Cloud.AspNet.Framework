// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Original file: https://github.com/IdentityServer/IdentityServer4.Quickstart.UI
// Modified by Jan Škoruba

using Skoruba.IdentityServer4.Shared.Configuration.Configuration.Identity;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Account
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public LoginResolutionPolicy LoginResolutionPolicy { get; set; } = LoginResolutionPolicy.Username;
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));
    }
}