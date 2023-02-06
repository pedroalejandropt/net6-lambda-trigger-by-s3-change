using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.S3;
using Elasticsearch.Net;
using Nest;

namespace LambdaTriggerByS3Change;

[ExcludeFromCodeCoverage]
internal static class Program
{
	private static async Task Main()
	{
        //var openSearchEndpoint = Environment.GetEnvironmentVariable("OpenSearchEndpoint");

        //if (openSearchEndpoint is null)
        //{
        //    throw new ArgumentException("OpenSearchEndpoint must have a value.");
        //}

        //using (var singleNodeConnectionPool = new SingleNodeConnectionPool(new Uri(openSearchEndpoint)))
        //{
        //    using (var elasticConnectionSettings = new ConnectionSettings(singleNodeConnectionPool))
        //    {
                using (var amazonS3Client = new AmazonS3Client())
				{
					//var elasticClient = new ElasticClient(elasticConnectionSettings);
					//using (var handler = new Handler(amazonS3Client, elasticClient))
					using (var handler = new Handler(amazonS3Client))
                    {
						await LambdaBootstrapBuilder.Create((Func<S3Event, Task>)handler.Run, new DefaultLambdaJsonSerializer()).Build().RunAsync();
					}
				}
        //    }
        //}
	}
}
