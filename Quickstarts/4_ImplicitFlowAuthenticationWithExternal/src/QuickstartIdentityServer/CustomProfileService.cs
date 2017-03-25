using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using IdentityServer4.Extensions;
using IdentityServer4.Test;

namespace QuickstartIdentityServer
{
    public class CustomProfileService : IProfileService
    {
        private readonly ILogger<CustomProfileService> _logger;
        private readonly TestUserStore _userStore;

        public CustomProfileService(TestUserStore userStore, ILogger<CustomProfileService> logger)
        {
            _userStore = userStore;
            _logger = logger;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            _logger.LogDebug("Get profile called for {subject} from {client} with {claimTypes} because {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName,
                context.RequestedClaimTypes,
                context.Caller);

            if (context.RequestedClaimTypes.Any())
            {
                // Add own claims for user
                var user = _userStore.FindBySubjectId(context.Subject.GetSubjectId());
                context.AddFilteredClaims(user.Claims);

                // Add additional claims (used for NT authentication test)
                context.AddFilteredClaims(context.Subject.Claims);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}