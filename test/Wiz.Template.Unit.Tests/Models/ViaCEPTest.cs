using Wiz.Template.Function.Models;
using Xunit;

namespace Wiz.Template.Unit.Tests.Models
{
    public class ViaCEPTest
    {
        [Theory]
        [InlineData("689011-11")]
        [InlineData("640104-45")]
        [InlineData("440520-61")]
        [InlineData("689068-10")]
        public void CEPFormat_Test(string cep)
        {
            var format = ViaCEP.CEPFormat(cep);

            Assert.Equal(format, cep.Replace("-", string.Empty));
        }
    }
}