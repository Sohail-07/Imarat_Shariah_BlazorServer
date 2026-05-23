using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Imarat_Shariah.Services
{
    public class RevalidatingIdentityAuthenticationStateProvider<TUser>
        : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public RevalidatingIdentityAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = optionsAccessor.Value;
        }

        // Set state check frequency (production scaling ke liye 20 mins standard hai)
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(20);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                var user = await userManager.GetUserAsync(authenticationState.User);
                if (user == null)
                {
                    return false;
                }
                else if (!userManager.SupportsUserSecurityStamp)
                {
                    return true;
                }
                else
                {
                    var principalStamp = authenticationState.User.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
                    var userStamp = await userManager.GetSecurityStampAsync(user);
                    return principalStamp == userStamp;
                }
            }
            finally
            {
                if (scope is IAsyncDisposable disposable)
                {
                    await disposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }
    }
}