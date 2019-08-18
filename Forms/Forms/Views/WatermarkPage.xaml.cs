using Forms.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SkiaSharp;
using SQLite;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WatermarkPage : ContentPage
	{
        string dbPath;
        SQLiteConnection db;

        public WatermarkPage ()
		{
			InitializeComponent ();
            image.Source = "circle";
            //image.Source = ImageSource.FromResource("Forms.Images.circle.png", typeof(WatermarkPage));
            //FetchMarkedImage();
        }

        async void TakePhoto_Clicked(object sender, System.EventArgs e)
        {
            if (cameraImageNameEntry.Text == null || watermarkTextEntry.Text == null)
            {
                return;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = cameraImageNameEntry.Text + ".jpg"
            });
            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            //setting image just after clicking using it's path(location)
            //image.Source = ImageSource.FromStream(() =>
            //{
            //    Stream stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});

            byte[] markedImageByteArray = File.ReadAllBytes(TextWatermark(file.Path));
            image.Source = ImageSource.FromStream(() => new MemoryStream(markedImageByteArray));

            InsertImageInDatabase(file.Path);
        }

        private void InsertImageInDatabase(string filePath)
        {
            if (!File.Exists(dbPath))
            {
                dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Marked_Images"); //Call Database 
                db = new SQLiteConnection(dbPath);
                //var data = db.Table<ItemsDB>(); //Call Table  
                db.CreateTable<WatermarkDB>();
            }

            //GetWatermarkTableObj(filePath);
            db.Insert(GetWatermarkTableObj(filePath));
        }

        private WatermarkDB GetWatermarkTableObj(string filePath)
        {
            //image at filePath to byte[]
            //byte[] clickedImageByteArray = File.ReadAllBytes(filePath);

            //setting image source from byte[]
            //image.Source = ImageSource.FromStream(() => new MemoryStream(clickedImageByteArray));

            byte[] markedImageByteArray = File.ReadAllBytes(TextWatermark(filePath));

            WatermarkDB watermark = new WatermarkDB
            {
                Name = cameraImageNameEntry.Text,
                WatermarkText = watermarkTextEntry.Text,
                MarkedImageByteArray = markedImageByteArray
            };
            System.Console.WriteLine($"path: {filePath}||------------");

            return watermark;
        }

        private void PlaceWatermark(string filePath)
        {
            SKImage finalImage;
            SKBitmap pic = SKBitmap.Decode(filePath);

            using (var tempSurface = SKSurface.Create(new SKImageInfo(pic.Width, pic.Height)))
            {
                //get the drawing canvas of the surface
                var canvas = tempSurface.Canvas;

                //set background color
                canvas.Clear(SKColors.Transparent);

                //go through each image and draw it on the final image
                int offset = 0;
                int offsetTop = 0;
                
                canvas.DrawBitmap(pic, SKRect.Create(offset, offsetTop, 1000, 1000));

                SKBitmap stamp = SKBitmap.Decode("storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/test_1.jpg");

                canvas.DrawBitmap(stamp, SKRect.Create(offset, pic.Height, 100, 100));
                ///

                // return the surface as a manageable image
                finalImage = tempSurface.Snapshot();
            }

            //save the new image
            using (SKData encoded = finalImage.Encode(SKEncodedImageFormat.Png, 100))
            using (Stream outFile = File.OpenWrite("storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/final.jpg"))
            {
                encoded.SaveTo(outFile);
                Image temp = new Image
                {
                    Source = ImageSource.FromFile("storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/final.jpg")
                };
                image.Source = ImageSource.FromFile("storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/final.jpg");
            }

        }

        private string TextWatermark(string filePath)
        {
            SKImage finalImage;
            string markedImagePath = string.Empty;

            SKBitmap pic = SKBitmap.Decode(filePath);

            using (var tempSurface = SKSurface.Create(new SKImageInfo(pic.Width, pic.Height)))
            {
                //get the drawing canvas of the surface
                var canvas = tempSurface.Canvas;

                //set background color
                canvas.Clear(SKColors.Transparent);

                //go through each image and draw it on the final image
                int offset = 0;
                int offsetTop = 0;

                canvas.DrawBitmap(pic, SKRect.Create(offset, offsetTop, pic.Width, pic.Height));

                
                SKPaint paint = new SKPaint
                {
                    Color = SKColors.LightGreen,
                    TextSize = 50
                };

                SKPoint offsetPoint = new SKPoint
                {
                    X = pic.Width / 20,
                    Y = pic.Height * 9.5f / 10
                };

                SKPath path = new SKPath();

                canvas.DrawTextOnPath(watermarkTextEntry.Text, path, offsetPoint, paint);
                
                //canvas.DrawBitmap(stamp, SKRect.Create(offset, pic.Height, 100, 100));

                ///

                // return the surface as a manageable image
                finalImage = tempSurface.Snapshot();
            }

            //save the new image
            using (SKData encoded = finalImage.Encode(SKEncodedImageFormat.Png, 100))
            using (Stream outFile = File.OpenWrite("storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/final.jpg"))
            {
                encoded.SaveTo(outFile);
                image.Source = ImageSource.FromFile("storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/final.jpg");
                markedImagePath = "storage/emulated/0/Android/data/Forms.Forms/files/Pictures/Sample/final.jpg";
            }
            return markedImagePath;
        }

        private void FetchMarkedImage()
        {
            WatermarkDB watermarkDB = new WatermarkDB();

            dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Marked_Images"); //Call Database 
            db = new SQLiteConnection(dbPath);
            watermarkDB = db.Get<WatermarkDB>("one");

            image.Source = ImageSource.FromStream(() => new MemoryStream(watermarkDB.MarkedImageByteArray)); 
        }

    }
}