using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Blink
{
    [Activity(Label = "Blink", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button startButton = FindViewById<Button>(Resource.Id.start_button);

            startButton.Click += StartButton_Click;
        }

        private void StartButton_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(ImageActivity));
            StartActivity(intent);
        }
    }
}