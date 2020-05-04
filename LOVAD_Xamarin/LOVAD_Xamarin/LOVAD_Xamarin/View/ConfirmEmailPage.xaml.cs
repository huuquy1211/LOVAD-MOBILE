using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
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
    public partial class ConfirmEmailPage : ContentPage
    {
        UserModel userProfileModel = new UserModel();
        public ConfirmEmailPage(UserModel user)
        {
            InitializeComponent();
            userProfileModel = user;
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
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
                    if (responseData.UserRespond.EmailConfirmed == true)
                    {
                        //PopupNavigation.Instance.PushAsync(new PlaceUserView(userSelect));
                        //DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                        await DisplayAlert("Thông báo", "Xác nhận thành công!", "Quay lại trang chủ");
                        await Navigation.PushAsync(new MainPage(responseData.UserRespond));
                    }
                    else
                    {
                        var messErr = "Xác nhận Email không thành công!";
                        DependencyService.Get<IMessage>().LongTime(messErr);
                        await Navigation.PushAsync(new MainPage(responseData.UserRespond));
                    }


                }
                else
                {
                    DependencyService.Get<IMessage>().LongTime(message[0].ToString());
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }
    }
}