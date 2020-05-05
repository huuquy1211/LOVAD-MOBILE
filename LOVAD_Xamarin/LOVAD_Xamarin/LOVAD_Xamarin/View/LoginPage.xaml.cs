//using Android.Widget;
using IdentityModel.Client;
using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        #region [LoginModel]
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }

        private string _userId;
        public string UserId { get { return _userId; } set { _userId = value; OnPropertyChanged("UserId"); } }


        private string _passWord;
        public string PassWord { get { return _passWord; } set { _passWord = value; OnPropertyChanged("PassWord"); } }

        private bool _rememberMe;
        public bool RememberMe { get { return _rememberMe; } set { _rememberMe = value; OnPropertyChanged("RememberMe"); } }


        private string _ipAddress;
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged("IpAddress"); } }

        private string _portAPI;
        public string PortAPI { get { return _portAPI; } set { _portAPI = value; OnPropertyChanged("PortAPI"); } }

        #endregion
        public int flag;
        LoginModel loginModel;
        RegExr regExr = new RegExr();

        private bool checkSetting = false;//Kiểm tra đã cấu hình chưa

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
            //Load lại mật khẩu
            EventRememberMe();
            //Load lại uri
            EventRememberUri();
            btnSaveSetting.IsEnabled = false;
        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return base.OnBackButtonPressed();
        }

        #region [Mã hóa mật khẫu để lưu vào thiết bị]
        static public string EncodeTo64(string toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongTime(ex.Message);
                throw;
            }

        }

        static public string DecodeFrom64(string encodeData)
        {
            try
            {
                byte[] encodeDataAsBytes = System.Convert.FromBase64String(encodeData);
                string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodeDataAsBytes);
                return returnValue;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongTime(ex.Message);
                throw;
            }

        }
        #endregion

        #region [Load lại các dữ liệu đã lưu trên máy]
        public void EventRememberUri()
        {
            try
            {
                //Đọc file uri
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var SerIpAddress = Path.Combine(documents, "IpAddress.txt");
                var SerPortAPI = Path.Combine(documents, "PortAPI.txt");

                var RememberSerIpAddress = File.ReadAllText(SerIpAddress);
                var RememberSerPortAPI = File.ReadAllText(SerPortAPI);

                if (!string.IsNullOrEmpty(RememberSerIpAddress) && !string.IsNullOrEmpty(RememberSerPortAPI))
                {
                    Global.Intance.SerIpAdress = RememberSerIpAddress;
                    Global.Intance.SerPortAPI = RememberSerPortAPI;
                }
                else
                {
                    Global.Intance.SerIpAdress = null;
                    Global.Intance.SerPortAPI = null;
                }
            }
            catch (Exception)
            {
                Global.Intance.SerIpAdress = null;
                Global.Intance.SerPortAPI = null;
            }
        }
        public void EventRememberMe()
        {
            try
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var UserNameDecode = Path.Combine(documents, "username.txt");
                var PassWordDecode = Path.Combine(documents, "password.txt");
                var Check = Path.Combine(documents, "check.txt");


                var RememberUsername = File.ReadAllText(UserNameDecode);
                var RememberPassword = File.ReadAllText(PassWordDecode);
                int check = Int32.Parse(File.ReadAllText(Check));

                var username = DecodeFrom64(RememberUsername);
                var password = DecodeFrom64(RememberPassword);


                if (check == 1)
                {
                    UserName = username;
                    PassWord = password;
                    RememberMe = true;
                }
                else
                {
                    UserName = null;
                    PassWord = null;
                    RememberMe = false;
                }
            }
            catch (Exception)
            {
                UserName = null;
                PassWord = null;
                RememberMe = false;
            }
        }
        #endregion

        #region [Xử lý nhập]
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                lblUserNameErr.IsVisible = true;
            }
            else
                lblUserNameErr.IsVisible = false;
        }

        private void txtPassWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(PassWord))
            {
                lblPassWordErr.IsVisible = true;
            }
            else
                lblPassWordErr.IsVisible = false;
        }
        #endregion

        #region [Login]
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("login"));

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord))
            {
                lblPassWordErr.IsVisible = false;
                lblUserNameErr.IsVisible = false;

                loginModel = new LoginModel();
                loginModel.UserName = UserName.Trim();
                loginModel.Password = PassWord.Trim();
                loginModel.RememberMe = RememberMe;
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var RememberUsername = Path.Combine(documents, "username.txt");
                    var RememberPassword = Path.Combine(documents, "password.txt");
                    var Check = Path.Combine(documents, "check.txt");
                    if (RememberMe == true)
                    {
                        var UserNameEncode = EncodeTo64(UserName);
                        var PassWordEncode = EncodeTo64(PassWord);

                        File.WriteAllText(RememberUsername, UserNameEncode);
                        File.WriteAllText(RememberPassword, PassWordEncode);
                        flag = 1;
                        File.WriteAllText(Check, flag.ToString());
                    }
                    else
                    {

                        File.WriteAllText(RememberUsername, null);
                        File.WriteAllText(RememberPassword, null);
                        flag = 0;
                        File.WriteAllText(Check, flag.ToString());
                    }

                    var UriServer = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var SerIpAddress = Path.Combine(UriServer, "IpAddress.txt");
                    var SerPortAPI = Path.Combine(UriServer, "PortAPI.txt");

                    File.WriteAllText(SerIpAddress, Global.Intance.SerIpAdress);
                    File.WriteAllText(SerPortAPI, Global.Intance.SerPortAPI);



                }
                catch (Exception)
                {
                    await DisplayAlert("Thông báo", "Lỗi lưu lại mật khẩu!", "OK");
                }


                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/Login";

               
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                        string jsonData = JsonConvert.SerializeObject(loginModel);
                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                      
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        string result = await response.Content.ReadAsStringAsync();

                        HttpHeaders headers = response.Headers;// Lấy các Headers  trả về

                        foreach (var header in headers)
                        {
                            string key_header = header.Key;// Key, ví dụ: Cache-Control, Set-Cookie ...
                            if (key_header.Contains("Set-Cookie"))
                            {
                                IEnumerable<string> value_header = header.Value;// Danh sách các giá trị cho header
                                Global.Intance.Cookie = string.Join(",", value_header);
                            }
                        }

                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var fileName = Path.Combine(documents, "cookie.txt");
                        File.WriteAllText(fileName, Global.Intance.Cookie);

                        var responseData = JsonConvert.DeserializeObject<RespondModel>(result);
                        var message = responseData.Content;
                        if (responseData != null && responseData.Result == true && responseData.UserRespond != null)
                        {
                            if (responseData.UserRespond.Id != null)
                            {
                                await PopupNavigation.Instance.PopAllAsync();
                                await Navigation.PushAsync(new MainPage(responseData.UserRespond));
                                DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                                return;
                            }
                            else
                            {
                                await PopupNavigation.Instance.PopAllAsync();
                                var Err = "Không nết nối được máy chủ!";
                                DependencyService.Get<IMessage>().LongTime(Err);
                                return;
                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                            DependencyService.Get<IMessage>().LongTime(message[0].ToString());
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    var Err = "Không nết nối được máy chủ!";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            else
            {
                await PopupNavigation.Instance.PopAllAsync();
                await DisplayAlert("Thông báo", "Vui lòng nhập đầy đủ thông tin!", "OK");
            }
        }

        private void btnShowPassWord_Clicked(object sender, EventArgs e)
        {
            if (txtPassWord.IsPassword == true)
            {
                btnShowPassWord.ImageSource = "show.png";
                txtPassWord.IsPassword = false;
                return;
            }

            if (txtPassWord.IsPassword == false)
            {
                btnShowPassWord.ImageSource = "hiden.png";
                txtPassWord.IsPassword = true;
                return;
            }
        }

        private async void tblForgotPassword_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            await PopupNavigation.Instance.PopAsync();
            var Err = "Không truy cập vào chức năng này!";
            DependencyService.Get<IMessage>().LongTime(Err);
            return;
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
        #endregion

        #region [Cấu hình ứng dụng]

        private void btnSetting_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                PopupNavigation.Instance.PushAsync(new LoadingView("search"));
                //Đọc file uri
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var SerIpAddress = Path.Combine(documents, "IpAddress.txt");
                var SerPortAPI = Path.Combine(documents, "PortAPI.txt");

                var RememberSerIpAddress = File.ReadAllText(SerIpAddress);
                var RememberSerPortAPI = File.ReadAllText(SerPortAPI);

                if (!string.IsNullOrEmpty(RememberSerIpAddress) && !string.IsNullOrEmpty(RememberSerPortAPI))
                {
                    IpAddress = RememberSerIpAddress;
                    PortAPI = RememberSerPortAPI;
                }
                else
                {
                    IpAddress = null;
                    PortAPI = null;
                }
            }
            catch (Exception)
            {
                IpAddress = null;
                PortAPI = null;
            }

            grdLogin.IsVisible = false;
            grdSetting.IsVisible = true;
            PopupNavigation.Instance.PopAllAsync();
        }

        private void btnSaveSetting_Clicked(object sender, EventArgs e)
        {
            try
            {
                PopupNavigation.Instance.PushAsync(new LoadingView("search"));

                checkSetting = true;
                grdLogin.IsVisible = true;
                grdSetting.IsVisible = false;
               

                Global.Intance.SerIpAdress = IpAddress;
                Global.Intance.SerPortAPI = PortAPI;

                //Lưu file uri
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var SerIpAddress = Path.Combine(documents, "IpAddress.txt");
                var SerPortAPI = Path.Combine(documents, "PortAPI.txt");

                File.WriteAllText(SerIpAddress, Global.Intance.SerIpAdress);
                File.WriteAllText(SerPortAPI, Global.Intance.SerPortAPI);

                
                PopupNavigation.Instance.PopAllAsync();
                var Err = "Cấu hình thành công!";
                DependencyService.Get<IMessage>().ShortTime(Err);
            }
            catch (Exception)
            {
                PopupNavigation.Instance.PopAllAsync();
                var Err = "Lỗi lưu địa chỉ API, Port!";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
            
        }

        private void txtIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (!regExr.AddressReg(IpAddress))
            {
                tblErrIpAddress.Text = "Vui lòng nhập địa chỉ IP!";
                tblErrIpAddress.IsVisible = true;
            }
            else
            {
                tblErrIpAddress.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(IpAddress) && !string.IsNullOrEmpty(PortAPI) && tblErrIpAddress.IsVisible == false && tblErrPortAPI.IsVisible == false)
            {
                btnSaveSetting.IsEnabled = true;
            }
            else
                btnSaveSetting.IsEnabled = false;
        }

        private void txtPortAPI_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (!regExr.NumberReg(PortAPI))
            {
                tblErrPortAPI.Text = "Port API phải là số (VD: 123, 456, ...)";
                tblErrPortAPI.IsVisible = true;
            }
            else
            {
                tblErrPortAPI.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(IpAddress) && !string.IsNullOrEmpty(PortAPI) && tblErrIpAddress.IsVisible == false && tblErrPortAPI.IsVisible == false)
            {
                btnSaveSetting.IsEnabled = true;
            }
            else
                btnSaveSetting.IsEnabled = false;
        }

        private void btnCloseSetting_Clicked(object sender, EventArgs e)
        {
            checkSetting = false;
            grdLogin.IsVisible = true;
            grdSetting.IsVisible = false;
            var Err = "Chưa cấu hình!";
            DependencyService.Get<IMessage>().LongTime(Err);
        }
        #endregion
    }
}