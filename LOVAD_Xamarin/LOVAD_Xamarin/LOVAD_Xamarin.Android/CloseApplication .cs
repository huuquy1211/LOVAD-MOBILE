﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LOVAD_Xamarin.Utility;
using Xamarin.Forms;

namespace LOVAD_Xamarin.Droid
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            var activity = (Activity)Android.App.Application.Context;
            activity.FinishAffinity();
        }
    }
}