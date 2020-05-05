using LOVAD_Xamarin.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageView : PopupPage
    {
        public ImageView(string imageSource)
        {
            InitializeComponent();
            imgShowView.Source = ParseImageByteArray(imageSource);
        }
        public ImageSource ParseImageByteArray(string ImagePath)
        {
            if (ImagePath != null)
            {
                try
                {
                    byte[] byteArray = Convert.FromBase64String(ImagePath);
                    Stream stream = new MemoryStream(byteArray);
                    var imageSource = ImageSource.FromStream(() => stream);
                    return imageSource;
                }
                catch (Exception ex)
                {

                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return null;
                }
                finally
                {

                }
            }
            else
            {
                return "NoImage.png";
            }


        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync(true);
        }
       
        private async void imgShowView_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync(true);
        }
    }
}