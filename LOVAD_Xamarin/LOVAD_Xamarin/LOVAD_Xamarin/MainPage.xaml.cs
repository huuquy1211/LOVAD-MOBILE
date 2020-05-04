using LOVAD_Xamarin.Model;
using LOVAD_Xamarin.Utility;
using LOVAD_Xamarin.View;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOVAD_Xamarin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {

        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }
        UserModel userProfileModel = new UserModel();
        private bool AcceptBack;
        public static Action BackPressed;
        public MainPage(UserModel user)
        {
            InitializeComponent();
            if (user == null)
                Navigation.PushAsync(new LoginPage());
            else
            {
                BindingContext = this;
                NavigationPage.SetHasNavigationBar(this, false);
                userProfileModel = user;
                UserName = "Xin chào " + userProfileModel.UserName;
                CheckUserLogin();
            }
        }

       
        //Không bấn nút trở về
        protected override bool OnBackButtonPressed()
        {
            if (AcceptBack)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return base.OnBackButtonPressed();
            }
            Device.BeginInvokeOnMainThread(() => { PromptForExit(); });
            return true;
        }

        private async void PromptForExit()
        {
            if (await DisplayAlert("", "Bạn có muốn thoát khỏi ứng dụng?", "Thoát", "Quay lại"))
            {
                AcceptBack = true;
                BackPressed();
            }

        }
        public async void CheckUserLogin()
        {
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetRole?UserName=" + userProfileModel.UserName;
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();
                var responseData = Int32.Parse(result);

                //var message = responseData.Content;
                if (responseData == 1)
                {
                    grdAccount.IsVisible = false;
                    grdService.IsVisible = true;
                    grdPlace.IsVisible = false;
                    grdUserAccount.IsVisible = false;
                }
                else if (responseData == 2)
                {
                    grdUserAccount.IsVisible = true;
                    grdAccount.IsVisible = true;
                    grdService.IsVisible = true;
                    grdPlace.IsVisible = true;
                }
                else if (responseData == 3)
                {
                    grdUserAccount.IsVisible = true;
                    grdService.IsVisible = true;
                }
                else if (responseData == 4)
                {
                    grdUserAccount.IsVisible = true;
                }
                else if (responseData == 5)
                {
                    await DisplayAlert("Thông báo", "Không tìm thấy tài khoản!", "OK");
                }
                else if (responseData == 6)
                {
                    await DisplayAlert("Thông báo", "Tài khoản này không được quyền thao tác trên ứng dụng!", "OK");
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }

        private async void TaiKhoanClick(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await PopupNavigation.Instance.PopAsync();
            var Err = "Không truy cập vào chức năng này!";
            DependencyService.Get<IMessage>().LongTime(Err);
            return;
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await Navigation.PushAsync(new AccountPage(userProfileModel));
            await PopupNavigation.Instance.PopAsync();
        }

        private async void PlaceClick(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await PopupNavigation.Instance.PopAsync();
            var Err = "Không truy cập vào chức năng này!";
            DependencyService.Get<IMessage>().LongTime(Err);
            return;
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await Navigation.PushAsync(new PlacePage(userProfileModel));
            await PopupNavigation.Instance.PopAsync();
        }

        private async void LParkingClick(object sender, EventArgs e)
        {
            bool IsMasterDetail = false;
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await Navigation.PushAsync(new PlaceLParkingPage(userProfileModel, IsMasterDetail));
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void TramCanClick(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await PopupNavigation.Instance.PopAsync();
            var Err = "Không truy cập vào chức năng này!";
            DependencyService.Get<IMessage>().LongTime(Err);
            return;
            bool IsMasterDetail = false;
            await Navigation.PushAsync(new PlaceTramCanPage(userProfileModel, IsMasterDetail));
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnAccount_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await PopupNavigation.Instance.PopAsync();
            var Err = "Không truy cập vào chức năng này!";
            DependencyService.Get<IMessage>().LongTime(Err);
            return;
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetUserInf";

            UserModel user = new UserModel();
            user.UserName = userProfileModel.UserName;

            try
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var fileName = Path.Combine(documents, "cookie.txt");
                var cookie = File.ReadAllText(fileName);

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(user);

                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                content.Headers.Add("Cookie", cookie);
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<RespondModel>(result);

                var message = responseData.Content;
                if (responseData.Result == true)
                {
                    await Navigation.PushAsync(new EditUserProfilePage(responseData.UserRespond));
                }
                else
                {
                    DependencyService.Get<IMessage>().LongTime(message[0].ToString());
                }
            }
            catch (Exception ex)
            {
                //var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
            await PopupNavigation.Instance.PopAsync();
        }


    }


}
