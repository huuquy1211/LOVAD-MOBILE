using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceTramCanPage : ContentPage
    {
        UserModel userProfileModel = new UserModel();
        private bool isMasterDetail;

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
        public PlaceTramCanPage(UserModel user, bool IsMasterDetail)
        {
            InitializeComponent();
            userProfileModel = user;
            BindingContext = this;
           
            NavigationPage.SetHasNavigationBar(this, false);
            Page = 1;
            pkrPageSize.SelectedIndex = 1;
            GetPlaceOfUserMember();
            CurentPage = "Trang" + Page + "/" + 1;
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
        public async void GetPlaceOfUserMember()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Path.Combine(documents, "cookie.txt");
            var cookie = File.ReadAllText(fileName);
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + userProfileModel.Id + "&page=" + Page + "&pageSize=" + PageSize;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", cookie);
                HttpResponseMessage response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                //var message = responseData.Content;
                if (responseData != null && responseData.Result == true && responseData.LstPlaces.Count > 0)
                {
                    ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);

                    //foreach (var item in responseData)
                    //    if (item.TypePlace == 0)
                    //        lstPlaceLparking.Add(item);
                    //lstvLparking.ItemsSource = lstPlaceLparking;

                    CountPage = (int)responseData.CountPage;
                    CurentPage = "Trang" + Page + "/" + CountPage;
                }
                else if (responseData == null && responseData.Result == false && responseData.LstPlaces.Count <= 0)
                {
                    DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }

        private void frmSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keySearch = frmSearch.Text;
            lstvPlaceTranCan.ItemsSource = ListPlace.Where(x => x.Name.ToLower().Contains(keySearch.ToLower()));
        }

        private async void lstvPlaceTranCan_ItemTapped(object sender, ItemTappedEventArgs e)
        {
           
            var PlaceSelect = (PlaceModel)lstvPlaceTranCan.SelectedItem;
            await Navigation.PushAsync(new TramCanPage(userProfileModel));
        }

        private void pkrPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPlace != null)
            {
                GetPlaceOfUserMember();
            }
        }

        #region [Phân trang]
        private async void btnFristPage_Clicked(object sender, EventArgs e)
        {
            btnBackPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            Page = 1;
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Path.Combine(documents, "cookie.txt");
            var cookie = File.ReadAllText(fileName);
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + userProfileModel.Id + "&page=" + Page + "&pageSize=" + PageSize;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", cookie);
                HttpResponseMessage response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                //var message = responseData.Content;
                if (responseData != null && responseData.Result == true && responseData.LstPlaces.Count > 0)
                {
                    ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                    //lstvPlace.ItemsSource = ListPlace;

                    //listDataPlace = responseData;
                    CountPage = (int)responseData.CountPage;
                    CurentPage = "Trang" + Page + "/" + CountPage;
                }
                else if (responseData == null && responseData.Result == false && responseData.LstPlaces.Count <= 0)
                {
                    DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }

        private async void btnBackPage_Clicked(object sender, EventArgs e)
        {
            btnLastPage.IsEnabled = true;
            btnNextPage.IsEnabled = true;
            if (Page > 0)
            {
                Page--;
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var fileName = Path.Combine(documents, "cookie.txt");
                var cookie = File.ReadAllText(fileName);
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + userProfileModel.Id + "&page=" + Page + "&pageSize=" + PageSize;
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                    //var message = responseData.Content;
                    if (responseData != null && responseData.Result == true && responseData.LstPlaces.Count > 0)
                    {
                        ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                        //lstvPlace.ItemsSource = ListPlace;

                        //listDataPlace = responseData;
                        CountPage = (int)responseData.CountPage;
                        CurentPage = "Trang" + Page + "/" + CountPage;
                    }
                    else if (responseData == null && responseData.Result == false && responseData.LstPlaces.Count <= 0)
                    {
                        DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            if (Page == 1)
            {
                btnBackPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
            }
        }

        private async void btnNextPage_Clicked(object sender, EventArgs e)
        {

            btnBackPage.IsEnabled = true;
            btnFristPage.IsEnabled = true;
            Page++;
            if (CountPage >= Page)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var fileName = Path.Combine(documents, "cookie.txt");
                var cookie = File.ReadAllText(fileName);
                string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + userProfileModel.Id + "&page=" + Page + "&pageSize=" + PageSize;
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Cookie", cookie);
                    HttpResponseMessage response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                    //var message = responseData.Content;
                    if (responseData != null && responseData.Result == true && responseData.LstPlaces.Count > 0)
                    {
                        ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                        //lstvPlace.ItemsSource = ListPlace;

                        //listDataPlace = responseData;
                        CountPage = (int)responseData.CountPage;
                        CurentPage = "Trang" + Page + "/" + CountPage;
                    }
                    else if (responseData == null && responseData.Result == false && responseData.LstPlaces.Count <= 0)
                    {
                        DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                    }
                }
                catch (Exception ex)
                {
                    var Err = "Không nết nối được máy chủ";
                    DependencyService.Get<IMessage>().LongTime(Err);
                }
            }
            if (CountPage == Page)
            {
                btnNextPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
            }
        }

        private async void btnLastPage_Clicked(object sender, EventArgs e)
        {
            btnBackPage.IsEnabled = true;
            btnFristPage.IsEnabled = true;
            btnLastPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;
            Page = CountPage;
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Path.Combine(documents, "cookie.txt");
            var cookie = File.ReadAllText(fileName);
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetPlaceOfUserMember?UserId=" + userProfileModel.Id + "&page=" + Page + "&pageSize=" + PageSize;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", cookie);
                HttpResponseMessage response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<RespondPlaceModel>(result);

                //var message = responseData.Content;
                if (responseData != null && responseData.Result == true && responseData.LstPlaces.Count > 0)
                {
                    ListPlace = new ObservableCollection<PlaceModel>(responseData.LstPlaces);
                    //lstvPlace.ItemsSource = ListPlace;

                    //listDataPlace = responseData;
                    CountPage = (int)responseData.CountPage;
                    CurentPage = "Trang" + Page + "/" + CountPage;
                }
                else if (responseData == null && responseData.Result == false && responseData.LstPlaces.Count <= 0)
                {
                    DependencyService.Get<IMessage>().LongTime("Dữ liệu rỗng!");
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }
        #endregion

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(userProfileModel));
        }

    }
}