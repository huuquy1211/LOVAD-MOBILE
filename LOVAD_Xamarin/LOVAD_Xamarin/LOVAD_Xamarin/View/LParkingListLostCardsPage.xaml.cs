using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
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
    public partial class LParkingListLostCardsPage : ContentPage
    {
        private string _startDate;
        public string startDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("startDate"); } }

        private string _endDate;
        public string endDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("endDate"); } }


        private string _inVehicleTypeList;//Loại đối tượng
        public string inVehicleTypeList { get { return _inVehicleTypeList; } set { _inVehicleTypeList = value; OnPropertyChanged("inVehicleTypeList"); } }


        private string _cartType;//Loại thẻ
        public string CartType { get { return _cartType; } set { _cartType = value; OnPropertyChanged("CartType"); } }

        private string _searchField;//Thông tin tìm kiếm
        public string searchField { get { return _searchField; } set { _searchField = value; OnPropertyChanged("searchField"); } }

        private string _searchContent;//Nội dung tìm kiếm
        public string searchContent { get { return _searchContent; } set { _searchContent = value; OnPropertyChanged("searchContent"); } }

        private string _pageSize;
        public string PageSize { get { return _pageSize; } set { _pageSize = value; OnPropertyChanged("PageSize"); } }


        private ObservableCollection<DataSearchModel> _vehicleList;//Loại đối tượng
        public ObservableCollection<DataSearchModel> VehicleList { get { return _vehicleList; } set { _vehicleList = value; OnPropertyChanged("VehicleList"); } }


        private ObservableCollection<DataSearchModel> _searchFieldList;//Thông tin tìm kiếm
        public ObservableCollection<DataSearchModel> SearchFieldList { get { return _searchFieldList; } set { _searchFieldList = value; OnPropertyChanged("SearchFieldList"); } }

        private ObservableCollection<DataSearchModel> _cartTypeList;//Thông tin tìm kiếm
        public ObservableCollection<DataSearchModel> CartTypeList { get { return _cartTypeList; } set { _cartTypeList = value; OnPropertyChanged("CartTypeList"); } }

        public Dictionary<string, string> values;

        private ObservableCollection<ValueLParkingModel> _valList;//Danh sách data
        public ObservableCollection<ValueLParkingModel> ValList { get { return _valList; } set { _valList = value; OnPropertyChanged("ValList"); } }

        public List<string> NameVehicleList;
        public LParkingListLostCardsPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetDataLParkingInAndOut();
        }
        #region [Data Main Lparking Page]
        public void GetDataLParkingInAndOut()
        {

            //CurentPage = 1;
            #region [vehicleTypeListName]
            LoadInVehicleTypeList();
            pkrVehicle.SelectedIndex = 0;
            #endregion

            #region [CartType]
            LoadSearchCartTypeListt();
            pkrCartType.SelectedIndex = 0;
            #endregion

            #region [SearchField]
            LoadSearchFieldList();
            pkrSearchField.SelectedIndex = 0;
            #endregion

            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ValList = new ObservableCollection<ValueLParkingModel>(responseData.data);
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(message);
                }
            }
            else
            {
                var message = "Dữ liệu rỗng!";
                DependencyService.Get<IMessage>().LongTime(message);
            }

        }
        private void btnSearch_Clicked(object sender, EventArgs e)
        {
            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ValList = new ObservableCollection<ValueLParkingModel>(responseData.data);
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(message);
                }
            }
            else
            {
                var message = "Dữ liệu rỗng!";
                DependencyService.Get<IMessage>().LongTime(message);
            }
        }
        public static string SendHttpPostRequest(String requestUrl, Dictionary<string, string> values, int timeOut /*milliseconds*/)
        {
            try
            {
                string postData = string.Empty; //format key1=value1&key2=value2
                bool isTheFirst = true;
                foreach (var item in values)
                {
                    if (isTheFirst == true)
                    {
                        postData += string.Format("{0}={1}", item.Key, item.Value);
                        isTheFirst = false;
                    }
                    else
                    {
                        postData += string.Format("&{0}={1}", item.Key, item.Value);
                    }
                }
                var data = Encoding.UTF8.GetBytes(postData);
                WebRequest request = WebRequest.Create(requestUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Timeout = timeOut;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                var message = "Không kết nối được sever!";
                DependencyService.Get<IMessage>().LongTime(message);
                return null;
            }
        }

        public void AddDataValues()
        {
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");

            values = new Dictionary<string, string>();
            values.Add("start", "");
            values.Add("length", PageSize);

            values.Add("startDate", startDate);
            values.Add("endDate", endDate);

           
            values.Add("searchContent", searchContent);// noi dung tim kiem
            values.Add("typeVehicle", inVehicleTypeList);
            values.Add("typeCard", CartType);
            values.Add("cardCode", null);
            values.Add("customerCode", null);
            values.Add("customerName", null);
            values.Add("customerTel", null);
            values.Add("vehicleNumberPlate", null);
        }
        #endregion

        #region [Load Data Search]
        //Danh sách Loại đối tượng
        public void LoadInVehicleTypeList()
        {
            Dictionary<string, string> values = new Dictionary<string, string>(1);
            values.Add("", "");

            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/load-vehicle-list";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<List<DataSearchModel>>(result);
                if (responseData != null)
                {
                    //NameVehicleList = new List<string>();
                    //NameVehicleList.Add("All");
                    //foreach (var item in responseData)
                    //{
                    //    NameVehicleList.Add(item.Name);
                    //}
                    //VehicleList = new ObservableCollection<DataSearchModel>(NameVehicleList);

                    VehicleList = new ObservableCollection<DataSearchModel>();
                    VehicleList.Add(new DataSearchModel() { Name = "All" });

                    foreach (var item in responseData)
                    {
                        VehicleList.Add(item);
                    }
                }
                else
                {

                    var message = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(message);
                }
            }
            else
            {
                var message = "Dữ liệu rỗng!";
                DependencyService.Get<IMessage>().LongTime(message);
            }
        }

        //Danh sách Thông tin tìm kiếm
        public void LoadSearchFieldList()
        {
            List<DataSearchModel> ListSearchField = new List<DataSearchModel>();
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "1", NameSearchField = "1-Mã thẻ" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "2", NameSearchField = "2-Mã khách hàng" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "3", NameSearchField = "3-Họ tên" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "4", NameSearchField = "4-Số điện thoại" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "5", NameSearchField = "5-Biển số xe" });
            SearchFieldList = new ObservableCollection<DataSearchModel>(ListSearchField);
        }

        public void LoadSearchCartTypeListt()
        {
            List<DataSearchModel> ListCartType = new List<DataSearchModel>();
            ListCartType.Add(new DataSearchModel() { IdCartType = "1", NameCartType = "Tất cả" });
            ListCartType.Add(new DataSearchModel() { IdCartType = "2", NameCartType = "Thẻ ngày" });
            ListCartType.Add(new DataSearchModel() { IdCartType = "3", NameCartType = "Thẻ khách hàng" });
            ListCartType.Add(new DataSearchModel() { IdCartType = "4", NameCartType = "Thẻ đăng nhập" });
            ListCartType.Add(new DataSearchModel() { IdCartType = "5", NameCartType = "Thẻ xóa" });
            CartTypeList = new ObservableCollection<DataSearchModel>(ListCartType);
        }
        #endregion
        private void btnUnfold_Clicked(object sender, EventArgs e)
        {
            if (grdSearchInformation.IsVisible == false)
            {
                btnUnfold.ImageSource = "dow.png";
                btnUnfold.Text = "ĐÓNG THÔNG TIN";
                grdSearchInformation.IsVisible = true;

                return;

            }

            if (grdSearchInformation.IsVisible == true)
            {
                btnUnfold.ImageSource = "up.png";
                btnUnfold.Text = "MỞ THÔNG TIN";
                grdSearchInformation.IsVisible = false;

                return;
            }
        }

        private void pkrSearchField_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var item = (DataSearchModel)pkrSearchField.SelectedItem;
            //searchField = item.IdSearchField;

            searchField = pkrSearchField.SelectedIndex.ToString();
        }


        private void pkrVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (DataSearchModel)pkrVehicle.SelectedItem;
            if (item.Name.Equals("All"))
            {
                inVehicleTypeList = "-1";
            }
            else
                inVehicleTypeList = pkrVehicle.SelectedIndex.ToString();
        }

        private void pkrCartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var item = (DataSearchModel)pkrCartType.SelectedItem;
            //CartType = item.IdCartType;

            CartType = pkrCartType.SelectedIndex.ToString();
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
    }
}