using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserProfilePage : ContentPage
    {
        #region [ChangePassword]
        private string _userNameChange;
        public string UserNameChange { get { return _userNameChange; } set { _userNameChange = value; OnPropertyChanged("UserNameChange"); } }

        private string _currentPass;
        public string CurrentPass { get { return _currentPass; } set { _currentPass = value; OnPropertyChanged("CurrentPass"); } }

        private string _newPass;
        public string NewPass { get { return _newPass; } set { _newPass = value; OnPropertyChanged("NewPass"); } }

        private string _newPassConfirm;
        public string NewPassConfirm { get { return _newPassConfirm; } set { _newPassConfirm = value; OnPropertyChanged("NewPassConfirm"); } }

        #endregion

        #region [UpdateProfile]
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }

        private string _email;
        public string Email { get { return _email; } set { _email = value; OnPropertyChanged("Email"); } }

        private string _phoneNumber;
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; OnPropertyChanged("PhoneNumber"); } }

        private string _fullName;
        public string FullName { get { return _fullName; } set { _fullName = value; OnPropertyChanged("FullName"); } }

        #endregion

        public bool flag = false; //Bấm vào send Email
        UserModel userProfileModel = new UserModel();
        RegExr regExr = new RegExr();
        public EditUserProfilePage(UserModel user)
        {
            InitializeComponent();
            BindingContext = this;
          
            userProfileModel = user;
            UserName = userProfileModel.UserName;
            FullName = userProfileModel.FullName;
            PhoneNumber = userProfileModel.PhoneNumber;
            Email = userProfileModel.Email;

            UserNameChange = userProfileModel.UserName;

            //if (userProfileModel.EmailConfirmed == true)
            //{
            //    grdVerificationEmail.IsVisible = false;
            //}
            //else
            //    grdVerificationEmail.IsVisible = true;

        }
        public async void GetUserInf()
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
                    await Navigation.PushAsync(new EditUserProfilePage(responseData.UserRespond));
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

      
        //public void StartRunLoading()
        //{
           
        //    grdAccountInfo.IsVisible = false;
        //    grdChangePassword.IsVisible = false;
        //    grdLogOut.IsVisible = false;
        //    scrUserProfile.IsEnabled = false;
        //    frmLoading.IsEnabled = true;
        //    frmLoading.IsPlaying = true;
        //    frmLoading.IsVisible = true;
        //    NavigationPage.SetHasNavigationBar(this, false);

        //}

        //public void StopRunLoading()
        //{
           
        //    grdAccountInfo.IsVisible = true;
        //    grdChangePassword.IsVisible = true;
        //    grdLogOut.IsVisible = true;
        //    scrUserProfile.IsEnabled = true;
        //    frmLoading.IsEnabled = false;
        //    frmLoading.IsPlaying = false;
        //    frmLoading.IsVisible = false;
        //    NavigationPage.SetHasNavigationBar(this, true);
        //}

        private async void btnSaveChangePassword_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserNameChange) && !string.IsNullOrEmpty(CurrentPass) && !string.IsNullOrEmpty(NewPassConfirm) && tblErrNewPassConfirm.TextColor == Color.FromHex("#00FF00") && !string.IsNullOrEmpty(NewPass) && tblErrNewPass.IsVisible == false)
            {
                tblUserNameChange.IsVisible = false;
                tblCurrentPass.IsVisible = false;
                tblNewPass.IsVisible = false;
                tblNewPassConfirm.IsVisible = false;

                UserChangePassModel _UserChangePass = new UserChangePassModel();
                _UserChangePass.UserName = UserNameChange.Trim();
                _UserChangePass.CurrentPassword = CurrentPass.Trim();
                _UserChangePass.NewPassword = NewPass.Trim();



                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/ChangePassword";


                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);

                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(_UserChangePass);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondModel>(result);

                    var message = responseData.Content;
                    if (responseData.Result == true)
                    {
                        await Navigation.PushAsync(new EditUserProfilePage(userProfileModel));
                        DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                    }
                    else
                    {
                        //DependencyService.Get<IMessage>().Longtime(message[0].ToString());
                        await DisplayAlert("Thông báo", message[0].ToString(), "OK");
                    }
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

        private async void btnUpdateProfile_Clicked(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(PhoneNumber))
            {
                tblFullName.IsVisible = false;
                tblPhoneNumber.IsVisible = false;

                UserModel userUpdate = new UserModel();
                userUpdate.UserName = UserName.Trim();
                userUpdate.FullName = FullName.Trim();
                userUpdate.PhoneNumber = PhoneNumber.Trim();
                userUpdate.Email = Email.Trim();
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/UpdateProfile";
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);

                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(userUpdate);

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondModel>(result);

                    var message = responseData.Content;
                    if (responseData.Result == true)
                    {
                        string urlCheck = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetUserInf";
                        UserModel user = new UserModel();
                        user.UserName = userProfileModel.UserName;

                        try
                        {
                            var documents1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            var fileName1 = Path.Combine(documents1, "cookie.txt");
                            var cookie1 = File.ReadAllText(fileName1);

                            HttpClient client1 = new HttpClient();
                            string jsonData1 = JsonConvert.SerializeObject(user);

                            StringContent content1 = new StringContent(jsonData1, Encoding.UTF8, "application/json");
                            content1.Headers.Add("Cookie", cookie1);
                            HttpResponseMessage response1 = await client.PostAsync(urlCheck, content1);
                            string result1 = await response1.Content.ReadAsStringAsync();
                            var responseData1 = JsonConvert.DeserializeObject<RespondModel>(result1);

                            var message1 = responseData1.Content;
                            if (responseData1.Result == true)
                            {
                                if(responseData1.UserRespond.EmailConfirmed == true)
                                {
                                    DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                                    grdVerificationEmail.IsVisible = false;
                                }
                                else
                                {
                                    tblVerificationEmail.Text = "Email chưa xác nhận!";
                                    tblVerificationEmail.TextColor = Color.Red;
                                    grdVerificationEmail.IsVisible = true;
                                    DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                                }
                            }
                            else
                            {
                                await DisplayAlert("Thông báo", message[0].ToString(), "OK");
                            }
                        }
                        catch (Exception ex)
                        {
                            var Err = "Không nết nối được máy chủ";
                            DependencyService.Get<IMessage>().LongTime(Err);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", message[0].ToString(), "OK");
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            else
            {
                //if (tblVerificationEmail.TextColor == Color.Red)
                //{
                //    await DisplayAlert("Thông báo", "Email chưa được xác nhận!", "OK");
                //    return;
                //}
                await DisplayAlert("Thông báo", "Vui lòng nhập thông tin hợp lệ!", "OK");
            }


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


        private async void btnSendVerificationEmail_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("email"));
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(PhoneNumber))
            {
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/SendVerificationEmail";
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);

                    UserModel userInput = new UserModel();
                    userInput.UserName = UserName.Trim();
                    userInput.Email = Email.Trim();
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(userInput);

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string result = await response.Content.ReadAsStringAsync();
                    //var responseData = JsonConvert.DeserializeObject<RespondModel>(result);
                    int responseData = int.Parse(result);

                    if (responseData == 0)
                    {
                        var answer = await DisplayAlert("Thông báo", "Truy cập mail dể xác nhận?", "Truy cập email", "Đóng");
                        if(answer == true)
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                            await PopupNavigation.Instance.PushAsync(new ConfirmEmailView(userProfileModel));
                            flag = true;
                        }
                        else
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                            tblVerificationEmail.Text = "Email chưa xác nhận!";
                            tblVerificationEmail.TextColor = Color.Red;
                            var message = "Email chưa xác nhận!";
                            DependencyService.Get<IMessage>().LongTime(message);
                        }
                        
                    }
                    else if (responseData == 1)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        var message = "Lỗi khi đọc dữ liệu từ requestbody!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        tblVerificationEmail.Text = "Email chưa xác nhận!";
                        tblVerificationEmail.TextColor = Color.Red;
                    }
                    else if (responseData == 2)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        var message = "Không tìm thấy tài khoản!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        tblVerificationEmail.Text = "Email chưa xác nhận!";
                        tblVerificationEmail.TextColor = Color.Red;
                    }
                    else if (responseData == 3)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        var message = "Lỗi khi lưu email!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        tblVerificationEmail.Text = "Email chưa xác nhận!";
                        tblVerificationEmail.TextColor = Color.Red;
                    }
                    else if (responseData == 4)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        var message = "Lỗi khi gửi mail!";
                        await DisplayAlert("Thông báo", message, "OK");
                        tblVerificationEmail.Text = "Email chưa xác nhận!";
                        tblVerificationEmail.TextColor = Color.Red;
                    }
                    else
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        await DisplayAlert("Thông báo", "Email chưa được xác nhận!", "OK");
                        tblVerificationEmail.Text = "Email chưa xác nhận!";
                        tblVerificationEmail.TextColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            else
            {
                await PopupNavigation.Instance.PopAllAsync();
                await DisplayAlert("Thông báo", "Vui lòng nhập đầy đủ thông tin!", "OK");
            }

            
        }

        #region [Kiểm tra nhập text]
        private void txtNewPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.PasswordReg(NewPass))
            {
                tblNewPass.Text = "Mật khẩu ít nhất 8 kí tự, 1 kí tự đặt biệt, 1 chữ cái viết hoa, 1 chữ thường, 1 số (VD: !Abcde12, ...)";
                tblNewPass.IsVisible = true;
            }
            else
                tblNewPass.IsVisible = false;

            if (txtNewPass.Text == txtCurrentPass.Text && txtNewPass.Text != "")
            {
                tblErrNewPass.IsVisible = true;
                txtNewPass.TextColor = Color.Red;
            }
            else
            {
                tblErrNewPass.IsVisible = false;
                txtNewPass.TextColor = Color.Black;
            }

            if (txtNewPass.Text == txtNewPassConfirm.Text && txtNewPass.Text != "")
            {
                txtNewPassConfirm.TextColor = Color.FromHex("#00FF00");
                tblErrNewPassConfirm.IsVisible = true;
                tblErrNewPassConfirm.Text = "Trùng khớp mật khẩu!";
                tblErrNewPassConfirm.TextColor = Color.FromHex("#00FF00");
            }
            else
            {
                txtNewPassConfirm.TextColor = Color.Red;
                tblErrNewPassConfirm.IsVisible = true;
                tblErrNewPassConfirm.Text = "Không trùng khớp mật khẩu!";
                tblErrNewPassConfirm.TextColor = Color.Red;
            }
        }
        private void txtNewPassConfirm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPassConfirm))
            {
                tblNewPassConfirm.Text = "Không để trống!";
                tblNewPassConfirm.IsVisible = true;
            }

            else
                tblNewPassConfirm.IsVisible = false;


            if (txtNewPassConfirm.Text == txtNewPass.Text && txtNewPassConfirm.Text != "")
            {
                txtNewPassConfirm.TextColor = Color.FromHex("#00FF00");
                tblErrNewPassConfirm.IsVisible = true;
                tblErrNewPassConfirm.Text = "Trùng khớp mật khẩu!";
                tblErrNewPassConfirm.TextColor = Color.FromHex("#00FF00");
            }
            else
            {
                txtNewPassConfirm.TextColor = Color.Red;
                tblErrNewPassConfirm.IsVisible = true;
                tblErrNewPassConfirm.Text = "Không trùng khớp mật khẩu!";
                tblErrNewPassConfirm.TextColor = Color.Red;
            }
        }
        private void txtCurrentPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentPass))
            {
                tblCurrentPass.Text = "Không để trống!";
                tblCurrentPass.IsVisible = true;
            }
            else
                tblCurrentPass.IsVisible = false;

            if (txtCurrentPass.Text == txtNewPass.Text && txtCurrentPass.Text != "")
            {
                tblErrNewPass.IsVisible = true;
                txtNewPass.TextColor = Color.Red;
            }
            else
            {
                tblErrNewPass.IsVisible = false;
                txtNewPass.TextColor = Color.Black;
            }
        }
        private void txtUserNameChange_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!regExr.UsernameReg(UserNameChange))
            {
                tblUserNameChange.Text = "Không đúng định dạng, không để trống, từ 5 - 20 kí tự! (VD: abc_1.23, ...)";

                tblUserNameChange.IsVisible = true;
            }
            else
                tblUserNameChange.IsVisible = false;
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
        private void txtFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.NameReg(FullName))
            {
                tblFullName.Text = "Tên phải là chữ, không để trống! (VD: Nguyễn Văn A, ...)";
                tblFullName.IsVisible = true;
            }
            else
                tblFullName.IsVisible = false;
        }
        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.PhoneNumberReg(PhoneNumber))
            {
                tblPhoneNumber.Text = "Không đúng định dạng, không để trống! (VD: 012345678, ...)";
                tblPhoneNumber.IsVisible = true;
            }
            else
                tblPhoneNumber.IsVisible = false;
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.EmailReg(Email))
            {
                tblEmail.Text = "Không đúng định dạng, không để trống! (VD: abc@gmail.com, xyz@yahoo.com, ...)";
                tblEmail.IsVisible = true;
            }
            else
                tblEmail.IsVisible = false;
        }
        #endregion

    }
}