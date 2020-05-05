using LOVAD_Xamarin.Model;
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
    public partial class LParkingReportRevenuePage : ContentPage
    {

        //Ngày ra vào
        private string _startDate;
        public string startDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("startDate"); } }

        private string _endDate;
        public string endDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("endDate"); } }

        //Danh sách tên mã trạm
        private string _inStationCodeListName; //Chọn hết => All, ngược lại Empty.
        public string inStationCodeListName { get { return _inStationCodeListName; } set { _inStationCodeListName = value; OnPropertyChanged("inStationCodeListName"); } }

        //Danh sách mã trạm
        private string _inStationCodeList;//Tên cách nhau dấu , nếu chọn hết => Empty
        public string inStationCodeList { get { return _inStationCodeList; } set { _inStationCodeList = value; OnPropertyChanged("inStationCodeList"); } }

        //Loại dữ liệu
        private string _inVehicleDataType; //Chọn index
        public string inVehicleDataType { get { return _inVehicleDataType; } set { _inVehicleDataType = value; OnPropertyChanged("inVehicleDataType"); } }

        //Tổng doanh thu thống kê
        private int _totalOut; //Tổng lượng ra
        public int TotalOut { get { return _totalOut; } set { _totalOut = value; OnPropertyChanged("TotalOut"); } }

        private int _totalIn; //Tổng lượng vào
        public int TotalIn { get { return _totalIn; } set { _totalIn = value; OnPropertyChanged("TotalIn"); } }

        private double _totalPrice; //Tổng tiền
        public double TotalPrice { get { return _totalPrice; } set { _totalPrice = value; OnPropertyChanged("TotalPrice"); } }


        private ObservableCollection<ValueReportLParkingModel> _dataLParkingReport;
        public ObservableCollection<ValueReportLParkingModel> DataLParkingReport { get { return _dataLParkingReport; } set { _dataLParkingReport = value; OnPropertyChanged("DataLParkingReport"); } }

        private ObservableCollection<ValueReportLParkingModel> _stationCodeList;
        public ObservableCollection<ValueReportLParkingModel> StationCodeList { get { return _stationCodeList; } set { _stationCodeList = value; OnPropertyChanged("StationCodeList"); } }

        public Dictionary<string, string> values;//Add dữ liệu truyền đi
        public ViewCell lastCell;
        private bool isCheckByAuto = false;//Tích chọn auto
        private List<string> SelectStationCodeList; //Danh sách các StationCode được chọn
        public LParkingReportRevenuePage()
        {
            InitializeComponent();
            BindingContext = this;

            //Khởi tạo các biến khi khởi động
            pkrVehicleDataType.SelectedIndex = 0;
            inStationCodeList = "";
            inStationCodeListName = "All";
            btnStationCodeList.Text = "Tất cả";

            //Load dữ liệu report
            GetDataLParkingReport();

            //Load dữ liệu của máy trạm
            GetDataStationCode();
        }

        #region [Load data StationCodeList]

        public void GetDataStationCode()
        {
            PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            try
            {
                values = new Dictionary<string, string>();
                string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/load-station-list";
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<List<ValueReportLParkingModel>>(result);
                    if (responseData != null)
                    {
                        StationCodeList = new ObservableCollection<ValueReportLParkingModel>();
                        foreach (var item in responseData)
                        {
                            if (!item.Name.Equals("Tất cả"))
                            {
                                StationCodeList.Add(item);
                            }
                        }
                        //var message = "Load dữ liệu thành công!";
                        //DependencyService.Get<IMessage>().ShortTime(message);
                    }
                    else
                    {
                        //var message = "Dữ liệu rỗng!";
                        //DependencyService.Get<IMessage>().LongTime(message);
                    }
                }
                else
                {
                    //var message = "Dữ liệu rỗng!";
                    //DependencyService.Get<IMessage>().LongTime(message);
                }
            }
            catch (Exception)
            {

                PopupNavigation.Instance.PopAllAsync();
            }
            PopupNavigation.Instance.PopAllAsync();
        }
        #endregion
        #region [Hàm xử lý chung]
        private void btnSearch_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            try
            {
                //Clear doanh thu trước khi search
                TotalPrice = 0;
                TotalIn = 0;
                TotalOut = 0;

                AddDataValues();
                string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/load-day-statistic";
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<DataReportLParkingModel>(result);
                    if (responseData != null && responseData.data.Count > 0)
                    {
                        DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>(responseData.data);

                        foreach (var item in DataLParkingReport)
                        {
                            TotalPrice += item.TotalPrice;
                            TotalIn += item.InCount;
                            TotalOut += item.OutCount;
                        }

                        PopupNavigation.Instance.PopAllAsync();
                        var message = "Tìm dữ liệu thành công!";
                        DependencyService.Get<IMessage>().ShortTime(message);
                    }
                    else
                    {
                        PopupNavigation.Instance.PopAllAsync();
                        DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>();
                        var message = "Dữ liệu rỗng!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                }
                else
                {
                    PopupNavigation.Instance.PopAllAsync();
                    DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>();
                    var message = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(message);

                }
            }
            catch (Exception)
            {
                PopupNavigation.Instance.PopAllAsync();
                DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>();
                var message = "Không kết nối được máy chủ!";
                DependencyService.Get<IMessage>().LongTime(message);
            }

        }
        public void GetDataLParkingReport()
        {
            PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            try
            {
                //Clear doanh thu trước khi search
                TotalPrice = 0;
                TotalIn = 0;
                TotalOut = 0;
                
                AddDataValues();
                string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/load-day-statistic";
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<DataReportLParkingModel>(result);
                    if (responseData != null && responseData.data.Count > 0)
                    {
                        DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>(responseData.data);

                        foreach (var item in DataLParkingReport)
                        {
                            TotalPrice += item.TotalPrice;
                            TotalIn += item.InCount;
                            TotalOut += item.OutCount;
                        }

                        PopupNavigation.Instance.PopAllAsync();
                        var message = "Tìm dữ liệu thành công!";
                        DependencyService.Get<IMessage>().ShortTime(message);
                    }
                    else
                    {
                        PopupNavigation.Instance.PopAllAsync();
                        DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>();
                        var message = "Dữ liệu rỗng!";
                        DependencyService.Get<IMessage>().LongTime(message);
                    }
                }
                else
                {
                    PopupNavigation.Instance.PopAllAsync();
                    DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>();
                    var message = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(message);

                }
            }
            catch (Exception)
            {
                PopupNavigation.Instance.PopAllAsync();
                DataLParkingReport = new ObservableCollection<ValueReportLParkingModel>();
                var message = "Không kết nối được máy chủ!";
                DependencyService.Get<IMessage>().LongTime(message);
            }



        }

        public void AddDataValues()
        {
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            //Danh sách tên mã trạm
            values.Add("inStationCodeListName", inStationCodeListName); //Chọn hết => All, ngược lại Empty.
            //Danh sách mã trạm
            values.Add("inStationCodeList", inStationCodeList);//Tên cách nhau dấu , nếu chọn hết => Empty
            //Loại dữ liệu
            values.Add("inVehicleDataType", inVehicleDataType);//Index


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
        private void ViewCell_Tapped(object sender, EventArgs e)
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
        #endregion


        private void btnStationCodeList_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            grdIsBusy.IsVisible = true;
            stlSelectDataSearch.IsVisible = true;
            grdStationCodeList.IsVisible = true;
            SelectStationCodeList = new List<string>();
            SelectStationCodeList.Clear();
            inStationCodeList = "";
            inStationCodeListName = "";
            btnStationCodeList.Text = "";
        }

        private void LstvStationCodeList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = (ValueReportLParkingModel)LstvStationCodeList.SelectedItem;

            if (selected != null)
            {
                if (StationCodeList != null)
                {
                    if (selected.IsSelected == true)
                    {
                        selected.IsSelected = false;
                    }
                    else
                    {
                        selected.IsSelected = true;

                    }
                }
            }
        }

        private void cbSelectAll_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto == false)
            {
                isCheckByAuto = true;
                if (StationCodeList != null)
                {
                    if (e.Value == true)
                    {

                        foreach (var item in StationCodeList)
                        {
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in StationCodeList)
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
        private void cbSelect_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto)
                return;

            if (StationCodeList != null)
            {
                bool isCheckAll = true;
                foreach (var item in StationCodeList)
                {
                    if (item.IsSelected == false)
                    {
                        isCheckAll = false;
                        break;
                    }
                }

                if (cbSelectAll.IsChecked != isCheckAll)
                {
                    isCheckByAuto = true;
                    cbSelectAll.IsChecked = isCheckAll;
                }
            }
        }

        private void btnSelectItem_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, true);
            grdIsBusy.IsVisible = false;
            stlSelectDataSearch.IsVisible = false;
            grdStationCodeList.IsVisible = false;
            SelectStationCodeList.Clear();
            inStationCodeList = "";
            inStationCodeListName = "";
            btnStationCodeList.Text = "";

            //Duyệt qua các phần tử của StationCodeList
            if (StationCodeList != null)
            {
                foreach (var item in StationCodeList)
                {
                    if (item.IsSelected == true)
                    {
                        //Những item được chọn cho vào list
                        SelectStationCodeList.Add(item.Name);
                    }
                }

                //Duyệt qua list và thêm vào inStationCodeList cách nhau bởi dấu ,
                for (int i = 0; i < SelectStationCodeList.Count(); i++)
                {
                    if (i == SelectStationCodeList.Count() - 1)
                    {
                        inStationCodeList += SelectStationCodeList[i];
                    }
                    else
                    {
                        inStationCodeList += SelectStationCodeList[i] + ",";
                    }

                }

                if (SelectStationCodeList.Count() == StationCodeList.Count())
                {
                    //Nếu chọn tất cả
                    inStationCodeList = "";
                    inStationCodeListName = "All";
                    btnStationCodeList.Text = "Tất cả";
                }
                else
                {
                    //Nếu không chọn tất cả
                    inStationCodeListName = "";
                    btnStationCodeList.Text = inStationCodeList;

                    if (inStationCodeList == "")
                    {
                        btnStationCodeList.Text = "--Chọn--";
                    }
                }



            }
        }


    }
}