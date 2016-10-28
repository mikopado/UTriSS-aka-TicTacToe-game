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
    public class GameActivity : Activity, View.IOnClickListener
    {

        private Button[] Board;
        public Game play;
        public static int userCount;
        public static int droidCount;
        public static int tiesCount;
        public static int totalCount;

        // Method that handles the button click event but basically controls the entire game
        public void OnClick(View v)
        {
            Button btn = (Button)v;

            if (play.WhichPlayer == Player.User)
            {

                btn.Text = play.UserMark;
                SwapMarks(btn);

                if (play.EndOfGame() == false)
                {
                    play.GiveTurn();

                    play.MoveAndroid();


                }

            }

            if (play.EndOfGame())
            {
                DisplayScore();
                TrackPoints();
            }
            if (play.WhichPlayer == Player.Android)
            {
                if (play.EndOfGame() == false)
                {
                    btn.Text = play.DroidMark;
                    SwapMarks(btn);
                    play.GiveTurn();
                }

            }
            
            play.DisableButton(btn);

        }

        // Display score launching an alert dialog with two options: one to keep playing
        // the other to go back to main page
        private void DisplayScore()
        {
            if (play.SetScore() == 1)
            {
                var userWin = new AlertDialog.Builder(this);
                userWin.SetMessage("You Win!");

                userWin.SetPositiveButton("Keep Playing", (s, e) =>
                {

                    var intent = new Intent(this, typeof(GameActivity));
                    StartActivity(intent);

                });

                userWin.SetNegativeButton("Go Back to Main Page.", (s, e) =>
                {

                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);

                });

                userWin.Create().Show();

            }
            else if (play.SetScore() == 2)
            {

                var droidWin = new AlertDialog.Builder(this);
                droidWin.SetMessage("Android Win!");

                droidWin.SetPositiveButton("Keep Playing", (s, e) =>
                {

                    var intent = new Intent(this, typeof(GameActivity));
                    StartActivity(intent);

                });

                droidWin.SetNegativeButton("Go Back to Main Page.", (s, e) =>
                {

                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);

                });

                droidWin.Create().Show();
            }
            else
            {
                var noWin = new AlertDialog.Builder(this);
                noWin.SetMessage("Tie!");

                noWin.SetPositiveButton("Keep Playing", (s, e) =>
                {

                    var intent = new Intent(this, typeof(GameActivity));
                    StartActivity(intent);

                });

                noWin.SetNegativeButton("Go Back to Main Page.", (s, e) =>
                {

                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);

                });

                noWin.Create().Show();
            }
        }

        // Keep tracking every score (user win, android win, tie) to update the historic score 
        private void TrackPoints()
        {
            if (play.SetScore() == 1)
            {

                userCount++;
                
            }

            else if (play.SetScore() == 2)
            {

                droidCount++;

            }
            else
            {
                tiesCount++;
            }

            totalCount++;
        }


        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GameLayout);

            Button A1 = FindViewById<Button>(Resource.Id.btnA1);
            Button A2 = FindViewById<Button>(Resource.Id.btnA2);
            Button A3 = FindViewById<Button>(Resource.Id.btnA3);
            Button A4 = FindViewById<Button>(Resource.Id.btnA4);
            Button A5 = FindViewById<Button>(Resource.Id.btnA5);
            Button A6 = FindViewById<Button>(Resource.Id.btnA6);
            Button A7 = FindViewById<Button>(Resource.Id.btnA7);
            Button A8 = FindViewById<Button>(Resource.Id.btnA8);
            Button A9 = FindViewById<Button>(Resource.Id.btnA9);

            Board = new Button[] { A1, A2, A3, A4, A5, A6, A7, A8, A9 };
                        

            foreach (var item in Board)
            {

                item.SetOnClickListener(this);

            }

            Button scoreView = FindViewById<Button>(Resource.Id.btnHistPoints);


            scoreView.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(TrackingScoreActivity));
                StartActivity(intent);

            };

            Button newGame = FindViewById<Button>(Resource.Id.btnNewGame);

            newGame.Click += (object sender, EventArgs e) =>
            {
                ClearScore();
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            ShowDialogFragment();           
        

        }

        // Show the dialog to let user choice if wants to play first or let android play first.
        private void ShowDialogFragment()
        {

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DialogChoosePlayer choosePlayer = new DialogChoosePlayer();
            choosePlayer.Show(transaction, "DialogPlayer");

            choosePlayer.DialogClosed += (object sender, DialogEventArgs e) =>
            {

                play = new Game(Board, e.ReturnPlayer);

                play.StartOfGame();
            };


        }

        // Clear score when user decide to start new game
        private void ClearScore()
        {
            userCount = 0;
            droidCount = 0;
            tiesCount = 0;
            totalCount = 0;
        }

        // Method that changes the button background to images of X or O.
        private void SwapMarks(Button b)
        {
            if (b.Text.Equals("X"))
            {
                b.SetBackgroundResource(Resource.Drawable.Ximage);
            }
            else if (b.Text.Equals("O"))
            {
                b.SetBackgroundResource(Resource.Drawable.Oimage);
            }
        }
    }
}