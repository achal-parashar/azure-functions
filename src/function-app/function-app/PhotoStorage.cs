using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using Microsoft.WindowsAzure.Storage.Blob;
using Photos.Models;
using Azure.Storage.Blobs;
using Photos.AnalyzerService.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace function_app
{
    public  class PhotoStorage
    {
        //private readonly IAnalyzerService analyzerService;

        //public PhotoStorage(IAnalyzerService analyzerService)
        //{
        //    this.analyzerService = analyzerService;
        //}
        //[FunctionName("PhotoStorage")]
        //public  async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PhotoStorage")] HttpRequest req,
        //    [Blob("photos",FileAccess.Write,Connection =Literals.StorageConnectionString)] BlobContainerClient blobContainerClient,
        //    ILogger logger,
        //    [CosmosDB("photos","metadata",ConnectionStringSetting = Literals.CosmosDbConnectionString,CreateIfNotExists =true)] IAsyncCollector<dynamic> items)
        //{
        //    var body = await new StreamReader(req.Body).ReadToEndAsync();
        //    var request = JsonConvert.DeserializeObject<PhotoUploadModel>(body);
        //    var newId = Guid.NewGuid();
        //    var blobName = $"{newId}.jpg";
        //    var bytes = Convert.FromBase64String(request.Photo);
        //    var contents = new MemoryStream(bytes);

        //    BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
        //    await blobClient.UploadAsync(contents);
        //   var analysisResult = await analyzerService.AnalyzeAsync(bytes);
        //    var item = new
        //    {
        //        id = newId,
        //        name = request.Name,
        //        description = request.Description,
        //        tags = request.Tags
        //    };
        //    await items.AddAsync(item);
        //    return new OkObjectResult(newId);
        //}
        [FunctionName("PhotoStorage")]
        public async Task<byte[]> Run(
            [ActivityTrigger] PhotoUploadModel req,
            [Blob("photos", FileAccess.Write, Connection = Literals.StorageConnectionString)] BlobContainerClient blobContainerClient,
            ILogger logger,
            [CosmosDB("photos", "metadata", ConnectionStringSetting = Literals.CosmosDbConnectionString, CreateIfNotExists = true)] IAsyncCollector<dynamic> items)
        {
            var newId = Guid.NewGuid();
            var blobName = $"{newId}.jpg";
            
            var bytes = Convert.FromBase64String(req.Photo);
            var contents = new MemoryStream(bytes);

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(contents);
            var item = new
            {
                id = newId,
                name = req.Name,
                description = req.Description,
                tags = req.Tags
            };
            await items.AddAsync(item);
            return bytes;
        }
    }
}
