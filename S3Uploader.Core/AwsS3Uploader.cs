using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace S3Uploader.Core
{
    public class AwsS3Uploader
    {
        private static readonly RegionEndpoint defaultRegion = Amazon.RegionEndpoint.EUWest1;

        private const string DEFAULT_CONTENT_TYPE = "binary/octet-stream";

        public async Task<Result> AttemptUpload(string rootPath, string bucket)
        {
            if (!Directory.Exists(rootPath)) return Result.Fail($"Invalid path: {rootPath}");

            var filesToUpload = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);

            using (var client = new AmazonS3Client(defaultRegion))
            {
                foreach (var filename in filesToUpload)
                {
                    Console.WriteLine($"Uploading file {filename}");

                    var remoteName = filename
                        .Replace(rootPath, String.Empty)
                        .Replace(@"\", "/");

                    var result = await UploadFile(client, filename, remoteName, bucket);

                    if (result.IsFailure) Console.WriteLine(result.Error);
                }

                return Result.Ok();
            }
        }

        private async Task<Result> UploadFile(IAmazonS3 client, string filename, string remoteName, string bucket)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = remoteName,
                    FilePath = filename,
                    ContentType = GetContentType(filename)
                };

                var response = await client.PutObjectAsync(putRequest);

                return response.IsSuccessStatusCode() ? Result.Ok() : Result.Fail($"Invalid response for S3 upload to {remoteName}");
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                string errorMsg;

                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    errorMsg = "Check the provided AWS Credentials.";
                }
                else
                {
                    errorMsg = $"Error occurred. Message:'{amazonS3Exception.Message}' when writing an object";
                }

                return Result.Fail(errorMsg);
            }
        }

        private string GetContentType(string filename)
        {
            var mimeType = MimeMapping.GetMimeMapping(filename);

            if (string.IsNullOrWhiteSpace(mimeType)) return DEFAULT_CONTENT_TYPE;

            return mimeType;
        }
    }

    internal static class PutObjectResponseExtensions
    {
        internal static bool IsSuccessStatusCode(this PutObjectResponse response)
            => response.HttpStatusCode.Equals(HttpStatusCode.OK) || response.HttpStatusCode.Equals(HttpStatusCode.Created);
    }
}
