using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LOVAD_Xamarin.Droid;
using LOVAD_Xamarin.Model;
using Xamarin.Forms;
using Color = Android.Graphics.Color;

[assembly: Dependency(typeof(Message_Droid))]
namespace LOVAD_Xamarin.Droid
{
    public class Message_Droid : IMessage
    {
        void IMessage.LongTime(string message)
        {
            var toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long);
            toast.SetGravity(GravityFlags.Bottom, 0, 300);

            Color c = Color.Red;
            ColorMatrixColorFilter CM = new ColorMatrixColorFilter(new float[]
                {
                    0,0,0,0,c.R,
                    0,0,0,0,c.G,
                    0,0,0,0,c.B,
                    0,0,0,1,0
                });
            toast.View.Background.SetColorFilter(CM);
            toast.Show();
        }
        void IMessage.ShortTime(string message)
        {
            var toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
            toast.SetGravity(GravityFlags.Bottom, 0, 300);
            //Color c = Color.Argb(100,0,100,0);  
            Color c = Color.Rgb(0,255,0);
            ColorMatrixColorFilter CM = new ColorMatrixColorFilter(new float[]
                {
                    0,0,0,0,c.R,
                    0,0,0,0,c.G,
                    0,0,0,0,c.B,
                    0,0,0,1,0
                });
            toast.View.Background.SetColorFilter(CM);
            toast.Show();
        }
    }
}