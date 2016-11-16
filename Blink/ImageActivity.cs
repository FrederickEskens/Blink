using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Blink
{
    [Activity(Label = "ImageActivity")]
    public class ImageActivity : Activity
    {
        List<Bitmap> images;

        int imageCount = 0;
        int index = 0;

        ImageView imageView;

        Button showImageButton;
        Button nextImageButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Image);

            if(images == null)
            {
                InitializeImages();
            }           

            imageView = FindViewById<ImageView>(Resource.Id.image);
            imageView.SetImageBitmap(images[index]);

            showImageButton = FindViewById<Button>(Resource.Id.show_button);
            nextImageButton = FindViewById<Button>(Resource.Id.next_button);

            nextImageButton.Visibility = ViewStates.Gone;
            imageView.Visibility = ViewStates.Invisible;

            showImageButton.Click += ShowImage;
            nextImageButton.Click += NextImage;
        }

        private void ShowImage(object sender, EventArgs e)
        {
            showImageButton.Visibility = ViewStates.Gone;
            nextImageButton.Visibility = ViewStates.Visible;
            FlashImage();            
        }

        private void FlashImage()
        {
            imageView.Visibility = ViewStates.Visible;
            HideImageWithDelay(50);
        }

        private void NextImage(object sender, EventArgs e)
        {
            if (index < imageCount - 1)
            {
                index++;
                imageView.SetImageBitmap(images[index]);
                showImageButton.Visibility = ViewStates.Visible;
                nextImageButton.Visibility = ViewStates.Gone;
            }
        }

        async void HideImageWithDelay(int milliseconds)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            imageView.Visibility = ViewStates.Invisible;
        }       

        private Bitmap CropImage(Bitmap source)
        {
            var width = source.Width;
            var height = source.Height;
            var crop = (width - height) / 2;

            return Bitmap.CreateBitmap(source, crop, 0, height, height);
        }

        private void InitializeImages()
        {
            images = new List<Bitmap>();
            Type myType = typeof(Resource.Drawable);
            FieldInfo[] fields = myType.GetFields();
            foreach(FieldInfo f in fields)
            {
                if (!f.Name.Equals("Icon"))
                { 
                    imageCount++;
                    var bitmap = BitmapFactory.DecodeResource(Resources, (int)f.GetValue(null));
                    images.Add(CropImage(bitmap));
                }
            }
            images.Shuffle();
        }
    }
}