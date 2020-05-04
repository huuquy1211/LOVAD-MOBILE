using LOVAD_Xamarin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LParkingCustomerPage : ContentPage
    {
        public LParkingCustomerPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void btnUnfold_Clicked(object sender, EventArgs e)
        {
            if (grdSearchInformation.IsVisible == false)
            {

                btnUnfold.ImageSource = "dow.png";
                btnUnfold.Text = "ĐÓNG THÔNG TIN";
                grdSearchInformation.IsVisible = true;

                return;

            }

            if (grdSearchInformation.IsVisible == true)
            {
                btnUnfold.ImageSource = "up.png";
                btnUnfold.Text = "MỞ THÔNG TIN";
                grdSearchInformation.IsVisible = false;

                return;
            }
        }

        #region [Nút nhấn show combobox]
        private void IsVisibleCombobox(string NameBtn)
        {
            switch (NameBtn)
            {
                case "btnDocType":
                    {
                        grdIsBusy.IsVisible = true;
                        stlSelectDataSearch.IsVisible = true;

                        grdDocType.IsVisible = true;
                        grdCardStatus.IsVisible = false;
                        grdCusType.IsVisible = false;
                        grdObjectType.IsVisible = false;
                        grdPaymentType.IsVisible = false;
                        break;
                    }
                case "btnCardStatus":
                    {
                        grdIsBusy.IsVisible = true;
                        stlSelectDataSearch.IsVisible = true;

                        grdDocType.IsVisible = false;
                        grdCardStatus.IsVisible = true;
                        grdCusType.IsVisible = false;
                        grdObjectType.IsVisible = false;
                        grdPaymentType.IsVisible = false;
                        break;
                    }
                case "btnCusType":
                    {
                        grdIsBusy.IsVisible = true;
                        stlSelectDataSearch.IsVisible = true;

                        grdDocType.IsVisible = false;
                        grdCardStatus.IsVisible = false;
                        grdCusType.IsVisible = true;
                        grdObjectType.IsVisible = false;
                        grdPaymentType.IsVisible = false;
                        break;
                    }
                case "btnPaymentType":
                    {
                        grdIsBusy.IsVisible = true;
                        stlSelectDataSearch.IsVisible = true;

                        grdDocType.IsVisible = false;
                        grdCardStatus.IsVisible = false;
                        grdCusType.IsVisible = false;
                        grdPaymentType.IsVisible = true;
                        grdObjectType.IsVisible = false;
                       
                        break;
                    }
                case "btnObjectType":
                    {
                        grdIsBusy.IsVisible = true;
                        stlSelectDataSearch.IsVisible = true;

                        grdDocType.IsVisible = false;
                        grdCardStatus.IsVisible = false;
                        grdCusType.IsVisible = false;
                        grdPaymentType.IsVisible = false;
                        grdObjectType.IsVisible = true;
                        break;
                    }
                default:
                    break;
            }
            
        }
        private void btnDocType_Clicked(object sender, EventArgs e)
        {
            IsVisibleCombobox("btnDocType");
        }

        private void btnCardStatus_Clicked(object sender, EventArgs e)
        {
            IsVisibleCombobox("btnCardStatus");

        }

        private void btnCusType_Clicked(object sender, EventArgs e)
        {
            IsVisibleCombobox("btnCusType");
        }

        private void btnPaymentType_Clicked(object sender, EventArgs e)
        {
            IsVisibleCombobox("btnPaymentType");
        }

        private void btnObjectType_Clicked(object sender, EventArgs e)
        {
            IsVisibleCombobox("btnObjectType");
        }
        #endregion

        private void ViewCell_Tapped(object sender, EventArgs e)
        {

        }

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Thông báo", "Bạn có muốn đăng xuất không?", "Đăng xuất", "Quay lại");
            if (answer == true)
            {
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/Logout";
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);

                    HttpClient client = new HttpClient();
                    StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    //var responseData = JsonConvert.DeserializeObject<RespondModel>(result);
                    bool responseData = bool.Parse(result);

                    //var message = responseData.Content;
                    if (responseData == true)
                    {
                        await Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        var message = "Lỗi server";
                        //DependencyService.Get<IMessage>().Longtime(message[0].ToString());
                        await DisplayAlert("Thông báo", message, "OK");
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            else
                return;

        }
    }
}