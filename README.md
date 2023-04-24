# Function Documentation: LambdaTriggerByS3Change

## Overview
This function is written in C# and designed to be deployed as an AWS Lambda function. The function listens to changes in an S3 bucket and updates an OpenSearch instance based on the contents of the files in the bucket.

## Dependencies
This function requires the following dependencies:
- `Nest` - ElasticSearch client library for .NET
- `System`
- `Amazon.S3` - AWS SDK for S3
- `System.IO`
- `System.Linq`
- `Newtonsoft.Json`
- `System.Text.Json` - JSON serializer for .NET
- `Elasticsearch.Net`
- `LambdaTriggerByS3Change.Models`

## Usage
The function can be triggered by S3 bucket events, such as object created or deleted events. The function receives an S3Event object containing information about the event, including the S3 bucket name and key of the object that triggered the event.

When a new object is created or updated in the bucket, the function reads the contents of the object and deserializes it into a `FatherClass` object. The contents of this object are then serialized into a JSON string using `System.Text.Json.JsonSerializer` and updated in an OpenSearch instance using the `UpdateOpenSearch` function.

When an object is deleted, the function logs a message indicating that the file has been deleted.

## Function Structure
The function is structured as follows:

1. The `using` statements import the required dependencies.
2. The `LambdaTriggerByS3Change` namespace contains the `Handler` class.
3. The `Handler` class implements the `IDisposable` interface and has a constructor that initializes an `IAmazonS3` object.
4. The `Run` function is the entry point for the Lambda function and is triggered by an S3 event.
5. The `UpdateOpenSearch` function updates an OpenSearch instance with a serialized JSON string.
