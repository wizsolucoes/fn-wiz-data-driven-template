using System.Threading.Tasks;
using Wiz.Template.Function.Models;

namespace Wiz.Template.Function.Services.Interfaces
{
    public interface IViaCEPService
    {
        Task<ViaCEP> GetByCEPAsync(string cep);
    }
}