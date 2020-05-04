using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAccountPage : ContentPage, INotifyPropertyChanged
    {
        #region [SaveAccount]
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }

        private string _passWord;
        public string PassWord { get { return _passWord; } set { _passWord = value; OnPropertyChanged("PassWord"); } }

        private string _confirmPassword;
        public string ConfirmPassword { get { return _confirmPassword; } set { _confirmPassword = value; OnPropertyChanged("ConfirmPassword"); } }
        #endregion
        InputModel inputModel;
        RegExr regExr = new RegExr();
        UserModel userModel;
        public AddAccountPage(UserModel user)
        {
            InitializeComponent();
            BindingContext = this;
            userModel = user;
        }
        //Lưu lại
        private async void SaveAccount_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord) && !string.IsNullOrEmpty(ConfirmPassword) && txtConfirmPassword.TextColor == Color.FromHex("#00FF00") && tblUserName.IsVisible == false && tblPassWord.IsVisible == false && tblConfirmPassword.IsVisible == false)
            {
                tblUserName.IsVisible = false;
                tblPassWord.IsVisible = false;
                tblConfirmPassword.IsVisible = false;

                inputModel = new InputModel();
                inputModel.UserName = UserName;
                inputModel.Password = PassWord;

                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/Register";
               
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);

                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(inputModel);

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("cookie", cookie);
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string result = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<RespondModel>(result);
                    var message = responseData.Content;
                    if (responseData.Result == true)
                    {
                        DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                        MessagingCenter.Send(this, "Update");//Gửi thông điệp update listview về PlacePage
                        await Navigation.PopAsync();
                    }

                    else
                        DependencyService.Get<IMessage>().LongTime(message[0].ToString());
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            else
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập thông tin hợp lệ!", "OK");
            }
        }

        //Thây đổi text trên form nhập
        private void txtConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                tblConfirmPassword.Text = "Không để trống!";
                tblConfirmPassword.IsVisible = true;
            }
            else
                tblConfirmPassword.IsVisible = false;

            if (txtPassWord.Text == txtConfirmPassword.Text )
            {
                txtConfirmPassword.TextColor = Color.FromHex("#00FF00");
                tblErConfirmPassword.IsVisible = true;
                tblErConfirmPassword.Text = "Trùng khớp mật khẩu!";
                tblErConfirmPassword.TextColor = Color.FromHex("#00FF00");
            }
            else
            {
                txtConfirmPassword.TextColor = Color.Red;
                tblErConfirmPassword.IsVisible = true;
                tblErConfirmPassword.Text = "Không trùng khớp mật khẩu!";
                tblErConfirmPassword.TextColor = Color.Red;
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

        private void txtPassWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.PasswordReg(PassWord))
            {
                tblPassWord.Text = "Mật khẩu ít nhất 8 kí tự, 1 kí tự đặt biệt, 1 chữ cái viết hoa, 1 chữ thường, 1 số (VD: !Abcde12, ...)";
                tblPassWord.IsVisible = true;
            }
            else
                tblPassWord.IsVisible = false;



            if (txtPassWord.Text == txtConfirmPassword.Text)
            {
                txtConfirmPassword.TextColor = Color.FromHex("#00FF00");
                tblErConfirmPassword.IsVisible = true;
                tblErConfirmPassword.Text = "Trùng khớp mật khẩu!";
                tblErConfirmPassword.TextColor = Color.FromHex("#00FF00");
            }
            else
            {
                txtConfirmPassword.TextColor = Color.Red;
                tblErConfirmPassword.IsVisible = true;
                tblErConfirmPassword.Text = "Không trùng khớp mật khẩu!";
                tblErConfirmPassword.TextColor = Color.Red;
            }
        }

       
    }
}