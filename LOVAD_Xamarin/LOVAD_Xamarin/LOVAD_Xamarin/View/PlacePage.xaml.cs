using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
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
    public partial class PlacePage : ContentPage
    {
       

        UserModel userProfileModel = new UserModel();
        #region [Place]

        private ObservableCollection<PlaceModel> _listplace;
        public ObservableCollection<PlaceModel> ListPlace { get { return _listplace; } set { _listplace = value; OnPropertyChanged(); } }

        private string _pageSize;
        public string PageSize { get { return _pageSize; } set { _pageSize = value; OnPropertyChanged("PageSize"); } }

        private int _page;
        public int Page { get { return _page; } set { _page = value; OnPropertyChanged("Page"); } }

        private string _curentPage;
        public string CurentPage { get { return _curentPage; } set { _curentPage = value; OnPropertyChanged("CurentPage"); } }

        private int _countPage;
        public int CountPage { get { return _countPage; } set { _countPage = value; OnPropertyChanged("CountPage"); } }

        #endregion

        #region [Search]
        private string _placeNameSearch;
        public string PlaceNameSearch { get { return _placeNameSearch; } set { _placeNameSearch = value; OnPropertyChanged("PlaceNameSearch"); } }
       
        private string _portApiSearch;
        public string PortApiSearch { get { return _portApiSearch; } set { _portApiSearch = value; OnPropertyChanged("PortApiSearch"); } }
       
        private string _ipAddressSearch;
        public string IpAddressSearch { get { return _ipAddressSearch; } set { _ipAddressSearch = value; OnPropertyChanged("IpAddressSearch"); } }

        private string typePlace;// loại cơ sở chọn khi search
        #endregion

        public PlacePage(UserModel user)
        {
            InitializeComponent();
            BindingContext = this;
            userProfileModel = user;
            Page = 1;
            pkrPageSize.SelectedIndex = 1;

            MessagingCenter.Subscribe<AddPlacePage>(this, "Update", (sender) =>
            {
                GetAllPlace();
                pkrPageSize.SelectedIndex = 0;
            });
            GetAllPlace();
            lblCurentPage.Text = "Trang" + Page + "/" + 1;
            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;


        }
        #region [Hàm load dữ liệu]


        public async void GetAllPlace()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = Path.Combine(documents, "cookie.txt");
                    var cookie = File.ReadAllText(fileName);
                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetAllPlace?page=" + Page + "&pageSize=" + PageSize + "&name=" + PlaceNameSearch + "&ipaddress=" + IpAddressSearch + "&typeplace=" + typePlace + "&portapi=" + PortApiSearch;

                    client.DefaultRequestHeaders.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                    //var message = responseData.Content;
                    if (responseData != null && responseData.Result == true)
                    {
                        ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                        //lstvPlace.ItemsSource = ListPlace;

                        //listDataPlace = responseData;
                        CountPage = (int)responseData.CountPage;
                        lblCurentPage.Text = "Trang" + Page + "/" + CountPage;
                        lblCurentPage.IsVisible = true;
                        if (CountPage == 1)
                        {
                            btnBackPage.IsEnabled = false;
                            btnFristPage.IsEnabled = false;
                            btnLastPage.IsEnabled = false;
                            btnNextPage.IsEnabled = false;
                            lblCurentPage.Text = "Trang" + Page + "/" + CountPage;
                        }
                        if (CountPage == 0)
                        {
                            btnBackPage.IsEnabled = false;
                            btnFristPage.IsEnabled = false;
                            btnLastPage.IsEnabled = false;
                            btnNextPage.IsEnabled = false;
                            lblCurentPage.Text = "";
                        }
                        if (ListPlace.Count == 0)
                        {
                            lblCurentPage.IsVisible = false;
                        }

                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                        btnBackPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = false;
                        btnNextPage.IsEnabled = false;
                        lblCurentPage.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                    btnBackPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnNextPage.IsEnabled = false;
                    lblCurentPage.Text = "";
                }
            }

        }
        #endregion

        #region [Chức năng CRUD]
        private void addPlace_Clicked(object sender, EventArgs e)
        {
            PlaceModel placeModel = new PlaceModel();
            string flag = "AddPlace";
            Navigation.PushAsync(new AddPlacePage(placeModel, userProfileModel, flag));
        }

        private async void btnDelPlace_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Thông báo", "Bạn có muốn xóa cơ sở này không?", "Đồng ý", "Hủy");
            if (answer == true)
            {
                var answer1 = await DisplayAlert("Thông báo", "Cơ sở sẽ bị xóa vĩnh viễn?", "Đồng ý", "Hủy");
                if (answer1 == true)
                {
                    var idPlaceBtnDel = (Xamarin.Forms.Button)sender;

                    PlaceModel placeModel = new PlaceModel();
                    placeModel.Id = idPlaceBtnDel.CommandParameter.ToString();

                    string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/DelPlace";

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

                        int responseData = Int32.Parse(result);


                        if (responseData == 0)
                        {
                            DependencyService.Get<IMessage>().ShortTime("Xóa thành công!");
                            pkrPageSize.SelectedIndex = 0;
                            GetAllPlace();
                        }
                        else if (responseData == 1)
                        {
                            DependencyService.Get<IMessage>().LongTime("Không tìm thấy cơ sở!");
                        }
                        else if (responseData == 2)
                        {
                            DependencyService.Get<IMessage>().LongTime("Lỗi khi nhận dữ liệu!");
                        }
                        else if (responseData == 3)
                        {
                            DependencyService.Get<IMessage>().LongTime("Lỗi khi xóa!");
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

        private void btnEditPlace_Clicked(object sender, EventArgs e)
        {
            var idUser = (Xamarin.Forms.Button)sender;
            var UserId = idUser.CommandParameter.ToString();

            PlaceModel placeSelect = new PlaceModel();
            placeSelect = ListPlace.FirstOrDefault(x => x.Id == UserId);
            string flag = "EditPlace";
            Navigation.PushAsync(new AddPlacePage(placeSelect, userProfileModel, flag));
        }
        #endregion

        #region [Phân trang]
        private void pkrPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPlace != null)
            {

                if (PageSize != null)
                {
                    if (PageSize == "All")
                    {
                        Page = 1;
                        GetAllPlace();
                    }
                    else
                    {
                        Page = 1;
                        GetAllPlace();
                        btnBackPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = true;
                        btnNextPage.IsEnabled = true;
                    }
                }
            }
        }
        private async void btnFristPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            Page = 1;
            GetAllPlace();
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnBackPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            if (Page > 0)
            {
                Page--;
                GetAllPlace();
            }
            if (Page == 1)
            {
                btnBackPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
            }
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnNextPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            btnBackPage.IsEnabled = true;
            btnFristPage.IsEnabled = true;
            Page++;
            if (CountPage >= Page)
            {
                GetAllPlace();
            }
            if (CountPage == Page)
            {
                btnNextPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
            }
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnLastPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            btnBackPage.IsEnabled = true;
            btnFristPage.IsEnabled = true;
            btnLastPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;
            Page = CountPage;
            GetAllPlace();
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion

        #region [Search]

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            GetAllPlace();
            btnUnfold.ImageSource = "up.png";
            btnUnfold.Text = "MỞ THÔNG TIN TÌM KIẾM";
            grdSearchInformation.IsVisible = false;
            pkrPageSize.SelectedIndex = 0;
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

        private void pkrTypePlaceSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrTypePlaceSearch.SelectedIndex == -1)
            {
                typePlace = null;
            }
            else
            {
                typePlace = pkrTypePlaceSearch.SelectedIndex.ToString();
            }
        }
       
        #endregion

    }

    
}