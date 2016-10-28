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

namespace UTriSS
{
    
    public class DialogChoosePlayer : DialogFragment, View.IOnClickListener
    {

        Button userPlay;
        Button droidPlay;
        public event EventHandler<DialogEventArgs> DialogClosed;
        DialogEventArgs dialogResult = new DialogEventArgs();

        //Allow to store a value after the dialog fragment dismiss. Value that determine which player plays first.
        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);

            DialogClosed?.Invoke(this, dialogResult);
        }


        //Create the dialog fragment with two buttons to let user choose to start first or not.
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
           
            var view = inflater.Inflate(Resource.Layout.DialogPlayer, container, false);

            userPlay = view.FindViewById<Button>(Resource.Id.btnUserStarts);
            droidPlay = view.FindViewById<Button>(Resource.Id.btnAndroidStarts);

            userPlay.SetOnClickListener(this);
            droidPlay.SetOnClickListener(this);

            return view;
        }

        // Control the click event of the two buttons on the dialog fragment, changing the player from
        // user to android or viceversa
        public void OnClick(View v)
        {
            Button b = (Button)v;

            if (b == userPlay)
            {
                dialogResult.ReturnPlayer = Player.User;
            }
            else if (b == droidPlay)
            {
                dialogResult.ReturnPlayer = Player.Android;
            }

            this.Dismiss();
        }


    }

    public class DialogEventArgs
    {
        public Player ReturnPlayer { get; set; }
    }
}