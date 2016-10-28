using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace UTriSS
{
    [Activity(Label = "UTriSS", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/CustomActionBarTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource            
            SetContentView(Resource.Layout.Main);

            Button playButton = FindViewById<Button>(Resource.Id.playButton);

            // Launch the game
            playButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(GameActivity));
                StartActivity(intent);

            };


        }       

    }
}
