using Nest;
using System;
using Amazon.S3;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using Elasticsearch.Net;
using LambdaTriggerByS3Change.Models;
using System.Threading.Tasks;
using Amazon.Lambda.S3Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LambdaTriggerByS3Change
{
	public sealed class Handler : IDisposable
	{
		private readonly IAmazonS3 amazonS3;
		private readonly IElasticClient elasticClient;
		public Handler(IAmazonS3 amazonS3)
		{
			this.amazonS3 = amazonS3;
			//this.elasticClient = elasticClient;
		}

		public void Dispose() { }

		public async Task Run(S3Event s3Event)
		{
            var s3Object = s3Event.Records?.FirstOrDefault().S3;
			Console.WriteLine($"RECORD: {JsonSerializer.Serialize(s3Event)}");
            try
            {
                if (s3Object == null)
                {
                    return;
                }

                using (var client = new AmazonS3Client())
                {

                    if (s3Event.Records?.FirstOrDefault().EventName == "ObjectRemoved:Delete")
                    {
                        Console.WriteLine($"FILE DELETED");
                        // UPDATE OPENSEARCH
                        // await UpdateOpenSearch(fatherClass, openSearchJson);

                    }
                    else
                    {
                        var file = await client.GetObjectAsync(s3Object.Bucket.Name, s3Object.Object.Key);
                        using var reader = new StreamReader(file.ResponseStream);
                        var fileContents = await reader.ReadToEndAsync();

                        var fatherClass = JsonConvert.DeserializeObject<FatherClass>(fileContents);
                        var openSearchJson = JsonSerializer.Serialize(fatherClass, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                        Console.WriteLine($"OpenSearch Object: {JsonSerializer.Serialize(openSearchJson)}");

                        // UPDATE OPENSEARCH
                        // await UpdateOpenSearch(fatherClass, openSearchJson);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}, \nNEW IMAGE DATA: {JsonSerializer.Serialize(s3Object)}");
                throw;
            }
        }

        public async Task UpdateOpenSearch(FatherClass fatherClass, string openSearchJson)
        {
            await this.elasticClient.LowLevel.IndexAsync<BytesResponse>(
                "index-name",
                fatherClass.Id,
                PostData.String(openSearchJson)
            );
        }
    }
}