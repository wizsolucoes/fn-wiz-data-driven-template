using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Wiz.Template.Auth
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private readonly string _audience;
        private readonly string _issuer;

        public AccessTokenProvider(string audience, string issuer)
        {
            _audience = audience;
            _issuer = issuer;

            var documentRetriever = new HttpDocumentRetriever();
            documentRetriever.RequireHttps = issuer.StartsWith("https://");

            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{issuer}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                documentRetriever
            );
        }

        public async Task<AccessTokenResult> ValidateToken(HttpRequest request)
        {
            try
            {
                if (request != null &&
                    request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                    request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
                {
                    var token = request.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);
                    var openIdConfig = await _configurationManager.GetConfigurationAsync(CancellationToken.None);

                    var tokenParams = new TokenValidationParameters()
                    {
                        RequireSignedTokens = true,
                        ValidAudience = _audience,
                        ValidateAudience = true,
                        ValidIssuer = _issuer,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKeys = openIdConfig.SigningKeys
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var result = handler.ValidateToken(token, tokenParams, out var securityToken);
                    return AccessTokenResult.Success(result);
                }
                else
                {
                    return AccessTokenResult.NoToken();
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return AccessTokenResult.Expired();
            }
            catch (Exception ex)
            {
                return AccessTokenResult.Error(ex);
            }
        }
    }
}
