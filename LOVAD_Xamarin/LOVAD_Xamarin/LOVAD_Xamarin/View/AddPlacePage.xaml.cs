using LOVAD_Xamarin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPlacePage : ContentPage
    {
        #region AddPlace
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }

        private string _passWordAPI;
        public string PassWordAPI { get { return _passWordAPI; } set { _passWordAPI = value; OnPropertyChanged("PassWordAPI"); } }

        private string _passWordDB;
        public string PassWordDB { get { return _passWordDB; } set { _passWordDB = value; OnPropertyChanged("PassWordDB"); } }

        private string _portAPI;
        public string PortAPI { get { return _portAPI; } set { _portAPI = value; OnPropertyChanged("PortAPI"); } }

        private string _portDB;
        public string PortDB { get { return _portDB; } set { _portDB = value; OnPropertyChanged("PortDB"); } }

        //private int _typePlace;
        //public int TypePlace { get { return _typePlace; } set { _typePlace = value; OnPropertyChanged("TypePlace"); } }

        private string _userNameAPI;
        public string UserNameAPI { get { return _userNameAPI; } set { _userNameAPI = value; OnPropertyChanged("UserNameAPI"); } }

        private string _userNameDB;
        public string UserNameDB { get { return _userNameDB; } set { _userNameDB = value; OnPropertyChanged("UserNameDB"); } }

        private string _address;
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged("Address"); } }
        private string _ipAddress;
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged("IpAddress"); } }


        #endregion

        PlaceModel placeModel;
        UserModel User = new UserModel();
        RegExr regExr = new RegExr();
        string Result;
        public AddPlacePage(PlaceModel place, UserModel user, string flag)
        {

            InitializeComponent();
            BindingContext = this;
            placeModel = place;
            User = user;
            Result = flag;
            lbTitle.Text = "ĐĂNG KÝ CƠ SỞ";
            if (Result.Equals("EditPlace"))
            {
                lbTitle.Text = "CHỈNH SỬA CƠ SỞ";
                Name = placeModel.Name;
                PassWordAPI = placeModel.PassWordAPI;
                PassWordDB = placeModel.PassWordDB;
                PortAPI = placeModel.PortAPI.ToString();
                PortDB = placeModel.PortDB.ToString();
                pkrTypePlace.SelectedIndex = placeModel.TypePlace;
                UserNameAPI = placeModel.UserNameAPI;
                UserNameDB = placeModel.UserNameDB;
                Address = placeModel.Address;
                IpAddress = placeModel.IpAddress;
            }
            //else
            //{
            //    Name = "";
            //    PassWordAPI = "";
            //    PassWordDB = "";
            //    PortAPI = "";
            //    PortDB = "";
            //    pkrTypePlace.SelectedIndex = -1;
            //    UserNameAPI = "";
            //    UserNameDB = "";
            //    Address = "";
            //}

#if DEBUG
            Name = "Đại học Công Nghiệp HCM";
            PassWordAPI = "!Quy1234";
            PassWordDB = "123";
            PortAPI = "1234";
            PortDB = "1234";
            UserNameAPI = "huuquy";
            UserNameDB = "sa";
            Address = "Hồ Chí Minh";
            IpAddress = "192.168.1.1";
#endif
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if (Result.Equals("AddPlace"))//AddPlace
            {

                if (tblErrPlaceName.IsVisible == false && tblErrIpAddress.IsVisible == false && tblErrAddress.IsVisible == false && tblErrPassWordAPI.IsVisible == false && tblErrPassWordDB.IsVisible == false && tblErrPortAPI.IsVisible == false && tblErrPortDB.IsVisible == false && tblErrUsernameAPI.IsVisible == false && tblErrUsernameDB.IsVisible == false && pkrTypePlace.SelectedItem != null)
                {
                    placeModel = new PlaceModel();
                    placeModel.Name = Name;
                    placeModel.PassWordAPI = PassWordAPI;
                    placeModel.PassWordDB = PassWordDB;
                    placeModel.PortAPI = Int32.Parse(PortAPI);
                    placeModel.PortDB = Int32.Parse(PortDB);
                    placeModel.TypePlace = pkrTypePlace.SelectedIndex;
                    placeModel.UserNameAPI = UserNameAPI;
                    placeModel.UserNameDB = UserNameDB;
                    placeModel.Address = Address;
                    placeModel.IpAddress = IpAddress;

                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/AddNewPlace";

                    try
                    {
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var fileName = Path.Combine(documents, "cookie.txt");
                        var cookie = File.ReadAllText(fileName);

                        HttpClient client = new HttpClient();
                        string jsonData = JsonConvert.SerializeObject(placeModel);

                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        content.Headers.Add("cookie", cookie);
                        HttpResponseMessage response = await client.PostAsync(url, content);

                        string result = await response.Content.ReadAsStringAsync();

                        //int responseData = Int32.Parse(result);

                        var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                        if (responseData != null && responseData.Result == true)
                        {

                            //DependencyService.Get<IMessage>().ShortTime(message);
                            ////MessagingCenter.Send(this, "Update");//Gửi thông điệp update listview về PlacePage
                            //await Navigation.PushAsync(new PlacePage(User));
                            //await Navigation.PopAsync();
                           
                            MessagingCenter.Send(this, "Update");//Gửi thông điệp update listview về PlacePage
                            await Navigation.PopAsync();
                            var message = "Thành công!";
                            DependencyService.Get<IMessage>().ShortTime(message.ToString());
                        }
                        else if (responseData != null && responseData.Result == false)
                        {
                            var message = responseData.Content.ToString();
                            DependencyService.Get<IMessage>().LongTime(message);
                        }
                        else
                        {
                            var message = "Thêm không thành công!";
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
                {
                    if (pkrTypePlace.SelectedItem == null)
                    {
                        await DisplayAlert("Thông báo", "Vui lòng chọn loại cơ sở!", "OK");
                    }
                    else
                        await DisplayAlert("Thông báo", "Vui lòng nhập đúng thông tin!", "OK");
                }
            }
            else //EditPlace
            {

                if (tblErrPlaceName.IsVisible == false && tblErrIpAddress.IsVisible == false && tblErrAddress.IsVisible == false && tblErrPassWordAPI.IsVisible == false && tblErrPassWordDB.IsVisible == false && tblErrPortAPI.IsVisible == false && tblErrPortDB.IsVisible == false && tblErrUsernameAPI.IsVisible == false && tblErrUsernameDB.IsVisible == false && pkrTypePlace.SelectedItem != null && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(PortDB) && !string.IsNullOrEmpty(UserNameDB) && !string.IsNullOrEmpty(PassWordDB) && !string.IsNullOrEmpty(PortAPI) && !string.IsNullOrEmpty(UserNameAPI) && !string.IsNullOrEmpty(PassWordAPI) && pkrTypePlace.SelectedIndex != -1)
                {
                    placeModel.Name = Name;
                    placeModel.PassWordAPI = PassWordAPI;
                    placeModel.PassWordDB = PassWordDB;
                    placeModel.PortAPI = Int32.Parse(PortAPI);
                    placeModel.PortDB = Int32.Parse(PortDB);
                    placeModel.TypePlace = pkrTypePlace.SelectedIndex;
                    placeModel.UserNameAPI = UserNameAPI;
                    placeModel.UserNameDB = UserNameDB;
                    placeModel.Address = Address;

                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/EditPlace";

                    try
                    {
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var fileName = Path.Combine(documents, "cookie.txt");
                        var cookie = File.ReadAllText(fileName);

                        HttpClient client = new HttpClient();
                        string jsonData = JsonConvert.SerializeObject(placeModel);

                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        content.Headers.Add("cookie", cookie);
                        HttpResponseMessage response = await client.PostAsync(url, content);

                        string result = await response.Content.ReadAsStringAsync();

                        var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);
                       
                        if (responseData.Result == true)
                        {
                            MessagingCenter.Send(this, "Update");//Gửi thông điệp update listview về PlacePage
                            await Navigation.PopAsync();
                            var message = responseData.Content;
                            DependencyService.Get<IMessage>().ShortTime(message[0].ToString());
                        }
                        else
                        {
                            var message1 = "Thêm không thành công!";
                            await DisplayAlert("Thông báo", message1, "OK");
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
                    if (pkrTypePlace.SelectedItem == null)
                    {
                        await DisplayAlert("Thông báo", "Vui lòng chọn loại cơ sở!", "OK");
                    }
                    else
                        await DisplayAlert("Thông báo", "Vui lòng nhập đúng thông tin!", "OK");
                }
            }


        }
        private async void btnCheckPlace_Clicked(object sender, EventArgs e)
        {

            if (tblErrPlaceName.IsVisible == false && tblErrIpAddress.IsVisible == false && tblErrAddress.IsVisible == false && tblErrPassWordAPI.IsVisible == false && tblErrPassWordDB.IsVisible == false && tblErrPortAPI.IsVisible == false && tblErrPortDB.IsVisible == false && tblErrUsernameAPI.IsVisible == false && tblErrUsernameDB.IsVisible == false && pkrTypePlace.SelectedItem != null && Name != null && PassWordAPI != null && PassWordDB != null && PortAPI != null && PortDB != null && UserNameAPI != null && UserNameDB != null && Address != null && IpAddress != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/CheckPlace?port=" + PortAPI + "&ipaddress=" + IpAddress;
                    try
                    {
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var fileName = Path.Combine(documents, "cookie.txt");
                        var cookie = File.ReadAllText(fileName);

                        client.DefaultRequestHeaders.Add("Cookie", cookie);
                        HttpResponseMessage response = await client.GetAsync(url);
                        var result = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                        //var message = responseData.Content;
                        if (responseData != null && responseData.Result == true)
                        {
                            DependencyService.Get<IMessage>().ShortTime(responseData.Message);
                            btnSave.IsEnabled = true;
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongTime(responseData.Message);
                            btnSave.IsEnabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        var Err = "Không nết nối được máy chủ";
                        DependencyService.Get<IMessage>().LongTime(Err);
                        btnSave.IsEnabled = false;
                    }
                }
            }
            else
            {
                if (pkrTypePlace.SelectedItem == null)
                {
                    await DisplayAlert("Thông báo", "Vui lòng chọn loại cơ sở!", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Vui lòng nhập đúng thông tin!", "OK");
            }


        }

        private void txtPlaceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.AddressReg(Name))
            {
                tblErrPlaceName.Text = "Không đúng định dạng, không để trống! (VD: Đại học Quốc Gia HCM, ...)";

                tblErrPlaceName.IsVisible = true;
            }
            else
                tblErrPlaceName.IsVisible = false;
        }

        private void txtUsernameAPI_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.UsernameReg(UserNameAPI))
            {
                tblErrUsernameAPI.Text = "Không đúng định dạng, không để trống, từ 5 - 20 kí tự! (VD: abc_1.23, ...)";

                tblErrUsernameAPI.IsVisible = true;
            }
            else
                tblErrUsernameAPI.IsVisible = false;
        }

        private void txtPassWordAPI_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.PasswordReg(PassWordAPI))
            {
                tblErrPassWordAPI.Text = "Mật khẩu ít nhất 8 kí tự, 1 kí tự đặt biệt, 1 chữ cái viết hoa, 1 chữ thường, 1 số (VD: !Abcde12, ...)";
                tblErrPassWordAPI.IsVisible = true;
            }
            else
                tblErrPassWordAPI.IsVisible = false;
        }

        private void txtPortAPI_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = false;
            if (!regExr.NumberReg(PortAPI))
            {
                tblErrPortAPI.Text = "Port API phải là số (VD: 123, 456, ...)";
                tblErrPortAPI.IsVisible = true;
            }
            else
                tblErrPortAPI.IsVisible = false;
        }

        private void txtUsernameDB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.StringOrNumberReg(UserNameDB))
            {
                tblErrUsernameDB.Text = "Tài khoản database chứa chữ cái và số (VD: abc, abc456, 456,...)";
                tblErrUsernameDB.IsVisible = true;
            }
            else
                tblErrUsernameDB.IsVisible = false;
        }

        private void txtPassWordDB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.StringOrNumberReg(PassWordDB))
            {
                tblErrPassWordDB.Text = "Mật khẩu chứa chữ cái và số (VD: abc, abc456, 456,...)";
                tblErrPassWordDB.IsVisible = true;
            }
            else
                tblErrPassWordDB.IsVisible = false;
        }

        private void txtPortDB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.NumberReg(PortDB))
            {
                tblErrPortDB.Text = "Port database phải là số (VD: 123, 456, ...)";
                tblErrPortDB.IsVisible = true;
            }
            else
                tblErrPortDB.IsVisible = false;
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.AddressReg(Address))
            {
                tblErrAddress.Text = "Vui lòng nhập địa chỉ (VD: Đường 20, Hiệp Bình Chánh, Thủ Đức, thành phố Hồ Chí Minh; Lê Duẫn, thành phố Đà Nẵng;...)";
                tblErrAddress.IsVisible = true;
            }
            else
                tblErrAddress.IsVisible = false;
        }

        private void txtIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = false;
            if (!regExr.AddressReg(IpAddress))
            {
                tblErrIpAddress.Text = "Vui lòng nhập địa chỉ IP!";
                tblErrIpAddress.IsVisible = true;
            }
            else
                tblErrIpAddress.IsVisible = false;
        }

    }
}