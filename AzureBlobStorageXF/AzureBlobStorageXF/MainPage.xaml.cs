namespace AzureBlobStorageXF
{
    using AzureBlobStorageXF.Services;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Xamarin.Forms;

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string fileName;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                });

                if (file == null)  return;

                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });

                fileName = await BlobStorageService.UploadBlob(file, ".jpg");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void BtnPhoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                MediaFile photo = await PhotoService.Instance.TakePhotoAsync();

                if (photo != null)
                {
                    imgChoosed.Source = ImageSource.FromStream(() =>

                    {
                        var imageStram = photo.GetStream();
                        return imageStram;
                    });

                    fileName = await BlobStorageService.UploadBlob(photo, ".jpg");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        private async void BtnDownload_Clicked(object sender, EventArgs e) 
        {
            bool result = await BlobStorageService.DownloadBlob(fileName);
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e) 
        {
            bool result = await BlobStorageService.DeleteBlob(fileName);
        }
    }
}
