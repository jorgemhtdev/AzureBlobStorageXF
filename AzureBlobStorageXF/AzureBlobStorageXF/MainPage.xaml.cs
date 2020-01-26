namespace AzureBlobStorageXF
{
    using AzureBlobStorageXF.Services;
    using Microsoft.Azure.Storage.Blob;
    using Plugin.Media;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Stream stream;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)  return;

                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });

                await BlobStorageService.UploadBlob(stream, "jpg");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void BtnPhoto_Clicked(object sender, EventArgs e) { }

        private async void BtnDownload_Clicked(object sender, EventArgs e) { }

        private async void BtnDelete_Clicked(object sender, EventArgs e) { }
    }
}
