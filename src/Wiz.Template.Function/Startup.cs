using System;
using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Wiz.Template.Function.Services;
using Wiz.Template.Function.Services.Interfaces;

[assembly: FunctionsStartup(typeof(Wiz.Template.Function.Startup))]
namespace Wiz.Template.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<IViaCEPService, ViaCEPService>((s, c) =>
            {
                c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ViaCEPUrl"));
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }
    }
}