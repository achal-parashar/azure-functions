using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Photos.AnalyzerService.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Collections.Generic;

namespace function_app
{
    public class PhotosAnalyzer
    {
        private readonly IAnalyzerService analyzerService;

        public PhotosAnalyzer(IAnalyzerService analyzerService)
        {
            this.analyzerService = analyzerService;
        }
        [FunctionName("PhotosAnalyzer")]
        public async Task<dynamic> Run([ActivityTrigger] List<byte> image)
        {
            return await analyzerService.AnalyzeAsync(image.ToArray());
        }
    }
}
