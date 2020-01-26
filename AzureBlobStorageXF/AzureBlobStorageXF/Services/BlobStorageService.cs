namespace AzureBlobStorageXF.Services
{
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using System;
    using System.IO;
    using System.Threading.Tasks;


    public static class BlobStorageService
    {
        readonly static CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AppSettings.StorageAccountConnectionString);

        readonly static CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

        readonly static CloudBlobContainer blobContainer = blobClient.GetContainerReference(AppSettings.ContainerName);

        public static async Task<string> UploadBlob(Stream file, string extension)
        {
            try
            {

                await blobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                // await blobContainer.DeleteAsync();

                if (blobContainer != null)
                {
                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var blob = blobContainer.GetBlockBlobReference(fileName);
                    await blob.UploadFromStreamAsync(file);

                    return blob.Uri.AbsoluteUri;
                }
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
    }
}
