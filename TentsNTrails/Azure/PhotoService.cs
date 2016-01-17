using Microsoft.Owin.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TentsNTrails.Models;

namespace TentsNTrails.Azure
{
    public interface IPhotoService
    {
        void CreateAndConfigureAsync();
        Task<string> UploadPhotoAsync(HttpPostedFileBase photoToUpload);
    }

    public class PhotoService : IPhotoService
    {
        public const string IMAGE_URL_BASE = "https://tentsntrails.blob.core.windows.net/images/";

        ILogger log = null;

        
        public PhotoService()
        {
            log = new Logger();
        }

        public PhotoService(ILogger logger)
        {
            log = logger;
        }

        async public void CreateAndConfigureAsync()
        {
            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Create the "images" container if it doesn't already exist.
                if (await container.CreateIfNotExistsAsync())
                {
                    // Enable public access on the newly created "images" container
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });

                    log.Information("Successfully created Blob Storage Images Container and made it public");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Failure to Create or Configure images container in Blob Storage Service");
                throw;
            }
        }


        // Create a unique name for the images we are about to upload
        public string GetUniquePhotoName(string userName, string fileName)
        {
            string imageName = String.Format("{0}-photo-{1}{2}",
                userName,
                Guid.NewGuid().ToString(),
                Path.GetExtension(fileName));
            return imageName;
        }

        //upload a profile picture for a user of the given user.
        async public Task<string> UploadProfilePictureAsync(HttpPostedFileBase photoToUpload, User user)
        {
            // remove old image from blob storage
            if (user.ProfilePictureUrl.Contains(PhotoService.IMAGE_URL_BASE))
            {
                bool deleteResult = await DeletePhotoAsync(user.ProfilePictureUrl);
                System.Diagnostics.Debug.WriteLine(String.Format("Delete old image: '{0}' successful: {1}", user.ProfilePictureUrl, deleteResult));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Old image not deleted; matches '{0}'", Image.DEFAULT_PROFILE_PICTURE_URL));
            }

            string imageName = String.Format("profile-picture-{0}{1}{2}",
                user.UserName, 
                Guid.NewGuid().ToString(), 
                Path.GetExtension(photoToUpload.FileName));
            return await UploadPhotoAsync(photoToUpload, imageName);
        }

        // upload a generic photo, not specifying what user.
        async public Task<string> UploadPhotoAsync(HttpPostedFileBase photoToUpload)
        {
            string imageName = GetUniquePhotoName("user", photoToUpload.FileName );
            return await UploadPhotoAsync(photoToUpload, imageName);
        }

        // get the filename from a full image url from our website.
        public string GetFileNameFromUrl(String imageUrl)
        {
            string[] parts = imageUrl.Split(new string[] { PhotoService.IMAGE_URL_BASE }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 0 ? null : parts[0];
        }

        // TODO look at this example: http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#delete-blobs
        
        async public Task<bool> DeletePhotoAsync(string imageUrl)
        {
            Boolean success = false;

            if (imageUrl == null || imageUrl.Length == 0)
            {
                return success;
            }

            string fullPath = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(GetFileNameFromUrl(imageUrl));
                await blockBlob.DeleteAsync();
                success = true;
                timespan.Stop();
                log.TraceApi("Blob Service", "PhotoService.UploadPhoto", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                string s = "Error deleting photo blob from storage with url: " + imageUrl;
                System.Diagnostics.Debug.WriteLine(s);
                log.Error(ex, s);
                throw;
            }

            return success;
        }
        

        async public Task<string> UploadPhotoAsync(HttpPostedFileBase photoToUpload, string imageName)
        {
            if (photoToUpload == null || photoToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = photoToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(photoToUpload.InputStream);

                fullPath = blockBlob.Uri.ToString();

                timespan.Stop();
                log.TraceApi("Blob Service", "PhotoService.UploadPhoto", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error upload photo blob to storage");
                throw;
            }

            return fullPath;
        }
    }
}

