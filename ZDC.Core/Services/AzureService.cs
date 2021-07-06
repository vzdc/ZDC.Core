using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ZDC.Core.Services
{
    public class AzureService
    {
        private readonly BlobContainerClient _container;
        private readonly string _url;

        public AzureService(IConfiguration config)
        {
            var client = new BlobServiceClient(config.GetValue<string>("AzureConnectionString"));
            _container = client.GetBlobContainerClient(config.GetValue<string>("AzureBlobContainer"));
            _url = config.GetValue<string>("AzureBlobUrl");
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                var blob = _container.GetBlobClient($"{file.GetHashCode()}-{file.FileName}");
                await blob.UploadAsync(file.OpenReadStream());
                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task DeleteFile(string url)
        {
            try
            {
                var name = url.Replace(_url, "");
                await _container.DeleteBlobAsync(name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}