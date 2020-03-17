using System;
using System.Security.Claims;

namespace Wiz.Template.Auth
{
    public sealed class AccessTokenResult
    {
        private AccessTokenResult() { }

        public ClaimsPrincipal Principal { get; private set; }

        public AccessTokenStatus Status { get; private set; }

        public Exception Exception { get; private set; }

        public static AccessTokenResult Success(ClaimsPrincipal principal)
        {
            return new AccessTokenResult
            {
                Principal = principal,
                Status = AccessTokenStatus.Valid
            };
        }

        public static AccessTokenResult Expired()
        {
            return new AccessTokenResult
            {
                Status = AccessTokenStatus.Expired
            };
        }

        public static AccessTokenResult Error(Exception ex)
        {
            return new AccessTokenResult
            {
                Status = AccessTokenStatus.Error,
                Exception = ex
            };
        }

        public static AccessTokenResult NoToken()
        {
            return new AccessTokenResult
            {
                Status = AccessTokenStatus.NoToken
            };
        }
    }
}
