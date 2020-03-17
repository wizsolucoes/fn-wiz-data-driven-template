using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Wiz.Template.Auth
{
    public interface IAccessTokenProvider
    {
        Task<AccessTokenResult> ValidateToken(HttpRequest request);
    }
}
