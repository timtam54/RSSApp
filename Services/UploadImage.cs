using System;
using RssMob.Models;
using SkiaSharp;
using System.Text;
using System.Threading.Tasks;


namespace RssMob.Services
{
    //https://www.youtube.com/watch?v=ozNm46JDL78
    public class UploadImage
    {

        public async Task<FileResult> OpenMediaPickerAsync()
        {
            try {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Pick Photo" });
                if (result.ContentType == "jpeg" || result.ContentType == "image/png" || result.ContentType == "image/jpg" || result.ContentType == "image/jpeg")
                    return result;
                else 
                    await App.Current.MainPage.DisplayAlert("Error", "try again", "cancel");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }

        }

        public async Task<Stream> FileResultToStream(FileResult fileresult)
        {
            if (fileresult == null) return null;
            return await fileresult.OpenReadAsync();
        }
        public Stream ByteArrayStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }


        public string ByteBase64ToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        public byte[] StringToByteBase64(string text)
        {
            return Convert.FromBase64String(text);
        }
        /*public async Task<ImageFile> UploadToServerImageFile(ImageFile pht, int ParentTable, int ParentID)
        {
            pht.ParentID = ParentID;
            pht.ParentTable = ParentTable;
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(pht);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new System.Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = content;// Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTest>(content);
            }
            return pht;
        }*/
        public async Task<ImageFile> UploadToServer(ImageFile imagefile, int ParentTable, int ParentID)//,bool rotate)
        {
          
            //var pht = await GetImageFile(fileresult, rotate);

            imagefile.ParentID= ParentID;
            imagefile.ParentTable = ParentTable;
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(imagefile);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new System.Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = content;// Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTest>(content);
            }
            return imagefile;
        }
        string url = "https://roofsafetysolutions.azurewebsites.net/api/Image/";

        public static SKBitmap RotateBitmapResize(SKBitmap bitmap)
        {
            SKBitmap rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);
            using (var surface = new SKCanvas(rotatedBitmap))
            {
                surface.Translate(rotatedBitmap.Width, 0);
                surface.RotateDegrees(90);
                surface.DrawBitmap(bitmap, 0, 0);
            }
            return ResizeImage(rotatedBitmap);
        }

        /*
         * SKBitmap RotateBitmap(SKBitmap bitmap)
    {
        SKBitmap rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);
        using (var surface = new SKCanvas(rotatedBitmap))
        {
            surface.Translate(rotatedBitmap.Width, 0);
            surface.RotateDegrees(90);
            surface.DrawBitmap(bitmap, 0, 0);
        }
        return rotatedBitmap;
    }
         */
        /*
                Stream stream = await img.OpenReadAsync();     
                SKBitmap photoBitmap = RotateBitmap(SKBitmap.Decode(stream));
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
                    };
                Models.ImageFile imagefile = await uploadImage.UploadToServerImageFile(imfl, 1, _InspEquipID);  
         */



        public async Task<ImageFile> GetImageFile(FileResult fileresult,bool Rotate)
        {
            try
            {
                Stream stream = await fileresult.OpenReadAsync();
                Stream rotatedStream;
                SKBitmap photoBitmap = SKBitmap.Decode(stream);
                if (Rotate)
                {

                    photoBitmap = RotateBitmapResize(photoBitmap);




                }
                else
                {
                    photoBitmap = ResizeImage(photoBitmap);
                    //rotatedStream = stream;
                }
                rotatedStream = SKImage.FromBitmap(photoBitmap).Encode().AsStream();
                byte[] bytes = null;

                using (var ms = new MemoryStream())
                {
                    rotatedStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }
                return new ImageFile
                {
                    byteBase64 = Convert.ToBase64String(bytes),
                    ContentType = fileresult.ContentType,
                    FileName = Guid.NewGuid().ToString() + "." + Path.GetExtension(fileresult.FileName)
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private static SKBitmap ResizeImage(SKBitmap photoBitmap)
        {
            try
            {
                int resizedWidth = photoBitmap.Width; int resizedHeight = photoBitmap.Height;
                while (resizedWidth > 1000)
                {
                    resizedWidth = resizedWidth / 2;
                    resizedHeight = resizedHeight / 2;
                }
                if (resizedWidth != photoBitmap.Width)
                {
                    SkiaSharp.SKImageInfo info = new SKImageInfo(resizedWidth, resizedHeight);
                    SkiaSharp.SKFilterQuality qualityx = SKFilterQuality.High;
                    photoBitmap = photoBitmap.Resize(info, qualityx);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }
            return photoBitmap;
        }
    }
}
