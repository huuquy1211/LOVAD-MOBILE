using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingView : PopupPage
    {
        public LoadingView(string Name)
        {
            InitializeComponent();
            if (Name != null)
            {
                switch (Name)
                {
                    case "lparking":
                        {
                            frmLoadLparking.IsVisible = true;
                            frmLoad.IsVisible = false;
                            break;
                        }
                    case "search":
                        {
                            frmLoadLparking.IsVisible = false;
                            frmLoad.IsVisible = true;
                            frmLoad.Animation = "load2.json";

                            break;
                        }
                    case "email":
                        {
                            frmLoadLparking.IsVisible = false;
                            frmLoad.IsVisible = true;
                            frmLoad.Animation = "sendmail2.json";
                            break;
                        }
                    case "login":
                        {
                            frmLoadLparking.IsVisible = false;
                            frmLoad.IsVisible = true;
                            frmLoad.Animation = "loader.json";
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }
}