using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Reflection;
using System.Threading.Tasks;

namespace Blink
{
    [Activity(Label = "ImageActivity")]
    public class ImageActivity : Activity
    {
        List<Bitmap> images;
        int imageCount = 0;
        int index = 0;
        ImageView iv;
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

            iv = FindViewById<ImageView>(Resource.Id.image);
            iv.SetImageBitmap(images[index]);

            showImageButton = FindViewById<Button>(Resource.Id.show_button);
            nextImageButton = FindViewById<Button>(Resource.Id.next_button);

            nextImageButton.Visibility = ViewStates.Gone;
            iv.Visibility = ViewStates.Invisible;

            showImageButton.Click += ShowImage;
            nextImageButton.Click += NextImage;
        }

        private void ShowImage(object sender, EventArgs e)
        {
            showImageButton.Visibility = ViewStates.Gone;
            FlashImage();
            nextImageButton.Visibility = ViewStates.Visible;
        }

        private void FlashImage()
        {
            iv.Visibility = ViewStates.Visible;
            HideImageWithDelay(50);
        }

        async void HideImageWithDelay(int milliseconds)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            iv.Visibility = ViewStates.Invisible;
        }

        private void NextImage(object sender, EventArgs e)
        {
            if (index < imageCount-1)
            {
                index++;
                iv.SetImageBitmap(images[index]);
                showImageButton.Visibility = ViewStates.Visible;
                nextImageButton.Visibility = ViewStates.Gone;
            }
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