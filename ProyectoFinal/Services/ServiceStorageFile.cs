using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProyectoFinal.Services
{
    public class ServiceStorageFile
    {
        private String bucketName;
        private IAmazonS3 awsClient;

        public ServiceStorageFile(IAmazonS3 awsClient,IConfiguration config)
        {
            this.awsClient = awsClient;
            this.bucketName = config["AWSS3:BucketName"];
        }
        public async Task UploadFile(IFormFile file, String nombre)
        {
            using (var client = new AmazonS3Client("AKIAXWIS2DQZAJJCFUSP", "7oK7I5qdwf+8atFBOSHnSFgm9Ghs2qInxbtb0oAo", RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = nombre + file.FileName,
                        BucketName = "fotosjlc",
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }
        public async Task<List<String>> GetS3Files(String nombre)
        {
            ListVersionsResponse response = await awsClient.ListVersionsAsync(this.bucketName);
            return response.Versions.Select(x => x.Key).Where(x => x.StartsWith(nombre)).ToList();
        }
        public async Task<bool> DeleteFile(String filename)
        {
            DeleteObjectResponse response = await this.awsClient.DeleteObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Stream> GetFile(String filename)
        {
            GetObjectResponse response = await this.awsClient.GetObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
            else
            {
                return null;
            }
        }
    }
}
