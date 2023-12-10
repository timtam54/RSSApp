namespace RssMob.Views;

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using RssMob.Services;
using SkiaSharp;

public partial class Photo : ContentPage
{
    //https://www.freeiconspng.com/downloadimg/1445
    public int _InspEquipID;
    public iRefreshData inspEquipPhotos;
    public Photo(int InspEquipID, iRefreshData _inspEquipPhotos)
    {
        InitializeComponent();
        _InspEquipID = InspEquipID;
        inspEquipPhotos = _inspEquipPhotos;
        uploadImage = new UploadImage();
    }
    UploadImage uploadImage { get; set; }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        TakePhoto();
    }

    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            Indi.IsVisible = true;
            Indi.IsRunning = true;

            lblUpload.IsVisible = true;
            FileResult img = await MediaPicker.Default.CapturePhotoAsync();
            if (img != null)
            {

                Models.ImageFile imfl = await uploadImage.GetImageFile(img, true);

                /*  Stream stream = await img.OpenReadAsync();
                 *  SKBitmap photoBitmap =UploadImage.RotateBitmapResize(SKBitmap.Decode(stream));
                  Stream rotatedStream = SKImage.FromBitmap(photoBitmap).Encode().AsStream();
                  byte[] bytes = null;
                      using (var ms = new MemoryStream())
                      {
                          rotatedStream.CopyTo(ms);
                          bytes = ms.ToArray();
                      }
                      Models.ImageFile imfl= new Models.ImageFile
                      {
                          byteBase64 = Convert.ToBase64String(bytes),
                          ContentType = img.ContentType,
                          FileName = Guid.NewGuid().ToString() + "." + Path.GetExtension(img.FileName)
                      };*/
                Models.ImageFile imagefile = await uploadImage.UploadToServer(imfl, 1, _InspEquipID);
                Image_Upload.Source = ImageSource.FromStream(() => uploadImage.ByteArrayStream(uploadImage.StringToByteBase64(imfl.byteBase64)));// imagefile.byteBase64)));

                //              Image_Upload.Source = ImageSource.FromStream(() => uploadImage.ByteArrayStream(uploadImage.StringToByteBase64(imagefile.byteBase64)));

                await inspEquipPhotos.RefreshDataAsync();
               
                await Application.Current.MainPage.Navigation.PopAsync();
                
            }
            Indi.IsVisible = false;
            Indi.IsRunning = false;
            lblUpload.IsVisible = false;
        }
    }
    //installled sksharp just for this method
    
    private async void Save_Clicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    async void Gallery_Clicked(object sender, EventArgs e)
    {
        ActivityIndicator activityIndicator = (ActivityIndicator)Indi;
        activityIndicator. IsRunning = true;
        lblUpload.IsVisible = true;
        activityIndicator.IsVisible = true;
        FileResult img = await uploadImage.OpenMediaPickerAsync();
        if (img != null)
        {
          var  imagefile = await uploadImage.GetImageFile(img, false);

             imagefile = await uploadImage.UploadToServer(imagefile, 1, _InspEquipID);
            Image_Upload.Source = ImageSource.FromStream(() => uploadImage.ByteArrayStream(uploadImage.StringToByteBase64(imagefile.byteBase64)));
            
            
            await inspEquipPhotos.RefreshDataAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
         activityIndicator.IsRunning=false;
        activityIndicator.IsVisible = false;
        activityIndicator.IsRunning = false;
    }
}