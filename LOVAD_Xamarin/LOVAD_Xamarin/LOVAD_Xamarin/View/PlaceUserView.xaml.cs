using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class PlaceUserView : PopupPage
    {
        #region [AddPlaceForUser]
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }

        private string _namePlace;
        public string NamePlace { get { return _namePlace; } set { _namePlace = value; OnPropertyChanged("NamePlace"); } }

        private string _userId;
        public string UserId { get { return _userId; } set { _userId = value; OnPropertyChanged("UserId"); } }

        private string _id_Place;
        public string Id_Place { get { return _id_Place; } set { _id_Place = value; OnPropertyChanged("Id_Place"); } }


        private bool _checkAll;
        public bool CheckAll { get { return _checkAll; } set { _checkAll = value; OnPropertyChanged("CheckAll"); } }

        private ObservableCollection<PlaceModel> _listPlace;
        public ObservableCollection<PlaceModel> ListPlace { get { return _listPlace; } set { _listPlace = value; OnPropertyChanged(); } }

        #endregion

        UserModel userSelect;
        //string idPlaceSelect;
        RegExr regExr = new RegExr();
        List<PlaceModel> lstPlaceAddUser;
        List<string> IdPlace = new List<string>();
        public ViewCell lastCell;
        public int CountListPlace;
        public PlaceUserView(UserModel UserSelect)
        {
            InitializeComponent();
            userSelect = UserSelect;
            UserName = userSelect.UserName;
            GetPlaceOfUserMember();

            BindingContext = this;
        }
        public async void GetPlaceAddUserAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceAddUser?id_User=" +  userSelect.Id + "&page=" + 1;
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);


                    //client.DefaultRequestHeaders.Add("cookie", cookie);

                    StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<List<PlaceModel>>(result);
                        if (responseData != null)
                        {

                            lstPlaceAddUser = new List<PlaceModel>();
                            lstPlaceAddUser = responseData.ToList();
                            //pkrPlace.ItemsSource = lstPlaceAddUser;
                        }
                        else if (responseData == null)
                            await DisplayAlert("Thông báo", "Không tìm được user, không có cơ sở!", "OK");
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    //DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
        }
        public async void GetPlaceOfUserMember()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Path.Combine(documents, "cookie.txt");
            var cookie = File.ReadAllText(fileName);
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + userSelect.Id;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", cookie);
                HttpResponseMessage response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<List<PlaceModel>>(result);

                //var message = responseData.Content;
                if (responseData != null)
                {
                    ListDataPlaceUser.ItemsSource = responseData;
                }
                else if (responseData == null)
                {
                    await DisplayAlert("Thông báo", "Không tìm được user, không có cơ sở!", "OK");
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            grdAddPlaceForUser.IsVisible = false;
            grdSelectPlace.IsVisible = false;
            grdListPlace.IsVisible = true;
            MessagingCenter.Send(this, "cancel");//Gửi thông điệp
            PopupNavigation.Instance.PopAllAsync(true);
        }

        private async void btnDelPlaceOfUser_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Thông báo", "Bạn có muốn xóa cơ sở khỏi tài khoản này không?", "Đồng ý", "Hủy");
            if (answer == true)
            {
                var idPlace = (Button)sender;
                var PlaceId = idPlace.CommandParameter.ToString();

                WorkPlaceModel workPlaceModel = new WorkPlaceModel();
                workPlaceModel.Id_User = userSelect.Id;
                workPlaceModel.Id_Place = PlaceId;

                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/DelPlaceOfUser";

                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(workPlaceModel);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    //var responseData = JsonConvert.DeserializeObject<RespondModel>(result);

                    int responseData = int.Parse(result);


                    if (responseData == 0)
                    {
                        var message = "Xóa thành công!";
                        DependencyService.Get<IMessage>().ShortTime(message);
                        GetPlaceOfUserMember();
                    }
                    else if (responseData == 1)
                    {
                        var message = "WorkPlace đã tồn tại!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                    else if (responseData == 2)
                    {
                        var message = "Lỗi!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                    else
                    {
                        var message = "Xoá cơ sở cho tài khoản thất bại!";
                        DependencyService.Get<IMessage>().LongTime(message);
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


        private void btnAddPlace_Clicked(object sender, EventArgs e)
        {
            grdAddPlaceForUser.IsVisible = true;
            grdListPlace.IsVisible = false;
            grdSelectPlace.IsVisible = false;
            GetPlaceAddUserAsync();

            IdPlace.Clear();
        }


        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!regExr.UsernameReg(UserName))
            {
                tblUserName.Text = "Không đúng định dạng, hông để trống, từ 5 - 20 kí tự! (VD: abc_1.23, ...)";
                tblUserName.IsVisible = true;
            }
            else
                tblUserName.IsVisible = false;
        }



        private async void btnAddPlaceForUser_Clicked(object sender, EventArgs e)
        {
            if (IdPlace == null || IdPlace.Count<1)
                await DisplayAlert("Thông báo", "Vui lòng chọn cơ sở!", "OK");
            else
            {
                WorkPlaceModel workPlaceModel = new WorkPlaceModel();
                workPlaceModel.UserId = userSelect.Id;
                workPlaceModel.ListPlaceId = IdPlace;
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/AddListPlaceForUser";
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);

                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(workPlaceModel);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    content.Headers.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    //var responseData = JsonConvert.DeserializeObject<RespondModel>(result);

                    int responseData = Int32.Parse(result);

                    if (responseData == 0)
                    {
                        var message = "Thêm thành công!";
                        DependencyService.Get<IMessage>().ShortTime(message);
                        await PopupNavigation.Instance.PushAsync(new PlaceUserView(userSelect));
                    }
                    else if (responseData == 1)
                    {
                        var message = "Tài khoản không tồn tại!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                    else if (responseData == 2)
                    {
                        var message = "Lỗi nhận dữ liệu!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                    else if (responseData == 3)
                    {
                        var message = "Lỗi khi thêm cơ sở!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
        }

        private void btnCancelAddPlace_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PlaceUserView(userSelect));
        }



        private void LstvListPlace_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedPlace = (PlaceModel)LstvListPlace.SelectedItem;
            if (selectedPlace != null)
            {
                if (selectedPlace.IsSelected == true)
                    selectedPlace.IsSelected = false;
                else
                    selectedPlace.IsSelected = true;
            }
        }

        private void btnSelect_Clicked(object sender, EventArgs e)
        {
            if (ListPlace != null)
            {
                foreach (var item in ListPlace)
                {
                    if (item.IsSelected == true)
                    {
                        IdPlace.Add(item.Id);
                    }
                }
                btnSelectPlace.Text = IdPlace.Count() + " CƠ SỞ";
                grdAddPlaceForUser.IsVisible = true;
                grdListPlace.IsVisible = false;
                grdSelectPlace.IsVisible = false;
            }
            
        }

        private void btnSelectPlace_Clicked(object sender, EventArgs e)
        {
            IdPlace.Clear();
            //CountListPlace = 0;
            grdAddPlaceForUser.IsVisible = false;
            grdListPlace.IsVisible = false;
            grdSelectPlace.IsVisible = true;
            if (lstPlaceAddUser != null)
            {
                ListPlace = new ObservableCollection<PlaceModel>(lstPlaceAddUser);

            }
        }


        private void ViewCel_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.White;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;
                lastCell = viewCell;
            }
        }

        private void cbSelectAllPlace_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                if (ListPlace != null)
                {
                    foreach (var item in ListPlace)
                    {
                        if (item.IsSelected == false)
                        {
                            item.IsSelected = true;
                        }
                    }
                }
            }
            else
            {
                if (ListPlace != null)
                {
                    //btnSelectItemVehicle.IsEnabled = false;
                    foreach (var item in ListPlace)
                    {
                        if (item.IsSelected == true)
                        {
                            item.IsSelected = false;
                        }
                    }
                }
            }
        }
        private void cbSelectAllPlace2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                cbSelectAllPlace.IsVisible = true;

                cbSelectAllPlace2.IsVisible = false;
                cbSelectAllPlace2.IsChecked = false;

                if (cbSelectAllPlace.IsVisible == true)
                {
                    if (ListPlace != null)
                    {
                        //btnSelectItemVehicle.IsEnabled = true;
                        foreach (var item in ListPlace)
                        {
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;
                            }
                        }
                    }
                }
            }


        }

        private void cbSelectPlace_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ListPlace != null)
            {
                //cbSelectAllVehicle.IsChecked = null;
                bool Check = false;
                //CheckBox isCheckedOrNot = (CheckBox)sender;
                //var selectedVehicleList = isCheckedOrNot.BindingContext as DataSearchModel;
                foreach (var item in ListPlace)
                {
                    if (item.IsSelected == false)
                    {
                        Check = true;
                        break;
                    }
                }

                if (Check == true)
                {
                    cbSelectAllPlace2.IsVisible = true;
                    cbSelectAllPlace2.IsVisible = false;
                }
                else
                {
                    cbSelectAllPlace2.IsVisible = false;
                    cbSelectAllPlace.IsVisible = true;
                    cbSelectAllPlace.IsChecked = true;
                }

            }
        }
    }
}