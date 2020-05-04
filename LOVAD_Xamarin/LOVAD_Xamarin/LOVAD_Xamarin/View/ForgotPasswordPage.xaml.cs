using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


    public partial class ForgotPasswordPage : ContentPage, INotifyPropertyChanged
    {

        #region [ResetPassword]
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }

        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged("Password"); } }

        private string _passwordConfirm;
        public string PasswordConfirm { get { return _passwordConfirm; } set { _passwordConfirm = value; OnPropertyChanged("PasswordConfirm"); } }

        private string _answer;
        public string Answer { get { return _answer; } set { _answer = value; OnPropertyChanged("Answer"); } }


        private string _type;
        public string Type { get { return _type; } set { _type = value; OnPropertyChanged("Type"); } }

        #endregion

        RegExr regExr = new RegExr();
        public ForgotPasswordPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public void StartRunLoading()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            grdForgotPassword.IsVisible = false;
            frmLoading.IsEnabled = true;
            frmLoading.IsPlaying = true;
            frmLoading.IsVisible = true;

        }

        public void StopRunLoading()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            grdForgotPassword.IsVisible = true;
            frmLoading.IsEnabled = false;
            frmLoading.IsPlaying = false;
            frmLoading.IsVisible = false;
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    base.OnBackButtonPressed();
        //    //PopupNavigation.Instance.PopAllAsync(true);
        //    return true;
        //}
        private void pkrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = pkrType.SelectedIndex;
            switch (index)
            {
                case 0:
                    {
                        grdGroupQuesAns.IsVisible = false;
                        imgType.Source = "email.png";
                    }
                    break;
                case 1:
                    {
                        var message = "Chức năng chưa được sử dụng!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        return;
                        grdGroupQuesAns.IsVisible = true;
                        imgType.Source = "titlequestion.png";
                    }
                    break;
            }
        }

        private async void btnResetPassword_Clicked(object sender, EventArgs e)
        {
            if (pkrType.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn loại đặt lại mật khẩu!", "OK");
            }

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordConfirm) && txtPasswordConfirm.TextColor == Color.FromHex("#00FF00") && pkrType.SelectedItem != null)
            {
                StartRunLoading();
                tblUserName.IsVisible = false;
                tblPassword.IsVisible = false;
                tblErrPasswordConfirm.IsVisible = false;

                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/ResetPasswordByEmail";
                try
                {
                    InputModel user = new InputModel();
                    user.UserName = UserName;
                    user.Password = Password;

                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);


                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(user);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    //var responseData = JsonConvert.DeserializeObject<RespondModel>(result);

                    int responseData = int.Parse(result);


                    if (responseData == 0)
                    {
                        await DisplayAlert("Thông báo", "Đặt lại mật khẩu thành công, vui lòng vào email xác nhận để đăng nhập!", "Ok");
                        StopRunLoading();
                        await Navigation.PopToRootAsync(true);
                    }
                    else if (responseData == 1)
                    {
                        var message = "Lỗi khi đọc dữ liệu từ requestbody!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        StopRunLoading();
                    }
                    else if (responseData == 2)
                    {
                        var message = "Không tìm đc tài khoản!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        StopRunLoading();
                    }
                    else if (responseData == 3)
                    {
                        var message = "Chưa có email!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        StopRunLoading();
                    }
                    else if (responseData == 4)
                    {
                        var message = "Chưa xác thực mail!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        StopRunLoading();
                    }
                    else if (responseData == 5)
                    {
                        var message = "Lỗi khi gửi mail!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        StopRunLoading();
                    }
                    else
                    {
                        var message = "Đặt lại mật khẩu thất bại!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        StopRunLoading();
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                    StopRunLoading();
                }
            }
            else
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập đầy đủ thông tin!", "OK");
            }

        }

        private void txtPasswordConfirm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                tblPasswordConfirm.Text = "Vui lòng không để trống!";
                tblPasswordConfirm.IsVisible = true;
            }
            else
                tblPasswordConfirm.IsVisible = false;


            if (txtPassword.Text == txtPasswordConfirm.Text && !string.IsNullOrEmpty(PasswordConfirm))
            {
                txtPasswordConfirm.TextColor = Color.FromHex("#00FF00");
                tblErrPasswordConfirm.IsVisible = true;
                tblErrPasswordConfirm.Text = "Trùng khớp mật khẩu!";
                tblErrPasswordConfirm.TextColor = Color.FromHex("#00FF00");
            }
            else
            {
                txtPasswordConfirm.TextColor = Color.Red;
                tblErrPasswordConfirm.IsVisible = true;
                tblErrPasswordConfirm.Text = "Không trùng khớp mật khẩu!";
                tblErrPasswordConfirm.TextColor = Color.Red;
            }

        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.PasswordReg(Password))
            {
                tblPassword.Text = "Mật khẩu ít nhất 8 kí tự, 1 kí tự đặt biệt, 1 chữ cái viết hoa, 1 chữ thường, 1 số (VD: !Abcde12, ...)";
                tblPassword.IsVisible = true;
            }
            else
                tblPassword.IsVisible = false;

            if (txtPassword.Text == txtPasswordConfirm.Text)
            {
                txtPasswordConfirm.TextColor = Color.FromHex("#00FF00");
                tblErrPasswordConfirm.IsVisible = true;
                tblErrPasswordConfirm.Text = "Trùng khớp mật khẩu!";
                tblErrPasswordConfirm.TextColor = Color.FromHex("#00FF00");
            }
            else
            {
                txtPasswordConfirm.TextColor = Color.Red;
                tblErrPasswordConfirm.IsVisible = true;
                tblErrPasswordConfirm.Text = "Không trùng khớp mật khẩu!";
                tblErrPasswordConfirm.TextColor = Color.Red;
            }
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.UsernameReg(UserName))
            {
                tblUserName.Text = "Không đúng định dạng, không để trống, từ 5 - 20 kí tự! (VD: abc_1.23, ...)";
                tblUserName.IsVisible = true;
            }
            else
                tblUserName.IsVisible = false;
        }

        private void txtAnswer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Answer))
            {
                tblAnswer.IsVisible = true;
            }
            else
                tblAnswer.IsVisible = false;
        }

        //private void btnQuestion_Clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.Instance.PushAsync(new QuestionView());
        //}

    }
}