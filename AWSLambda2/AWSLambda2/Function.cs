using System;
using System.Threading.Tasks;

using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text;
using Newtonsoft.Json;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda2
{
    public class Function
    {
        public static async Task<string> FunctionHandler(Submission input, ILambdaContext context)
        {
            var logger = context.Logger;

            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast2))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = "josh-doerring",
                        Key = $"ta-ef-file-based/{input.TaxYear}/{input.Environment}/{input.ClientType}/{input.FileName}",
                        ContentType = "application/octet-stream",
                        ContentBody = input.FileData,
                    };

                    request.Metadata["x-amz-meta-email"] = input.Email;
                    request.Metadata["x-amz-meta-product-type"] = input.ProductType;
                    request.Metadata["x-amz-meta-tracking-id"] = input.TrackingId;

                    var response = await client.PutObjectAsync(request);

                    logger.Log(response.HttpStatusCode.ToString());
                }

                return JsonConvert.SerializeObject(new { trackingId = input.TrackingId });
            }
            catch (Exception ex)
            {
                logger.Log("Exception: " + ex.Message + "\n" + ex.StackTrace);
                return ex.Message;
            }
        }

        //public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        //{
        //    var logger = context.Logger;

        //    try
        //    {
        //        logger.Log("PathParameters-Start");
        //        var taxYear = input.PathParameters["tax-year"];
        //        var clientType = input.PathParameters["client-type"];
        //        var productType = input.PathParameters["product-type"];
        //        var fileName = input.PathParameters["file-name"];
        //        logger.Log("PathParameters-End");

        //        logger.Log("QueryStringParameters-Start");
        //        var email = input.QueryStringParameters["email"];
        //        var trackingId = input.QueryStringParameters["trackingId"];
        //        logger.Log("QueryStringParameters-End");

        //        logger.Log("StageVariables-Start");
        //        var environment = input.StageVariables["environment"];
        //        logger.Log("StageVariables-End");

        //        logger.Log("InputBody-Start");
        //        var fileData = input.Body;
        //        logger.Log("InputBody-End");

        //        try
        //        {
        //            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast2))
        //            {
        //                var request = new PutObjectRequest
        //                {
        //                    BucketName = "josh-doerring",
        //                    Key = $"ta-ef-file-based/{taxYear}/{environment}/{clientType}/{fileName}",
        //                    ContentType = "application/octet-stream",
        //                    ContentBody = fileData,
        //                };

        //                request.Metadata["x-amz-meta-email"] = email;
        //                request.Metadata["x-amz-meta-product-type"] = productType;
        //                request.Metadata["x-amz-meta-tracking-id"] = trackingId;

        //                var response = await client.PutObjectAsync(request);
        //            }

        //            return new APIGatewayProxyResponse
        //            {
        //                Body = JsonConvert.SerializeObject(new { trackingId = trackingId, isbase64 = input.IsBase64Encoded }),
        //                StatusCode = 200
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Log("Exception in PutS3Object: " + ex.Message + "\n" + ex.StackTrace);
        //            return new APIGatewayProxyResponse
        //            {
        //                Body = ex.Message,
        //                StatusCode = 500
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Log("UnhandledException: " + ex.Message + "\n" + ex.StackTrace);
        //        return new APIGatewayProxyResponse
        //        {
        //            Body = ex.Message,
        //            StatusCode = 500
        //        };
        //    }
        //}

        //public static async Task<string> FunctionHandler(Stream input, ILambdaContext context)
        //{
        //    var logger = context.Logger;

        //    try
        //    {
        //        using (var stream = new MemoryStream())
        //        {
        //            logger.Log("ReadStream-Begin");
        //            byte[] buffer = new byte[2048]; // read in chunks of 2KB
        //            int bytesRead;
        //            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
        //            {
        //                stream.Write(buffer, 0, bytesRead);
        //            }
        //            byte[] result = stream.ToArray();

        //            logger.Log("ReadStream-End");

        //            logger.Log("Convert-Start");
        //            logger.Log(Encoding.UTF8.GetString(result));
        //            logger.Log("Convert-End");

        //            return await Task.FromResult($"Stream Size: {result.Length}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Log("UnhandledException: " + ex.Message + "\n" + ex.StackTrace);
        //        return ex.Message;
        //    }
        //}
    }
}
