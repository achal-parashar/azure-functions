using function_app;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using Photos.AnalyzerService;
using Photos.AnalyzerService.Abstractions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace function_app
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAnalyzerService, ComputerVisionAnalyzerService>();
        }
    }
}
