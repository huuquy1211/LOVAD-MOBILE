using LOVAD_Xamarin.Model;
using LOVAD_Xamarin.Utility;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
       
       
        //Place add user
        private string _pageSize;
        public string PageSize { get { return _pageSize; } set { _pageSize = value; OnPropertyChanged("PageSize"); } }

        private int _page;
        public int Page { get { return _page; } set { _page = value; OnPropertyChanged("Page"); } }

        private string _curentPage;
        public string CurentPage { get { return _curentPage; } set { _curentPage = value; OnPropertyChanged("CurentPage"); } }

        private int _countPage;
        public int CountPage { get { return _countPage; } set { _countPage = value; OnPropertyChanged("CountPage"); } }

        //Account
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }

        private string _pageSizeAcc;
        public string PageSizeAcc { get { return _pageSizeAcc; } set { _pageSizeAcc = value; OnPropertyChanged("PageSizeAcc"); } }

        private int _pageAcc;
        public int PageAcc { get { return _pageAcc; } set { _pageAcc = value; OnPropertyChanged("PageAcc"); } }

        private string _curentPageAcc;
        public string CurentPageAcc { get { return _curentPageAcc; } set { _curentPageAcc = value; OnPropertyChanged("CurentPageAcc"); } }

        private int _countPageAcc;
        public int CountPageAcc { get { return _countPageAcc; } set { _countPageAcc = value; OnPropertyChanged("CountPageAcc"); } }

        //Place of user
        private string _pageSizeUser;
        public string PageSizeUser { get { return _pageSizeUser; } set { _pageSizeUser = value; OnPropertyChanged("PageSizeUser"); } }

        private int _pageUser;
        public int PageUser { get { return _pageUser; } set { _pageUser = value; OnPropertyChanged("PageUser"); } }

        private string _curentPageUser;
        public string CurentPageUser { get { return _curentPageUser; } set { _curentPageUser = value; OnPropertyChanged("CurentPageUser"); } }

        private int _countPageUser;
        public int CountPageUser { get { return _countPageUser; } set { _countPageUser = value; OnPropertyChanged("CountPageUser"); } }

        //Search Account
        private string _userNameSearch;
        public string UserNameSearch { get { return _userNameSearch; } set { _userNameSearch = value; OnPropertyChanged("UserNameSearch"); } }

        private string _email;
        public string Email { get { return _email; } set { _email = value; OnPropertyChanged("Email"); } }

        private string _fullName;
        public string FullName { get { return _fullName; } set { _fullName = value; OnPropertyChanged("FullName"); } }

        //Search Place add user
        private string _placeName;
        public string PlaceName { get { return _placeName; } set { _placeName = value; OnPropertyChanged("PlaceName"); } }

        private string _portApi;
        public string PortApi { get { return _portApi; } set { _portApi = value; OnPropertyChanged("PortApi"); } }
       
        private string _ipAddress;
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged("IpAddress"); } }


        //Search List Place
        private string _placeNameListPlace;
        public string PlaceNameListPlace { get { return _placeNameListPlace; } set { _placeNameListPlace = value; OnPropertyChanged("PlaceNameListPlace"); } }

        private string _portApiListPlace;
        public string PortApiListPlace { get { return _portApiListPlace; } set { _portApiListPlace = value; OnPropertyChanged("PortApiListPlace"); } }

        private string _ipAddressListPlace;
        public string IpAddressListPlace { get { return _ipAddressListPlace; } set { _ipAddressListPlace = value; OnPropertyChanged("IpAddressListPlace"); } }

        //chiều dài và rộng listview
        private double _heightListView;
        public double HeightListView { get { return _heightListView; } set { _heightListView = value; OnPropertyChanged("HeightListView"); } }
        private double _widthListView;
        public double WidthListView { get { return _widthListView; } set { _widthListView = value; OnPropertyChanged("WidthListView"); } }


        private ObservableCollection<PlaceModel> _listPlaceOfUser;
        public ObservableCollection<PlaceModel> ListPlaceOfUser { get { return _listPlaceOfUser; } set { _listPlaceOfUser = value; OnPropertyChanged(); } }

        private ObservableCollection<PlaceModel> _listPlaceAddUser;
        public ObservableCollection<PlaceModel> ListPlaceAddUser { get { return _listPlaceAddUser; } set { _listPlaceAddUser = value; OnPropertyChanged(); } }
        private ObservableCollection<UserModel> _listUser;
        public ObservableCollection<UserModel> ListUser { get { return _listUser; } set { _listUser = value; OnPropertyChanged("ListUser"); } }

        public UserModel userModel;
        public UserModel userSelect;
        public int flagShowPlace;
        public string IdUserSelect;
        private ViewCell lastCell;


        private string typePlace;// loại cơ sở chọn khi search place add user
        private string typePlaceListPlace;// loại cơ sở chọn khi search list place
        List<string> IdPlace;
        RegExr regExr = new RegExr();
        public AccountPage(UserModel user)
        {
            InitializeComponent();
            BindingContext = this;
            userModel = new UserModel();
            userModel = user;
            flagShowPlace = 0;
            Page = 1;
            pkrPageSizeAcc.SelectedIndex = 1;
            PageAcc = 1;



            GetAllUser();
            CurentPageAcc = "Trang " + PageAcc + "/" + CountPageAcc;


            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;

            btnBackPageAcc.IsEnabled = false;
            btnFristPageAcc.IsEnabled = false;
            btnLastPageAcc.IsEnabled = true;
            btnNextPageAcc.IsEnabled = true;



            MessagingCenter.Subscribe<AddAccountPage>(this, "Update", (sender) =>
            {
                PageAcc = 1;
                GetAllUser();
                pkrPageSizeAcc.SelectedIndex = 0;
                btnBackPageAcc.IsEnabled = false;
                btnFristPageAcc.IsEnabled = false;
                btnLastPageAcc.IsEnabled = false;
                btnNextPageAcc.IsEnabled = false;
            });

        }

        #region [Hàm dùng chung và load dữ liệu]
        protected override bool OnBackButtonPressed()
        {
            if (flagShowPlace == 0)
            {
                return false;
            }
            else
                return true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (DeviceInfo.IsOrientationPortrait() && width > height || !DeviceInfo.IsOrientationPortrait() && width < height)
            {
                HeightListView = height/0.3;
                WidthListView = width;
            }

            if (DeviceInfo.IsOrientationPortrait() && width < height || !DeviceInfo.IsOrientationPortrait() && width > height)
            {
                HeightListView = height/ 0.3
                    
                    ;
                WidthListView = width;
            }
        }
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            //CheckBox isCheckedOrNot = (CheckBox)sender;
            //var selectedVehicleList = isCheckedOrNot.BindingContext as DataSearchModel;

            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.White;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;
                lastCell = viewCell;
            }
        }

        public async void GetAllUser()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetAllUser?page=" + PageAcc + "&pageSize=" + PageSizeAcc + "&username=" + UserNameSearch + "&fullname=" + FullName;
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);


                    client.DefaultRequestHeaders.Add("cookie", cookie);
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<DataUserModel>(result);

                    //var message = responseData.Content;
                    if (responseData != null && responseData.Result == true)
                    {
                        ListUser = new ObservableCollection<UserModel>(responseData.ListUser);
                        CountPageAcc = (int)responseData.CountPage;
                        lblCurentPageAcc.IsVisible = true;
                        CurentPageAcc = "Trang " + PageAcc + "/" + CountPageAcc;

                        if (CountPageAcc == 1)
                        {
                            btnBackPageAcc.IsEnabled = false;
                            btnFristPageAcc.IsEnabled = false;
                            btnLastPageAcc.IsEnabled = false;
                            btnNextPageAcc.IsEnabled = false;

                        }
                        if (CountPageAcc == 0)
                        {
                            btnBackPageAcc.IsEnabled = false;
                            btnFristPageAcc.IsEnabled = false;
                            btnLastPageAcc.IsEnabled = false;
                            btnNextPageAcc.IsEnabled = false;
                            lblCurentPageAcc.Text = "";
                            //CurentPageAcc = "Trang " + 0 + "/" + 0;
                        }
                        if (ListUser.Count == 0)
                        {
                            lblCurentPageAcc.IsVisible = false;
                        }
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                        btnBackPageAcc.IsEnabled = false;
                        btnFristPageAcc.IsEnabled = false;
                        btnLastPageAcc.IsEnabled = false;
                        btnNextPageAcc.IsEnabled = false;
                        lblCurentPageAcc.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                    btnBackPageAcc.IsEnabled = false;
                    btnFristPageAcc.IsEnabled = false;
                    btnLastPageAcc.IsEnabled = false;
                    btnNextPageAcc.IsEnabled = false;
                    lblCurentPageAcc.Text = "";
                }
            }

        }
        public async void GetPlaceOfUserMember()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);
                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + IdUserSelect + "&page=" + PageUser + "&pageSize=" + PageSizeUser + "&name=" + PlaceNameListPlace + "&ipaddress=" + IpAddressListPlace + "&typeplace=" + typePlaceListPlace + "&portapi=" + PortApiListPlace;


                    client.DefaultRequestHeaders.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                    //var message = responseData.Content;
                    if (responseData != null && responseData.Result == true)
                    {
                        ListPlaceOfUser = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                        //lstvPlace.ItemsSource = ListPlace;

                        //listDataPlace = responseData;
                        CountPageUser = (int)responseData.CountPage;
                        lblCurentPageUser.IsVisible = true;
                        lblCurentPageUser.Text = "Trang" + PageUser + "/" + CountPageUser;
                        if (CountPageUser == 1)
                        {
                            btnBackPageUser.IsEnabled = false;
                            btnFristPageUser.IsEnabled = false;
                            btnLastPageUser.IsEnabled = false;
                            btnNextPageUser.IsEnabled = false;
                        }

                        if (CountPageUser == 0)
                        {
                            btnBackPageUser.IsEnabled = false;
                            btnFristPageUser.IsEnabled = false;
                            btnLastPageUser.IsEnabled = false;
                            btnNextPageUser.IsEnabled = false;
                            lblCurentPageUser.Text = "";
                            //CurentPageUser = "Trang" + 0 + "/" + 0;
                        }
                        if (ListPlaceOfUser.Count == 0)
                        {
                            lblCurentPageUser.IsVisible = false;
                        }
                    }
                    else if (responseData == null || responseData.Result == false)
                    {
                        DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                        btnBackPageUser.IsEnabled = false;
                        btnFristPageUser.IsEnabled = false;
                        btnLastPageUser.IsEnabled = false;
                        btnNextPageUser.IsEnabled = false;
                        lblCurentPageUser.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                    btnBackPageUser.IsEnabled = false;
                    btnFristPageUser.IsEnabled = false;
                    btnLastPageUser.IsEnabled = false;
                    btnNextPageUser.IsEnabled = false;
                    lblCurentPageUser.Text = "";
                }
            }
        }

        public async void GetPlaceAddUserAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceAddUser?id_User=" + userSelect.Id + "&page=" + Page + "&pageSize=" + PageSize + "&name=" + PlaceName + "&ipaddress=" + IpAddress + "&typeplace=" + typePlace + "&portapi=" + PortApi;
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
                        var responseData = JsonConvert.DeserializeObject<DataPlaceModel>(result);
                        if (responseData != null && responseData.Result == true)
                        {

                            lblCurentPage.IsVisible = true;
                            ListPlaceAddUser = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                            CountPage = (int)responseData.CountPage;
                            CurentPage = "Trang " + Page + "/" + CountPage;
                            //pkrPlace.ItemsSource = lstPlaceAddUser;

                            if (CountPage == 1)
                            {
                                btnBackPage.IsEnabled = false;
                                btnFristPage.IsEnabled = false;
                                btnLastPage.IsEnabled = false;
                                btnNextPage.IsEnabled = false;
                            }


                            if (CountPage == 0)
                            {
                                btnBackPage.IsEnabled = false;
                                btnFristPage.IsEnabled = false;
                                btnLastPage.IsEnabled = false;
                                btnNextPage.IsEnabled = false;
                                lblCurentPage.IsVisible = false;
                                //CurentPage = "Trang " + 0 + "/" + 0;
                            }
                            if (ListPlaceAddUser.Count() == 0)
                            {
                                lblCurentPage.IsVisible = false;
                                var mes = "Dữ liệu rỗng!";
                                DependencyService.Get<IMessage>().LongTime(mes);
                            }
                            else
                            {
                                var mes = "Load dữ liệu thành công!";
                                DependencyService.Get<IMessage>().ShortTime(mes);
                                
                            }
                            
                        }
                        else
                        {
                            var mes = "Dữ liệu rỗng!";
                            DependencyService.Get<IMessage>().LongTime(mes);
                            btnBackPage.IsEnabled = false;
                            btnFristPage.IsEnabled = false;
                            btnLastPage.IsEnabled = false;
                            btnNextPage.IsEnabled = false;
                            lblCurentPage.Text = "";

                        }
                    }
                }
                catch (Exception ex)
                {
                    btnBackPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnNextPage.IsEnabled = false;
                    lblCurentPage.Text = "";
                    //DependencyService.Get<IMessage>().LongTime(Err);
                    var mes = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(mes);
                }
            }
        }
        #endregion

        #region [Account]
        private void addAccount_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage(userModel));
        }
        private async void btnDeleteAccount_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Thông báo", "Bạn có muốn xóa tài khoản này không?", "Đồng ý", "Hủy");
            if (answer == true)
            {
                var answer1 = await DisplayAlert("Thông báo", "Tài khoản sẽ bị xóa vĩnh viễn?", "Đồng ý", "Hủy");
                if (answer1 == true)
                {
                    var idUserBtnDel = (Xamarin.Forms.Button)sender;

                    InputDelUserModel inputDelUserModel = new InputDelUserModel();
                    inputDelUserModel.UserId = idUserBtnDel.CommandParameter.ToString();

                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/DelUser";

                    try
                    {
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var fileName = Path.Combine(documents, "cookie.txt");
                        var cookie = File.ReadAllText(fileName);

                        HttpClient client = new HttpClient();
                        string jsonData = JsonConvert.SerializeObject(inputDelUserModel);

                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        content.Headers.Add("cookie", cookie);
                        HttpResponseMessage response = await client.PostAsync(url, content);

                        string result = await response.Content.ReadAsStringAsync();

                        int responseData = Int32.Parse(result);


                        if (responseData == 0)
                        {
                            GetAllUser();
                            DependencyService.Get<IMessage>().ShortTime("Xóa thành công!");
                        }
                        else if (responseData == 1)
                        {
                            DependencyService.Get<IMessage>().LongTime("Lỗi khi nhận dữ liệu!");
                        }
                        else if (responseData == 2)
                        {
                            DependencyService.Get<IMessage>().LongTime("Tài khoản này không được xóa!");
                        }
                        else if (responseData == 3)
                        {
                            DependencyService.Get<IMessage>().LongTime("Lỗi khi xóa User!");
                        }
                        else if (responseData == 4)
                        {
                            DependencyService.Get<IMessage>().LongTime("Lỗi khi xóa Role!");
                        }
                        else if (responseData == 5)
                        {
                            DependencyService.Get<IMessage>().LongTime("Không tìm thấy tài khoản!");
                        }
                        else if (responseData == 6)
                        {
                            DependencyService.Get<IMessage>().LongTime("lỗi try catch exception!");
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongTime("Lỗi ngoại lệ!");
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
            else
                return;
        }

        private void btnShowPlaceUser_Clicked(object sender, EventArgs e)
        {

            flagShowPlace = 1;
            var idUserBtnShowPlace = (Xamarin.Forms.Button)sender;
            if (idUserBtnShowPlace != null)
            {
                IdUserSelect = idUserBtnShowPlace.CommandParameter.ToString();

                userSelect = new UserModel();
                userSelect = ListUser.FirstOrDefault(x => x.Id == IdUserSelect);

                var IsNameManager = userSelect.UserName;
                if (IsNameManager.ToLower() != "manager" && IsNameManager.ToLower() != "admin")
                {
                    pkrPageSizeUser.SelectedIndex = 1;
                    PageUser = 1;
                    CurentPageUser = "Trang " + PageUser + "/" + CountPageUser;
                    btnBackPageUser.IsEnabled = false;
                    btnFristPageUser.IsEnabled = false;
                    btnLastPageUser.IsEnabled = true;
                    btnNextPageUser.IsEnabled = true;
                    GetPlaceOfUserMember();
                    NavigationPage.SetHasNavigationBar(this, false);

                    grdIsBusy.IsVisible = true;
                    stlAddPlaceOfUser.IsVisible = true;
                    grdListPlaceUser.IsVisible = true;
                }
                else
                {
                    DisplayAlert("", "Không thực hiện chức năng này cho tài khoản này!", "OK");
                }
            }
            

        }
        #endregion

        #region [List Place Of Usermember]
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
                        pkrPageSizeUser.SelectedIndex = 0;
                        lblCurentPageUser.Text = "Trang" + PageUser + "/" + CountPageUser;
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
            if (userSelect != null)
            {
                pkrPageSize.SelectedIndex = 1;
                GetPlaceAddUserAsync();
                IdPlace = new List<string>();
                btnSelectPlace.Text = "-- Chọn cơ sở --";
                UserName = userSelect.UserName;
                grdIsBusy.IsVisible = true;
                stlAddPlaceOfUser.IsVisible = true;
                grdListPlaceUser.IsVisible = false;
                grdAddPlaceForUser.IsVisible = true;

            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            flagShowPlace = 0;
            NavigationPage.SetHasNavigationBar(this, true);
            grdIsBusy.IsVisible = false;
            stlAddPlaceOfUser.IsVisible = false;
            grdListPlaceUser.IsVisible = false;
        }

        #endregion

        #region [Add Place User Member]
        private async void btnAddPlaceForUser_Clicked(object sender, EventArgs e)
        {
            if (IdPlace == null || IdPlace.Count < 1)
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
                    var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                    if (responseData.Result == true)
                    {
                        var message = "Thêm thành công!";

                        DependencyService.Get<IMessage>().ShortTime(message);

                        grdAddPlaceForUser.IsVisible = false;
                        grdSelectPlaceForUser.IsVisible = false;
                        stlAddPlaceOfUser.IsVisible = true;
                        grdListPlaceUser.IsVisible = true;
                        pkrPageSizeUser.SelectedIndex = 0;
                        GetPlaceOfUserMember();
                        lblCurentPageUser.Text = "Trang" + PageUser + "/" + CountPageUser;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongTime(responseData.Content[0]);
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
            grdAddPlaceForUser.IsVisible = false;

            grdSelectPlaceForUser.IsVisible = false;
            stlAddPlaceOfUser.IsVisible = true;
            grdListPlaceUser.IsVisible = true;
        }

        private void btnSelectPlace_Clicked(object sender, EventArgs e)
        {
            IdPlace = new List<string>();
            grdSelectPlaceForUser.IsVisible = true;


            stlAddPlaceOfUser.IsVisible = true;

            grdListPlaceUser.IsVisible = false;
            grdAddPlaceForUser.IsVisible = false;

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


        #endregion

        #region [Select Place Of User]
        private void btnSelect_Clicked(object sender, EventArgs e)
        {
            if (ListPlaceAddUser != null)
            {
                foreach (var item in ListPlaceAddUser)
                {
                    if (item.IsSelected == true)
                    {
                        IdPlace.Add(item.Id);
                    }
                }
                btnSelectPlace.Text = IdPlace.Count() + " CƠ SỞ";


                grdSelectPlaceForUser.IsVisible = false;
                stlAddPlaceOfUser.IsVisible = true;
                grdListPlaceUser.IsVisible = false;
                grdAddPlaceForUser.IsVisible = true;
            }
            else
            {
                btnSelectPlace.Text = IdPlace.Count() + " CƠ SỞ";


                grdSelectPlaceForUser.IsVisible = false;
                stlAddPlaceOfUser.IsVisible = true;
                grdListPlaceUser.IsVisible = false;
                grdAddPlaceForUser.IsVisible = true;
            }
        }
        private void cbSelectPlace_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto)
                return;

            if (ListPlaceAddUser != null)
            {
                bool isCheckAll = true;
                foreach (var item in ListPlaceAddUser)
                {
                    if (item.IsSelected == false)
                    {
                        isCheckAll = false;
                        break;
                    }
                }

                if (cbSelectAllPlace.IsChecked != isCheckAll)
                {
                    isCheckByAuto = true;
                    cbSelectAllPlace.IsChecked = isCheckAll;
                }
            }
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
        private bool isCheckByAuto = false;
        private void cbSelectAllPlace_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto == false)
            {
                isCheckByAuto = true;
                if (ListPlaceAddUser != null)
                {
                    if (e.Value == true)
                    {

                        foreach (var item in ListPlaceAddUser)
                        {
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ListPlaceAddUser)
                        {
                            if (item.IsSelected == true)
                            {
                                item.IsSelected = false;
                            }
                        }
                    }
                }
            }

            isCheckByAuto = false;
        }

        #endregion

        #region [Phân trang]
        //AddPlaceUser
        private void btnFristPage_Clicked(object sender, EventArgs e)
        {
            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            Page = 1;
            GetPlaceAddUserAsync();
        }
        private void btnBackPage_Clicked(object sender, EventArgs e)
        {
            IdPlace = new List<string>();
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            if (Page > 0)
            {
                Page--;
                GetPlaceAddUserAsync();
            }
            if (Page == 1)
            {
                btnBackPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
            }


        }
        private void btnNextPage_Clicked(object sender, EventArgs e)
        {
            IdPlace = new List<string>();
            btnBackPage.IsEnabled = true;
            btnFristPage.IsEnabled = true;
            Page++;
            if (CountPage >= Page)
            {
                GetPlaceAddUserAsync();
            }
            if (CountPage == Page)
            {
                btnNextPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
            }

        }
        private void btnLastPage_Clicked(object sender, EventArgs e)
        {
            btnBackPage.IsEnabled = true;
            btnFristPage.IsEnabled = true;
            btnLastPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;
            Page = CountPage;
            GetPlaceAddUserAsync();
        }

        //Account
        private void btnFristPageAcc_Clicked(object sender, EventArgs e)
        {
            btnBackPageAcc.IsEnabled = false;
            btnFristPageAcc.IsEnabled = false;
            btnLastPageAcc.IsEnabled = true;
            btnNextPageAcc.IsEnabled = true;
            PageAcc = 1;
            GetAllUser();
        }

        private void btnBackPageAcc_Clicked(object sender, EventArgs e)
        {
            btnLastPageAcc.IsEnabled = true;
            btnNextPageAcc.IsEnabled = true;
            if (PageAcc > 0)
            {
                PageAcc--;
                GetAllUser();
            }
            if (PageAcc == 1)
            {
                btnBackPageAcc.IsEnabled = false;
                btnFristPageAcc.IsEnabled = false;
            }

        }

        private void btnNextPageAcc_Clicked(object sender, EventArgs e)
        {
            btnBackPageAcc.IsEnabled = true;
            btnFristPageAcc.IsEnabled = true;
            PageAcc++;
            if (CountPageAcc >= PageAcc)
            {
                GetAllUser();
            }
            if (CountPageAcc == PageAcc)
            {
                btnNextPageAcc.IsEnabled = false;
                btnLastPageAcc.IsEnabled = false;
            }
        }

        private void btnLastPageAcc_Clicked(object sender, EventArgs e)
        {
            btnBackPageAcc.IsEnabled = true;
            btnFristPageAcc.IsEnabled = true;
            btnLastPageAcc.IsEnabled = false;
            btnNextPageAcc.IsEnabled = false;
            PageAcc = CountPageAcc;
            GetAllUser();
        }


        //ListPlaceOfUser
        private void btnFristPageUser_Clicked(object sender, EventArgs e)
        {
            btnBackPageUser.IsEnabled = false;
            btnFristPageUser.IsEnabled = false;
            btnLastPageUser.IsEnabled = true;
            btnNextPageUser.IsEnabled = true;
            PageUser = 1;
            GetPlaceOfUserMember();
        }

        private void btnBackPageUser_Clicked(object sender, EventArgs e)
        {
            btnLastPageUser.IsEnabled = true;
            btnNextPageUser.IsEnabled = true;
            if (PageUser > 0)
            {
                PageUser--;
                GetPlaceOfUserMember();
            }
            if (PageUser == 1)
            {
                btnBackPageUser.IsEnabled = false;
                btnFristPageUser.IsEnabled = false;
            }
        }

        private void btnNextPageUser_Clicked(object sender, EventArgs e)
        {
            btnBackPageUser.IsEnabled = true;
            btnFristPageUser.IsEnabled = true;
            PageUser++;
            if (CountPageUser >= PageUser)
            {
                GetPlaceOfUserMember();
            }
            if (CountPageUser == PageUser)
            {
                btnNextPageUser.IsEnabled = false;
                btnLastPageUser.IsEnabled = false;
            }
        }

        private void btnLastPageUser_Clicked(object sender, EventArgs e)
        {
            btnBackPageUser.IsEnabled = true;
            btnFristPageUser.IsEnabled = true;
            btnLastPageUser.IsEnabled = false;
            btnNextPageUser.IsEnabled = false;
            PageUser = CountPageUser;
            GetPlaceOfUserMember();
        }
        #endregion

        #region [PageSize]
        //Place select Place
        private void pkrPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPlaceAddUser != null)
            {
                if (PageSize != null)
                {
                    if (PageSize == "All")
                    {
                        Page = 1;
                        GetPlaceAddUserAsync();
                    }
                    else
                    {
                        Page = 1;
                        GetPlaceAddUserAsync();
                        btnBackPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = true;
                        btnNextPage.IsEnabled = true;
                    }
                }
            }

        }
        //Account
        private void pkrPageSizeAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListUser != null)
            {
                if (PageSizeAcc != null)
                {
                    if (pkrPageSizeAcc.SelectedIndex == 0)
                    {
                        PageAcc = 1;
                        GetAllUser();
                    }
                    else
                    {
                        PageAcc = 1;
                        GetAllUser();
                        btnBackPageAcc.IsEnabled = false;
                        btnFristPageAcc.IsEnabled = false;
                        btnLastPageAcc.IsEnabled = true;
                        btnNextPageAcc.IsEnabled = true;
                    }
                }
            }
        }
        //Place user
        private void pkrPageSizeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPlaceOfUser != null)
            {
                if (PageSizeUser != null)
                {
                    if (PageSizeUser == "All")
                    {
                        PageUser = 1;
                        GetPlaceOfUserMember();
                    }
                    else
                    {
                        PageUser = 1;
                        GetPlaceOfUserMember();
                        btnBackPageUser.IsEnabled = false;
                        btnFristPageUser.IsEnabled = false;
                        btnLastPageUser.IsEnabled = true;
                        btnNextPageUser.IsEnabled = true;
                    }
                }
            }
        }
        #endregion

        #region [Search]

        //Search account
        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            GetAllUser();
            pkrPageSizeAcc.SelectedIndex = 0;
            btnUnfold.ImageSource = "up.png";
            btnUnfold.Text = "MỞ THÔNG TIN TÌM KIẾM";
            grdSearchInformation.IsVisible = false;
            await PopupNavigation.Instance.PopAsync();
        }
        private void btnUnfold_Clicked(object sender, EventArgs e)
        {
            if (grdSearchInformation.IsVisible == false)
            {
                btnUnfold.ImageSource = "dow.png";
                btnUnfold.Text = "ĐÓNG THÔNG TIN TÌM KIẾM";
                grdSearchInformation.IsVisible = true;

                return;

            }

            if (grdSearchInformation.IsVisible == true)
            {
                btnUnfold.ImageSource = "up.png";
                btnUnfold.Text = "MỞ THÔNG TIN TÌM KIẾM";
                grdSearchInformation.IsVisible = false;

                return;
            }
        }

        //Search place add user
        private void lblClickInf_Tapped(object sender, EventArgs e)
        {
            if (grdSearchInformationPlaceAddUser.IsVisible == false)
            {

                lblClickInf.Text = "CLICK ĐÓNG THÔNG TIN TÌM KIẾM";
                grdSearchInformationPlaceAddUser.IsVisible = true;
                return;
            }

            if (grdSearchInformationPlaceAddUser.IsVisible == true)
            {
                lblClickInf.Text = "CLICK TÌM KIẾM";
                grdSearchInformationPlaceAddUser.IsVisible = false;
                return;
            }
        }


        private async void btnSearchPlaceAddUser_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            GetPlaceAddUserAsync();
            pkrPageSize.SelectedIndex = 0;
            lblClickInf.Text = "CLICK TÌM KIẾM";
            grdSearchInformationPlaceAddUser.IsVisible = false;
            await PopupNavigation.Instance.PopAsync();
        }

        private void pkrTypePlace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrTypePlace.SelectedIndex == -1)
            {
                typePlace = null;
            }
            else
            {
                typePlace = pkrTypePlace.SelectedIndex.ToString();
            }
        }

        //Search list place of user
        private void lblClickInfListPlace_Tapped(object sender, EventArgs e)
        {
            if (grdSearchInformationListPlace.IsVisible == false)
            {

                lblClickInfListPlace.Text = "CLICK ĐÓNG THÔNG TIN TÌM KIẾM";
                grdSearchInformationListPlace.IsVisible = true;
                return;
            }

            if (grdSearchInformationListPlace.IsVisible == true)
            {
                lblClickInfListPlace.Text = "CLICK TÌM KIẾM";
                grdSearchInformationListPlace.IsVisible = false;
                return;
            }
        }

        private void pkrTypePlaceListPlace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrTypePlaceListPlace.SelectedIndex == -1)
            {
                typePlaceListPlace = null;
            }
            else
            {
                typePlaceListPlace = pkrTypePlaceListPlace.SelectedIndex.ToString();
            }
        }
        private async void btnSearchListPlace_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            GetPlaceOfUserMember();
            pkrPageSizeUser.SelectedIndex = 0;
            lblClickInfListPlace.Text = "CLICK TÌM KIẾM";
            grdSearchInformationListPlace.IsVisible = false;
            await PopupNavigation.Instance.PopAsync();
        }

        #endregion


    }
}