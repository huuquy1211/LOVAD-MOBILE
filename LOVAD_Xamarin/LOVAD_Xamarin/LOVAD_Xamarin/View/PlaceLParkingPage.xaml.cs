using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceLParkingPage : ContentPage
    {
        UserModel userProfileModel;
        private bool isMasterDetail;
        private string typePlace;//Loại cơ sở chọn khi search

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

        private string typePlaceSearch;// loại cơ sở chọn khi search
        #endregion
        public PlaceLParkingPage(UserModel user, bool IsMasterDetail)
        {
            InitializeComponent();
            userProfileModel = user;
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
            Page = 1;
            pkrPageSize.SelectedIndex = 1;

            CurentPage = "Trang " + Page + "/" + 1;
            GetPlaceToType();
            isMasterDetail = IsMasterDetail;


            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Navigation.PushAsync(new MainPage(userProfileModel));
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isMasterDetail == true)
            {
                Global.Intance.masterDetailPage.IsGestureEnabled = false;
                //(Application.Current.MainPage as LParkingPage).IsGestureEnabled = false;
            }
        }
        public async void GetPlaceToType()
        {
            using (HttpClient client = new HttpClient())
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var fileName = Path.Combine(documents, "cookie.txt");
                var cookie = File.ReadAllText(fileName);
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceToType?typePlace=" + 0 + "&page=" + Page + "&pageSize=" + PageSize + "&name=" + PlaceNameSearch + "&ipaddress=" + IpAddressSearch + "&typeplace=" + typePlaceSearch + "&portapi=" + PortApiSearch;
                try
                {
                    client.DefaultRequestHeaders.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                    //var message = responseData.Content;
                    if (responseData != null && responseData.Result == true)
                    {
                        ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);

                        CountPage = (int)responseData.CountPage;
                        CurentPage = "Trang " + Page + "/" + CountPage;

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
                            lblCurentPage.Text = "";
                            //CurentPage = "Trang " + 0 + "/" + 0;
                        }

                    }
                    else if (responseData == null && responseData.Result == false)
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

        private async void lstvLparking_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("lparking"));
            lstvLparking.IsTabStop = true;
            var PlaceSelect = (PlaceModel)lstvLparking.SelectedItem;
            await Navigation.PushAsync(new LParkingPage(userProfileModel, PlaceSelect));
            await PopupNavigation.Instance.PopAllAsync();
        }

        private void pkrPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPlace != null)
            {
                if (PageSize != null)
                {
                    if (PageSize == "All")
                    {
                        Page = 1;
                        GetPlaceToType();
                    }
                    else
                    {
                        Page = 1;
                        btnBackPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = true;
                        btnNextPage.IsEnabled = true;
                        GetPlaceToType();
                    }
                }
            }
        }

        #region [Phân trang]
        private async void btnFristPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            
            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            Page = 1;
            GetPlaceToType();
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
                GetPlaceToType();
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
                GetPlaceToType();
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
            GetPlaceToType();
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(userProfileModel));
        }

        #region [Search]
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
                typePlaceSearch = null;
            }
            else
            {
                typePlaceSearch = pkrTypePlaceSearch.SelectedIndex.ToString();
            }

        }

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            GetPlaceToType();
            btnUnfold.ImageSource = "up.png";
            btnUnfold.Text = "MỞ THÔNG TIN TÌM KIẾM";
            grdSearchInformation.IsVisible = false;
            pkrPageSize.SelectedIndex = 0;
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion

    }
}