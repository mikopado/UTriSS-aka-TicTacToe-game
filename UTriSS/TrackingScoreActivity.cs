using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace UTriSS
{
    [Activity(Label = "UTriSS", Icon = "@drawable/icon", Theme = "@style/CustomActionBarTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TrackingScoreActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HistoricScore);

            TextView userScore = FindViewById<TextView>(Resource.Id.txtUserPoints);
            TextView droidScore = FindViewById<TextView>(Resource.Id.txtDroidPoints);
            TextView tieScore = FindViewById<TextView>(Resource.Id.txtTiesPoints);
            TextView totalScore = FindViewById<TextView>(Resource.Id.txtTotalCount);

            userScore.Text = GameActivity.userCount.ToString();
            droidScore.Text = GameActivity.droidCount.ToString();
            tieScore.Text = GameActivity.tiesCount.ToString();
            totalScore.Text = GameActivity.totalCount.ToString();

        }
    }
}