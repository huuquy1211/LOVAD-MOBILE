using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using LOVAD_Xamarin.Utility;
using UIKit;

namespace LOVAD_Xamarin.iOS
{
    public class CloseApplication:ICloseApplication
    {
        public void closeApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}