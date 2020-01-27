namespace AzureBlobStorageXF.Services
{
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Plugin.Media.Abstractions;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public static class BlobStorageService
    {
        readonly static CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AppSettings.StorageAccountConnectionString);

        readonly static CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

        readonly static CloudBlobContainer blobContainer = blobClient.GetContainerReference(AppSettings.ContainerName);

        public static async Task<string> UploadBlob(MediaFile file, string extension)
        {
            try
            {
                if (blobContainer != null)
                {
                    string fileName = $"{Guid.NewGuid()}{extension}";

                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    await blob.UploadFromStreamAsync(file.GetStream());

                    return fileName; //return blob.Uri.AbsoluteUri;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public static async Task<bool> DownloadBlob(string fileName)
        {
            try
            {
                if (blobContainer.ExistsAsync().Result)
                {
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

                    string localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

                    await blob.DownloadToFileAsync(localPath, FileMode.CreateNew);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }

        public static async Task<bool> DeleteBlob(string fileName)
        {
            try
            {
                if (blobContainer.ExistsAsync().Result)
                {
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    await blob.DeleteIfExistsAsync();

                    return true;
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }

    }
}
