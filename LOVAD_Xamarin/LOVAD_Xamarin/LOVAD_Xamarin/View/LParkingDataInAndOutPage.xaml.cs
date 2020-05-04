
using LOVAD_Xamarin.Model;
using LOVAD_Xamarin.Utility;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfDataGrid.XForms.DataPager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LParkingDataInAndOutPage : ContentPage
    {
        #region [LParkingDataInAndOutPage]
        private string _startDate;
        public string startDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("startDate"); } }

        private string _endDate;
        public string endDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("endDate"); } }

        private string _vehicleDataType;
        public string vehicleDataType { get { return _vehicleDataType; } set { _vehicleDataType = value; OnPropertyChanged("vehicleDataType"); } }

        private string _customerTypeListName;
        public string customerTypeListName { get { return _customerTypeListName; } set { _customerTypeListName = value; OnPropertyChanged("customerTypeListName"); } }


        private string _inCustomerTypeList;
        public string inCustomerTypeList { get { return _inCustomerTypeList; } set { _inCustomerTypeList = value; OnPropertyChanged("inCustomerTypeList"); } }

        private string _inPaymentTypeList;
        public string inPaymentTypeList { get { return _inPaymentTypeList; } set { _inPaymentTypeList = value; OnPropertyChanged("inPaymentTypeList"); } }



        private string _inVehicleTypeList;
        public string inVehicleTypeList { get { return _inVehicleTypeList; } set { _inVehicleTypeList = value; OnPropertyChanged("inVehicleTypeList"); } }

        private string _vehicleTypeListName;
        public string vehicleTypeListName { get { return _vehicleTypeListName; } set { _vehicleTypeListName = value; OnPropertyChanged("vehicleTypeListName"); } }
        private string _paymentTypeListName;
        public string paymentTypeListName { get { return _paymentTypeListName; } set { _paymentTypeListName = value; OnPropertyChanged("paymentTypeListName"); } }


        private string _stationCodeListName;
        public string stationCodeListName { get { return _stationCodeListName; } set { _stationCodeListName = value; OnPropertyChanged("stationCodeListName"); } }

        private string _searchField;
        public string searchField { get { return _searchField; } set { _searchField = value; OnPropertyChanged("searchField"); } }

        private string _searchContent;
        public string searchContent { get { return _searchContent; } set { _searchContent = value; OnPropertyChanged("searchContent"); } }


        private string _vehicleNotCheckOut;
        public string vehicleNotCheckOut { get { return _vehicleNotCheckOut; } set { _vehicleNotCheckOut = value; OnPropertyChanged("vehicleNotCheckOut"); } }

        private string _pageSize;
        public string PageSize { get { return _pageSize; } set { _pageSize = value; OnPropertyChanged("PageSize"); } }

        private double _heightImg;
        public double HeightImg { get { return _heightImg; } set { _heightImg = value; OnPropertyChanged("HeightImg"); } }

        private double _widthImg;
        public double WidthImg { get { return _widthImg; } set { _widthImg = value; OnPropertyChanged("WidthImg"); } }

        private ObservableCollection<DataSearchModel> _vehicleList;
        public ObservableCollection<DataSearchModel> VehicleList { get { return _vehicleList; } set { _vehicleList = value; OnPropertyChanged(); } }
        
        private ObservableCollection<DataSearchModel> _customerTypeList;
        public ObservableCollection<DataSearchModel> CustomerTypeList { get { return _customerTypeList; } set { _customerTypeList = value; OnPropertyChanged(); } }

        private ObservableCollection<DataSearchModel> _InPaymentTypeList;
        public ObservableCollection<DataSearchModel> InPaymentTypeList { get { return _InPaymentTypeList; } set { _InPaymentTypeList = value; OnPropertyChanged(); } }

        private ObservableCollection<DataSearchModel> _vehicleDataTypeList;
        public ObservableCollection<DataSearchModel> VehicleDataTypeList { get { return _vehicleDataTypeList; } set { _vehicleDataTypeList = value; OnPropertyChanged(); } }
        private ObservableCollection<DataSearchModel> _searchFieldList;
        public ObservableCollection<DataSearchModel> SearchFieldList { get { return _searchFieldList; } set { _searchFieldList = value; OnPropertyChanged(); } }

        #endregion

        #region [Biến khở tạo xử lý trong page]
        //public int PageSize = 30;
        public int CountlData = 0;
        public int CurentPage = 1;
        public long stt = 0;
        public int TotalCountEnd = 0;
        public bool flag;
        public bool flagViewImage;
        public int page = 0;
        public int MaxPage = 1;
        public bool CallBack;
        private bool isCheckByAuto = false;
        private bool Start = false;
        public List<string> DataVehicleType;
        public List<string> DataCustomerType;
        public List<string> DataInPaymentType;
        

        public List<ValueLParkingModel> ValList = new List<ValueLParkingModel>();
        public DataImageLParkingModel dataImageLParkingModel;
        public ViewCell lastCell;

        public Dictionary<string, string> values;
        #endregion
        public LParkingDataInAndOutPage()
        {
            
            InitializeComponent();
            Start = true;
            BindingContext = this;
            PageSize = "30";
            chkVehicleNotCheckOut.IsChecked = false;
            pkrSearchField.SelectedIndex = 0;
            searchContent = "";
            GetDataLParkingInAndOut(PageSize);
            btnBackPage.IsEnabled = false;
            grdSearchInformation.IsVisible = false;
        }
        #region [Event dùng chung]
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            if (flagViewImage == true)
            {
                PopupNavigation.Instance.PopAllAsync(true);
                flagViewImage = false;
            }

            //return false;
            return true;
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
        public void BackToMainLparkingPage()
        {
            grdIsBusy.IsVisible = false;
            stlSelectDataSearch.IsVisible = false;
            grdVehicle.IsVisible = false;
            grdCustomerType.IsVisible = false;
            grdInPaymentTypeList.IsVisible = false;
        }
        #endregion

        #region [Phân Trang]
        private async void btnNextPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            btnBackPage.IsEnabled = true;
            CurentPage++;
            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ValList = responseData.data.ToList();

                    //foreach (var item in ValList)
                    //{
                    //    stt += 1;
                    //    item.Index = stt.ToString();
                    //}

                    List<string> ListStt = new List<string>();
                    int tong = CurentPage * Int32.Parse(PageSize) - (Int32.Parse(PageSize) - 1);
                    for (int i = tong; i <= Int32.Parse(PageSize) * CurentPage; i++)
                    {
                        ListStt.Add(i.ToString());
                    }

                    for (int i = 0; i < ValList.Count; i++)
                    {
                        ValList[i].Index = ListStt[i];
                    }
                    lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                    btnFristPage.IsEnabled = true;
                }
                else
                {
                    lstvDuLieuVaoRaLParking.ItemsSource = "";
                    lblCurentPage.Text = "";
                    var message = "Dữ liệu rỗng!";
                    btnNextPage.IsEnabled = false;
                    DependencyService.Get<IMessage>().LongTime(message);
                }
                if (CurentPage == MaxPage)
                {
                    btnNextPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnFristPage.IsEnabled = true;
                    btnBackPage.IsEnabled = true;
                }
            }
            else
            {
                ErroAndSetDefault();
            }
            await PopupNavigation.Instance.PopAllAsync();
        }
        private async void btnBackPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            btnNextPage.IsEnabled = true;
            if (CurentPage > 0)
            {
                CurentPage--;
                AddDataValues();
                string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                    if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                    {
                        ValList = responseData.data.ToList();
                        List<string> ListStt = new List<string>();
                        int tong = CurentPage * Int32.Parse(PageSize)  - (Int32.Parse(PageSize) - 1);
                        for (int i = tong; i <= Int32.Parse(PageSize) * CurentPage; i++)
                        {
                            ListStt.Add(i.ToString());
                        }

                        for (int i = 0; i < ValList.Count; i++)
                        {
                            ValList[i].Index = ListStt[i];
                        }
                        
                        //long i = 1;
                        //long count = stt;
                        //int IsNumberprobably = (int)((double)CountlData / Int32.Parse(PageSize)) % 2;
                        //if (IsNumberprobably != 0 && flag == true)
                        //{
                        //    count = count + (Int32.Parse(PageSize) - TotalCountEnd);
                        //    foreach (var item in ValList)
                        //    {
                        //        stt = count - ((Int32.Parse(PageSize) * 2) - i);
                        //        i++;
                        //        item.Index = stt.ToString();
                        //    }
                        //    flag = false;
                        //}
                        //else
                        //{
                        //    foreach (var item in ValList)
                        //    {
                        //        stt = count - ((Int32.Parse(PageSize) * 2) - i);
                        //        i++;
                        //        item.Index = stt.ToString();
                        //    }
                        //}

                        lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                        lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                        btnLastPage.IsEnabled = true;
                        btnNextPage.IsEnabled = true;
                    }
                    else
                    {
                        ErroAndSetDefault();
                    }
                }
                else
                {
                    ErroAndSetDefault();
                }

            }
            if (CurentPage == 1)
            {
                btnBackPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
            }
            await PopupNavigation.Instance.PopAllAsync();

        }

        private async void btnFristPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));

            flag = false;
            stt = 0;
            CurentPage = 1;
            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ValList = responseData.data.ToList();

                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }
                    lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                    CountlData = responseData.RecordsTotal;
                    btnNextPage.IsEnabled = true;
                }
                else
                {
                    ErroAndSetDefault();
                }
                btnLastPage.IsEnabled = true;
                btnNextPage.IsEnabled = true;
                btnFristPage.IsEnabled = false;
                btnBackPage.IsEnabled = false;
            }
            else
            {
                ErroAndSetDefault();
            }

            await PopupNavigation.Instance.PopAllAsync();

        }

        private async void btnLastPage_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            flag = true;//Người dùng bấm vào nút cuối trang
            int page = (int)(CountlData / Int32.Parse(PageSize));
            //Làm tròn lên
            CurentPage = (int)Math.Ceiling((double)CountlData / Int32.Parse(PageSize));
            stt = page * Int32.Parse(PageSize);
            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ValList = responseData.data.ToList();
                    TotalCountEnd = ValList.Count();
                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }
                    lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;

                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = true;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = true;
                }
                else
                {
                    ErroAndSetDefault();
                }
            }
            else
            {
                ErroAndSetDefault();
            }
            await PopupNavigation.Instance.PopAllAsync();
        }
        #endregion

        #region [Data Main Lparking Page]
        public void GetDataLParkingInAndOut(string PageSize)
        {
            CurentPage = 1;
            #region [vehicleTypeListName]
            LoadInVehicleTypeList();

            if (VehicleList != null)
            {
                DataVehicleType = new List<string>();
                DataVehicleType.Clear();
                btnSelectInVehicleTypeList.Text = "All";
                inVehicleTypeList = "";
                foreach (var item in VehicleList)
                {
                    item.IsSelected = true;
                    DataVehicleType.Add(item.Id);
                }

                for (int i = 0; i < DataVehicleType.Count(); i++)
                {
                    if (i == DataVehicleType.Count() - 1)
                    {
                        inVehicleTypeList += DataVehicleType[i];
                    }
                    else
                    {
                        inVehicleTypeList += DataVehicleType[i] + ",";
                    }
                }
                DataVehicleType.Clear();
                vehicleTypeListName = "All";
                cbSelectAllVehicle.IsChecked = true;
            }
            else
            {
                vehicleTypeListName = null;
                btnInCustomerTypeList.Text = "-- Chọn loại đối tượng --";
               
            }
            #endregion

            #region [inCustomerTypeList]
            LoadCustomerTypeList();

            if (CustomerTypeList != null)
            {
                DataCustomerType = new List<string>();
                DataCustomerType.Clear();
                btnInCustomerTypeList.Text = "All";
                inCustomerTypeList = "";
                foreach (var item in CustomerTypeList)
                {
                    item.IsSelectedCustomerType = true;
                    DataCustomerType.Add(item.IdCustomerType);
                }

                for (int i = 0; i < DataCustomerType.Count(); i++)
                {
                    if (i == DataCustomerType.Count() - 1)
                    {
                        inCustomerTypeList += DataCustomerType[i];
                    }
                    else
                    {
                        inCustomerTypeList += DataCustomerType[i] + ",";
                    }
                }
                DataCustomerType.Clear();
                customerTypeListName = "All";
                cbSelectAllCustomerType.IsChecked = true;
            }
            else
            {
                customerTypeListName = null;
                btnSelectInVehicleTypeList.Text = "-- chọn loại khách --";

            }
            #endregion

            #region [InPaymentType]
            LoadInPaymentType();
            if (InPaymentTypeList != null)
            {
                DataInPaymentType = new List<string>();
                DataInPaymentType.Clear();
                btnInPaymentTypeList.Text = "All";
                inPaymentTypeList = "";
                foreach (var item in InPaymentTypeList)
                {
                    item.IsSelectedInPaymentType = true;
                    DataInPaymentType.Add(item.IdInPaymentType);
                }

                for (int i = 0; i < DataInPaymentType.Count(); i++)
                {
                    if (i == DataInPaymentType.Count() - 1)
                    {
                        inPaymentTypeList += DataInPaymentType[i];
                    }
                    else
                    {
                        inPaymentTypeList += DataInPaymentType[i] + ",";
                    }
                }
                DataInPaymentType.Clear();
                paymentTypeListName = "All";
                cbSelectAllInPaymentType.IsChecked = true;
            }
            else
            {
                paymentTypeListName = null;
                btnInPaymentTypeList.Text = "-- Hình thức thanh toán --";
            }
            #endregion

            #region [VehicleDataType]
            LoadVehicleDataTypeList();
            pkrVehicleDataType.SelectedIndex = 0;
            #endregion

            #region [SearchField]
            LoadSearchFieldList();
            pkrSearchField.SelectedIndex = 0;
            #endregion

            AddDataValues();
            string url = "http://"+ Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ValList = responseData.data;

                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();

                        Global.Intance.TotalRevenue += item.Price;
                    }
                    lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                   

                   
                    CountlData = responseData.RecordsTotal;

                    if (CountlData == ValList.Count)
                    {
                        lblCurentPage.Text = "Trang " + CurentPage + "/ " + 1;
                        btnNextPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = false;
                        btnBackPage.IsEnabled = false;
                    }
                    else
                    {
                        //Làm tròn lên
                        MaxPage = (int)Math.Ceiling((double)CountlData / Int32.Parse(PageSize));

                        lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                        btnNextPage.IsEnabled = true;
                    }
                    
                }
                else
                {
                    ErroAndSetDefault();
                }
            }
            else
            {
                ErroAndSetDefault();
            }

        }

        private async void btnSeach_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            stt = 0;
            CurentPage = 1;
            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            try
            {
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                    if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                    {
                        ValList = responseData.data.ToList();

                        foreach (var item in ValList)
                        {
                            stt += 1;
                            item.Index = stt.ToString();
                            Global.Intance.TotalRevenue += item.Price;
                        }
                        lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                        CountlData = responseData.RecordsTotal;
                        //await PopupNavigation.Instance.PopAllAsync();
                        if (PageSize != null)
                        {
                            if (PageSize != "All")
                            {
                                MaxPage = (int)Math.Ceiling((double)CountlData / Int32.Parse(PageSize));
                                lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                                btnNextPage.IsEnabled = true;
                                btnLastPage.IsEnabled = true;
                            }
                            else
                            {
                                MaxPage = 1;
                                lblCurentPage.Text = "Trang " + 1 + "/ " + MaxPage;
                                btnNextPage.IsEnabled = false;
                                btnFristPage.IsEnabled = false;
                                btnLastPage.IsEnabled = false;
                                btnBackPage.IsEnabled = false;
                            }
                        }
                        await PopupNavigation.Instance.PopAllAsync();
                        var message = "Tìm dữ liệu thành công!";
                        DependencyService.Get<IMessage>().ShortTime(message);
                    }
                    else
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        ErroAndSetDefault();

                    }
                }
                else
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    ErroAndSetDefault();
                }
            }
            catch (Exception)
            {
                await PopupNavigation.Instance.PopAllAsync();
                ErroAndSetDefault();
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
        public void ErroAndSetDefault()
        {
            lstvDuLieuVaoRaLParking.ItemsSource = "";
            lblCurentPage.Text = "";
            var message = "Dữ liệu rỗng!";
            btnNextPage.IsEnabled = false;
            btnFristPage.IsEnabled = false;
            btnLastPage.IsEnabled = false;
            btnBackPage.IsEnabled = false;
           
            DependencyService.Get<IMessage>().LongTime(message);
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

        private async void lstvDuLieuVaoRaLParking_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            try
            {
                var DataLParkingSelect = (ValueLParkingModel)lstvDuLieuVaoRaLParking.SelectedItem;
                Dictionary<string, string> values = new Dictionary<string, string>(8);
                values.Add("InFrontImagePath", DataLParkingSelect.InFrontImagePath);
                values.Add("InBackImagePath", DataLParkingSelect.InBackImagePath);
                values.Add("InFrontScanImagePath", DataLParkingSelect.InFrontScanImagePath);
                values.Add("InBackScanImagePath", DataLParkingSelect.InBackScanImagePath);
                values.Add("OutFrontImagePath", DataLParkingSelect.OutFrontImagePath);
                values.Add("OutBackImagePath", DataLParkingSelect.OutBackImagePath);
                values.Add("OutFrontScanImagePath", DataLParkingSelect.OutFrontScanImagePath);
                values.Add("OutBackScanImagePath", DataLParkingSelect.OutBackScanImagePath);

                dataImageLParkingModel = new DataImageLParkingModel();

                string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/download-image-list";
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<DataImageLParkingModel>(result);
                    if (responseData != null)
                    {

                        dataImageLParkingModel = responseData;
                        //InFrontImagePath //khuon mat vao
                        imgInBefore.Source = ParseImageByteArray(responseData.data.InFrontImagePath);

                        //InBackImagePath // bien so vao
                        imgOutBefore.Source = ParseImageByteArray(responseData.data.InBackImagePath);

                        //OutFrontImagePath //khuon mat ra
                        imgInAfter.Source = ParseImageByteArray(responseData.data.OutFrontImagePath);

                        //OutBackImagePath //bien so ra
                        imgOutAfter.Source = ParseImageByteArray(responseData.data.OutBackImagePath);


                        InFrontScanImagePath.Source = ParseImageByteArray(responseData.data.InFrontScanImagePath);
                        InBackScanImagePath.Source = ParseImageByteArray(responseData.data.InBackScanImagePath);
                        OutFrontScanImagePath.Source = ParseImageByteArray(responseData.data.OutFrontScanImagePath);
                        OutBackScanImagePath.Source = ParseImageByteArray(responseData.data.OutBackScanImagePath);
                        
                    }
                    else
                    {
                        var message = "Không có hình ảnh!";
                        DependencyService.Get<IMessage>().LongTime(message);
                        //InFrontImagePath //khuon mat vao
                        imgInBefore.Source = "NoImage.png";

                        //InBackImagePath // bien so vao
                        imgOutBefore.Source = "NoImage.png";

                        //OutFrontImagePath //khuon mat ra
                        imgInAfter.Source = "NoImage.png";

                        //OutBackImagePath //bien so ra
                        imgOutAfter.Source = "NoImage.png";

                        InFrontScanImagePath.Source = "NoImage.png";
                        InBackScanImagePath.Source = "NoImage.png";
                        OutFrontScanImagePath.Source = "NoImage.png";
                        OutBackScanImagePath.Source = "NoImage.png";
                        
                    }
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    DependencyService.Get<IMessage>().LongTime(message);
                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = false;
                  
                }
            }
            catch (Exception)
            {

                var message = "Dữ liệu rỗng!";
                DependencyService.Get<IMessage>().LongTime(message);
                btnNextPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
                btnBackPage.IsEnabled = false;
               
            }
            await PopupNavigation.Instance.PopAllAsync();
        }

        public ImageSource ParseImageByteArray(string ImagePath)
        {
            if (ImagePath != null)
            {
                try
                {
                    byte[] byteArray = Convert.FromBase64String(ImagePath);
                    Stream stream = new MemoryStream(byteArray);
                    var imageSource = ImageSource.FromStream(() => stream);
                    return imageSource;
                }
                catch (Exception ex)
                {

                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return null;
                }
                finally
                {

                }
            }
            else
            {
                //var message = "Hình ảnh không đầy đủ!";
                //DependencyService.Get<IMessage>().LongTime(message);
                return "NoImage.png";
            }


        }

        public void AddDataValues()
        {
            try
            {
                startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                if (chkVehicleNotCheckOut.IsChecked == false)
                {
                    vehicleNotCheckOut = "0";
                }
                else
                    vehicleNotCheckOut = "1";


                if (pkrPageSize.SelectedIndex == 5)
                {
                    values = new Dictionary<string, string>();
                    values.Add("stationCodeListName", "All");
                    values.Add("startDate", startDate);
                    values.Add("endDate", endDate);
                    values.Add("vehicleTypeListName", vehicleTypeListName);//Loại đối tượng
                    values.Add("inVehicleTypeList", inVehicleTypeList);
                    values.Add("customerTypeListName", customerTypeListName);//Loại khách
                    values.Add("inCustomerTypeList", inCustomerTypeList);
                    values.Add("paymentTypeListName", paymentTypeListName);//Hình thức thanh toán
                    values.Add("inPaymentTypeList", inPaymentTypeList);
                    values.Add("vehicleDataType", vehicleDataType);//ALL: "<> 'X'", Loại dữ liệu
                    values.Add("searchField", searchField);//BSX: "5" Thông tin tìm kiếm
                    values.Add("searchContent", searchContent);// noi dung tim kiem
                    values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
                    values.Add("pageSize", "0");
                    values.Add("currentPage", CurentPage.ToString());
                }
                else
                {
                    values = new Dictionary<string, string>();
                    values.Add("stationCodeListName", "All");
                    values.Add("startDate", startDate);
                    values.Add("endDate", endDate);
                    values.Add("vehicleTypeListName", vehicleTypeListName);//Loại đối tượng
                    values.Add("inVehicleTypeList", inVehicleTypeList);
                    values.Add("customerTypeListName", customerTypeListName);//Loại khách
                    values.Add("inCustomerTypeList", inCustomerTypeList);
                    values.Add("paymentTypeListName", paymentTypeListName);//Hình thức thanh toán
                    values.Add("inPaymentTypeList", inPaymentTypeList);
                    values.Add("vehicleDataType", vehicleDataType);//ALL: "<> 'X'", Loại dữ liệu
                    values.Add("searchField", searchField);//BSX: "5" Thông tin tìm kiếm
                    values.Add("searchContent", searchContent);// noi dung tim kiem
                    values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
                    values.Add("pageSize", PageSize);
                    values.Add("currentPage", CurentPage.ToString());
                }
               
            }
            catch (Exception ex)
            {

                DependencyService.Get<IMessage>().LongTime(ex.Message);
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
        #endregion

        #region [Xử lý Image khi click vào]
        private void imgInBefore_Tapped(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.InFrontImagePath));
            }
        }

        private void imgInAfter_Tapped(object sender, EventArgs e)
        {
            if (dataImageLParkingModel !=null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.OutFrontImagePath));
            }
        }

        private void imgOutBefore_Tapped(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.InBackImagePath));
            }
        }

        private void imgOutAfter_Tapped(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.OutBackImagePath));
            }
        }

        private void InFrontScanImagePath_Tap(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.InFrontScanImagePath));
            }
            
        }

        private void InBackScanImagePath_Tap(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.InBackScanImagePath));
            }
           
        }

        private void OutFrontScanImagePath_Tap(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.OutFrontScanImagePath));
            }
            
        }

        private void OutBackScanImagePath_Tap(object sender, EventArgs e)
        {
            if (dataImageLParkingModel != null)
            {
                flagViewImage = true;
                this.IsBusy = true;
                PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.OutBackScanImagePath));
            }
            
        }

        private void btnUnfoldImg_Clicked(object sender, EventArgs e)
        {
            if (grdImage.IsVisible == false)
            {

                btnUnfoldImg.ImageSource = "dow.png";
                btnUnfoldImg.Text = "ẨN HÌNH ẢNH";
                grdImage.IsVisible = true;

                return;

            }

            if (grdImage.IsVisible == true)
            {
                btnUnfoldImg.ImageSource = "up.png";
                btnUnfoldImg.Text = "XEM THÊM HÌNH ẢNH";
                grdImage.IsVisible = false;

                return;
            }
        }

       
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (DeviceInfo.IsOrientationPortrait() && width > height || !DeviceInfo.IsOrientationPortrait() && width < height)
            {
                HeightImg = height / 3;
                WidthImg = width / 2;
            }

            if (DeviceInfo.IsOrientationPortrait() && width < height || !DeviceInfo.IsOrientationPortrait() && width > height)
            {
                HeightImg = height / 2;
                WidthImg = width / 3;
            }
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
                    btnSeach.IsEnabled = true;
                    VehicleList = new ObservableCollection<DataSearchModel>(responseData);
                }
                else
                {
                    btnSeach.IsEnabled = false;
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

        //Danh sách Loại khách
        public void LoadCustomerTypeList()
        {
            List<DataSearchModel> ListCustomerType = new List<DataSearchModel>();
            ListCustomerType.Add(new DataSearchModel() { IdCustomerType = "-1", NameCustomerType = "Vé ngày" });
            ListCustomerType.Add(new DataSearchModel() { IdCustomerType = "0", NameCustomerType = "Vãng lai" });
            ListCustomerType.Add(new DataSearchModel() { IdCustomerType = "1", NameCustomerType = "Khách hàng" });
            ListCustomerType.Add(new DataSearchModel() { IdCustomerType = "2", NameCustomerType = "Nhân viên" });

            CustomerTypeList = new ObservableCollection<DataSearchModel>(ListCustomerType);
        }

        //Danh sách Hình thức thanh toán
        public void LoadInPaymentType()
        {
            List<DataSearchModel> ListInPaymentType = new List<DataSearchModel>();
            ListInPaymentType.Add(new DataSearchModel() { IdInPaymentType = "0", NameInPaymentType = "Không tính tiền" });
            ListInPaymentType.Add(new DataSearchModel() { IdInPaymentType = "1", NameInPaymentType = "Tính tiền theo ngày" });
            ListInPaymentType.Add(new DataSearchModel() { IdInPaymentType = "2", NameInPaymentType = "Tính tiền theo tháng" });

            InPaymentTypeList = new ObservableCollection<DataSearchModel>(ListInPaymentType);
        }

        //Danh sách Loại dữ liệu
        public void LoadVehicleDataTypeList()
        {
            List<DataSearchModel> ListVehicleDataType = new List<DataSearchModel>();
            ListVehicleDataType.Add(new DataSearchModel() { IdVehicleDataType = "<> 'X'", NameVehicleDataType = "Tất cả" });
            ListVehicleDataType.Add(new DataSearchModel() { IdVehicleDataType = "= 'O'", NameVehicleDataType = "Dữ liệu xe" });
            ListVehicleDataType.Add(new DataSearchModel() { IdVehicleDataType = "= 'N'", NameVehicleDataType = "Dữ liệu khách hàng" });
            VehicleDataTypeList = new ObservableCollection<DataSearchModel>(ListVehicleDataType);
        }

        //Danh sách Thông tin tìm kiếm
        public void LoadSearchFieldList()
        {
            List<DataSearchModel> ListSearchField = new List<DataSearchModel>();
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "1", NameSearchField = "1-Mã thẻ" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "2", NameSearchField = "2-Mã khách hàng" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "3", NameSearchField = "3-Họ tên" });
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "4", NameSearchField = "4-Số điện thoại"});
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "5", NameSearchField = "5-Biển số xe"});
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "6", NameSearchField = "6-Công ty"});
            ListSearchField.Add(new DataSearchModel() { IdSearchField = "7", NameSearchField = "7-Tổng thời lượng"});
            SearchFieldList = new ObservableCollection<DataSearchModel>(ListSearchField);
        }
        #endregion

        #region [Nút nhấn hiển thị danh sách checkbox trên LParkingMain]
        private void btnSelectInVehicleTypeList_Clicked(object sender, EventArgs e)
        {
            grdIsBusy.IsVisible = true;

            stlSelectDataSearch.IsVisible = true;
            grdVehicle.IsVisible = true;
            grdCustomerType.IsVisible = false;
            grdInPaymentTypeList.IsVisible = false;

            //bool IsCheck = true;
            //foreach (var item in VehicleList)
            //{
            //    if (item.IsSelected == false)
            //    {
            //        IsCheck = false;
            //        break;
            //    }
            //}

            //if (IsCheck == false)
            //    btnSelectItemVehicle.IsEnabled = false;
            //else
            //{
            //    btnSelectItemVehicle.IsEnabled = true;
            //}
                
        }


        private void btnInCustomerTypeList_Clicked(object sender, EventArgs e)
        {
            grdIsBusy.IsVisible = true;

            stlSelectDataSearch.IsVisible = true;

            grdVehicle.IsVisible = false;
            grdCustomerType.IsVisible = true;
            grdInPaymentTypeList.IsVisible = false;

            //bool IsCheck = false;
            //foreach (var item in CustomerTypeList)
            //{
            //    if (item.IsSelectedCustomerType == true)
            //    {
            //        IsCheck = true;
            //        break;
            //    }
            //}

            //if (IsCheck == true)
            //    btnSelectItemCustomerType.IsEnabled = true;
            //else
            //    btnSelectItemCustomerType.IsEnabled = false;
        }

        private void btnInPaymentTypeList_Clicked(object sender, EventArgs e)
        {
            grdIsBusy.IsVisible = true;

            stlSelectDataSearch.IsVisible = true;
            grdVehicle.IsVisible = false;
            grdCustomerType.IsVisible = false;
            grdInPaymentTypeList.IsVisible = true;

            //bool IsCheck = false;
            //foreach (var item in InPaymentTypeList)
            //{
            //    if (item.IsSelectedInPaymentType == true)
            //    {
            //        IsCheck = true;
            //        break;
            //    }
            //}

            //if (IsCheck == true)
            //    btnSelectItemInPaymentType.IsEnabled = true;
            //else
            //    btnSelectItemInPaymentType.IsEnabled = false;
        }
        #endregion

        #region [Xử lý VehicleList]
        private void btnSelectItemVehicle_Clicked(object sender, EventArgs e)
        {
            if (VehicleList != null)
            {
                DataVehicleType = new List<string>();
                DataVehicleType.Clear();
                inVehicleTypeList = "";

                btnSelectInVehicleTypeList.Text = "";

               

                foreach (var item in VehicleList)
                {
                    if (item.IsSelected == true)
                    {
                        DataVehicleType.Add(item.Name);
                        DataVehicleType.Add(item.Id);
                    }


                }

                for (int i = 0; i < DataVehicleType.Count(); i+=2)
                {
                    if (i == DataVehicleType.Count() - 2)
                    {
                        btnSelectInVehicleTypeList.Text += DataVehicleType[i];
                        inVehicleTypeList += DataVehicleType[i+1];
                    }
                    else
                    {
                        btnSelectInVehicleTypeList.Text += DataVehicleType[i] + ",";
                        inVehicleTypeList += DataVehicleType[i+1] + ",";
                    }
                }
                DataVehicleType.Clear();

                bool IsCheck = true;
                foreach (var item in VehicleList)
                {
                    if (item.IsSelected == false)
                    {
                        IsCheck = false;
                        break;
                    }
                }

                if (IsCheck == false)
                    vehicleTypeListName = null;
                else
                {
                    vehicleTypeListName = "All";
                    btnSelectInVehicleTypeList.Text = "All";
                }


                bool IsCheckFalse = true;
                foreach (var item in VehicleList)
                {
                    if (item.IsSelected == true)
                    {
                        IsCheckFalse = false;
                        break;
                    }
                }
                if (IsCheckFalse == true)
                    btnSelectInVehicleTypeList.Text = "-- Chọn loại đối tượng --";

                grdIsBusy.IsVisible = false;

                stlSelectDataSearch.IsVisible = false;
            }
            BackToMainLparkingPage();

        }
        private void cbSelectVehicle_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto)
                return;

            if (VehicleList != null)
            {
                bool isCheckAll = true;
                foreach (var item in VehicleList)
                {
                    if (item.IsSelected == false)
                    {
                        isCheckAll = false;
                        break;
                    }
                }

                if (cbSelectAllVehicle.IsChecked != isCheckAll)
                {
                    isCheckByAuto = true;
                    cbSelectAllVehicle.IsChecked = isCheckAll;
                }
            }
        }
        private void LstvVehicleList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //bool Check = false;
            var selectedVehicleList = (DataSearchModel)LstvVehicleList.SelectedItem;

            if (selectedVehicleList != null)
            {
                if (VehicleList != null)
                {
                    if (selectedVehicleList.IsSelected == true)
                    {
                        selectedVehicleList.IsSelected = false;
                    }
                    else
                    {
                        selectedVehicleList.IsSelected = true;

                    }
                }
            }
        }
        private void cbSelectAllVehicle_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto == false)
            {
                isCheckByAuto = true;
                if (VehicleList != null)
                {
                    if (e.Value == true)
                    {

                        foreach (var item in VehicleList)
                        {
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in VehicleList)
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

        #region [Xử lý CustomerTypeList]
        private void btnSelectItemCustomerType_Clicked(object sender, EventArgs e)
        {
            if (CustomerTypeList != null)
            {
                DataCustomerType = new List<string>();
                DataCustomerType.Clear();
                btnInCustomerTypeList.Text = "";
                inCustomerTypeList = "";
                foreach (var item in CustomerTypeList)
                {
                    if (item.IsSelectedCustomerType == true)
                    {
                        DataCustomerType.Add(item.NameCustomerType);
                        DataCustomerType.Add(item.IdCustomerType);
                    }
                }

                for (int i = 0; i < DataCustomerType.Count(); i+=2)
                {
                    if (i == DataCustomerType.Count() - 2)
                    {
                        btnInCustomerTypeList.Text += DataCustomerType[i];
                        inCustomerTypeList += DataCustomerType[i+1];
                    }
                    else
                    {
                        btnInCustomerTypeList.Text += DataCustomerType[i] + ",";
                        inCustomerTypeList += DataCustomerType[i + 1] + ",";
                    }
                }
                DataCustomerType.Clear();

                bool IsCheck = true;
                foreach (var item in CustomerTypeList)
                {
                    if (item.IsSelectedCustomerType == false)
                    {
                        IsCheck = false;
                        break;
                    }
                }

                if (IsCheck == false)
                    customerTypeListName = null;
                else
                {
                    customerTypeListName = "All";
                    btnInCustomerTypeList.Text = "All";
                }


                bool IsCheckFalse = true;
                foreach (var item in CustomerTypeList)
                {
                    if (item.IsSelectedCustomerType == true)
                    {
                        IsCheckFalse = false;
                        break;
                    }
                }
                if (IsCheckFalse == true)
                    btnInCustomerTypeList.Text = "-- Chọn loại khách --";

                grdIsBusy.IsVisible = false;

                stlSelectDataSearch.IsVisible = false;
                grdCustomerType.IsVisible = false;
            }
            BackToMainLparkingPage();
        }
        private void LstvCustomerType_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //bool Check = false;
            var selectedCustomerType = (DataSearchModel)LstvCustomerType.SelectedItem;
            if (selectedCustomerType != null)
            {
                if (CustomerTypeList != null)
                {
                    if (selectedCustomerType.IsSelectedCustomerType == true)
                    {
                        selectedCustomerType.IsSelectedCustomerType = false;
                    }
                    else
                    {
                        selectedCustomerType.IsSelectedCustomerType = true;

                    }
                }

                //foreach (var item in CustomerTypeList)
                //{
                //    if (item.IsSelectedCustomerType == true)
                //    {
                //        Check = true;
                //        break;
                //    }
                //}
                //if (Check == true)
                //    btnSelectItemCustomerType.IsEnabled = true;
                //else
                //    btnSelectItemCustomerType.IsEnabled = false;

            }
        }
        private void cbSelectAllCustomerType_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto == false)
            {
                isCheckByAuto = true;
                if (CustomerTypeList != null)
                {
                    if (e.Value == true)
                    {

                        foreach (var item in CustomerTypeList)
                        {
                            if (item.IsSelectedCustomerType == false)
                            {
                                item.IsSelectedCustomerType = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in CustomerTypeList)
                        {
                            if (item.IsSelectedCustomerType == true)
                            {
                                item.IsSelectedCustomerType = false;
                            }
                        }
                    }
                }
            }

            isCheckByAuto = false;
        }
        private void cbSelectCustomerType_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto)
                return;

            if (CustomerTypeList != null)
            {
                bool isCheckAll = true;
                foreach (var item in CustomerTypeList)
                {
                    if (item.IsSelectedCustomerType == false)
                    {
                        isCheckAll = false;
                        break;
                    }
                }

                if (cbSelectAllCustomerType.IsChecked != isCheckAll)
                {
                    isCheckByAuto = true;
                    cbSelectAllCustomerType.IsChecked = isCheckAll;
                }
            }
        }
        #endregion

        #region [Xử lý InPaymentTypeList]

        private void btnSelectItemInPaymentType_Clicked(object sender, EventArgs e)
        {
            if (InPaymentTypeList != null)
            {
                DataInPaymentType = new List<string>();
                DataInPaymentType.Clear();
                inPaymentTypeList = "";

                btnInPaymentTypeList.Text = "";
                foreach (var item in InPaymentTypeList)
                {
                    if (item.IsSelectedInPaymentType == true)
                    {
                        DataInPaymentType.Add(item.NameInPaymentType);
                        DataInPaymentType.Add(item.IdInPaymentType);
                    }
                }

                for (int i = 0; i < DataInPaymentType.Count(); i += 2)
                {
                    if (i == DataInPaymentType.Count() - 2)
                    {
                        btnInPaymentTypeList.Text += DataInPaymentType[i];
                        inPaymentTypeList += DataInPaymentType[i + 1];
                    }
                    else
                    {
                        btnInPaymentTypeList.Text += DataInPaymentType[i] + ",";
                        inPaymentTypeList += DataInPaymentType[i + 1] + ",";
                    }
                }
                DataInPaymentType.Clear();

                bool IsCheck = true;
                foreach (var item in InPaymentTypeList)
                {
                    if (item.IsSelectedInPaymentType == false)
                    {
                        IsCheck = false;
                        break;
                    }
                }

                if (IsCheck == false)
                    paymentTypeListName = null;
                else
                {
                    paymentTypeListName = "All";
                    btnInPaymentTypeList.Text = "All";
                }


                bool IsCheckFalse = true;
                foreach (var item in InPaymentTypeList)
                {
                    if (item.IsSelectedInPaymentType == true)
                    {
                        IsCheckFalse = false;
                        break;
                    }
                }
                if (IsCheckFalse == true)
                    btnInPaymentTypeList.Text = "-- Chọn đối tượng --";

                grdIsBusy.IsVisible = false;
                stlSelectDataSearch.IsVisible = false;
            }
            BackToMainLparkingPage();
        }
        private void LstvInPaymentTypeList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //bool Check = false;
            var selectedinPaymentType = (DataSearchModel)LstvInPaymentTypeList.SelectedItem;

            if (selectedinPaymentType != null)
            {
                if (InPaymentTypeList != null)
                {
                    if (selectedinPaymentType.IsSelectedInPaymentType == true)
                    {
                        selectedinPaymentType.IsSelectedInPaymentType = false;
                    }
                    else
                    {
                        selectedinPaymentType.IsSelectedInPaymentType = true;

                    }
                }

                //foreach (var item in InPaymentTypeList)
                //{
                //    if (item.IsSelectedInPaymentType == true)
                //    {
                //        Check = true;
                //        break;
                //    }
                //}
                //if (Check == true)
                //    btnSelectItemInPaymentType.IsEnabled = true;
                //else
                //    btnSelectItemInPaymentType.IsEnabled = false;
            }
        }

        private void cbSelectAllInPaymentType_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto == false)
            {
                isCheckByAuto = true;
                if (InPaymentTypeList != null)
                {
                    if (e.Value == true)
                    {

                        foreach (var item in InPaymentTypeList)
                        {
                            if (item.IsSelectedInPaymentType == false)
                            {
                                item.IsSelectedInPaymentType = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in InPaymentTypeList)
                        {
                            if (item.IsSelectedInPaymentType == true)
                            {
                                item.IsSelectedInPaymentType = false;
                            }
                        }
                    }
                }
            }

            isCheckByAuto = false;
        }

        private void cbSelectInPaymentType_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (isCheckByAuto)
                return;

            if (InPaymentTypeList != null)
            {
                bool isCheckAll = true;
                foreach (var item in InPaymentTypeList)
                {
                    if (item.IsSelectedInPaymentType == false)
                    {
                        isCheckAll = false;
                        break;
                    }
                }

                if (cbSelectAllInPaymentType.IsChecked != isCheckAll)
                {
                    isCheckByAuto = true;
                    cbSelectAllInPaymentType.IsChecked = isCheckAll;
                }
            }
        }
        #endregion

        #region [Xử lý VehicleDataType]
        private void pkrVehicleDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (DataSearchModel)pkrVehicleDataType.SelectedItem;
            if (item != null)
            {
                vehicleDataType = item.IdVehicleDataType;
            }
           
        }
        #endregion

        #region [Xử lý SearchField]
        private void pkrSearchField_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (DataSearchModel)pkrSearchField.SelectedItem;
            if (item != null)
            {
                searchField = item.IdSearchField;
            }
           
        }

        #endregion

        private async void pkrPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            if (Start != true)
            {
                if (pkrPageSize.SelectedIndex == 5)
                {
                    stt = 0;
                    CurentPage = 1;
                    try
                    {
                        startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        if (chkVehicleNotCheckOut.IsChecked == false)
                        {
                            vehicleNotCheckOut = "0";
                        }
                        else
                            vehicleNotCheckOut = "1";


                        values = new Dictionary<string, string>();
                        values.Add("stationCodeListName", "All");
                        values.Add("startDate", startDate);
                        values.Add("endDate", endDate);
                        values.Add("vehicleTypeListName", vehicleTypeListName);//Loại đối tượng
                        values.Add("inVehicleTypeList", inVehicleTypeList);
                        values.Add("customerTypeListName", customerTypeListName);//Loại khách
                        values.Add("inCustomerTypeList", inCustomerTypeList);
                        values.Add("paymentTypeListName", paymentTypeListName);//Hình thức thanh toán
                        values.Add("inPaymentTypeList", inPaymentTypeList);
                        values.Add("vehicleDataType", vehicleDataType);//ALL: "<> 'X'", Loại dữ liệu
                        values.Add("searchField", searchField);//BSX: "5" Thông tin tìm kiếm
                        values.Add("searchContent", searchContent);// noi dung tim kiem
                        values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
                        values.Add("pageSize", "0");
                        values.Add("currentPage", CurentPage.ToString());

                        string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
                        var result = SendHttpPostRequest(url, values, 180000);
                        if (result != null)
                        {
                            var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                            if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                            {
                                ValList = responseData.data.ToList();

                                foreach (var item in ValList)
                                {
                                    stt += 1;
                                    item.Index = stt.ToString();
                                }
                                lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                                CountlData = responseData.RecordsTotal;
                               

                                MaxPage = 1;
                                lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                                btnNextPage.IsEnabled = false;
                                btnLastPage.IsEnabled = false;
                                btnFristPage.IsEnabled = false;
                                btnBackPage.IsEnabled = false;
                            }
                            else
                            {
                                lstvDuLieuVaoRaLParking.ItemsSource = "";
                                lblCurentPage.Text = "";
                                btnNextPage.IsEnabled = false;
                                btnFristPage.IsEnabled = false;
                                btnLastPage.IsEnabled = false;
                                btnBackPage.IsEnabled = false;

                            }
                        }
                        else
                        {
                            lstvDuLieuVaoRaLParking.ItemsSource = "";
                            lblCurentPage.Text = "";
                            btnNextPage.IsEnabled = false;
                            btnFristPage.IsEnabled = false;
                            btnLastPage.IsEnabled = false;
                            btnBackPage.IsEnabled = false;
                        }
                    }
                    catch (Exception ex)
                    {

                        DependencyService.Get<IMessage>().LongTime(ex.Message);
                    }

                }
                else
                {
                    stt = 0;
                    CurentPage = 1;
                    AddDataValues();
                    string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
                    var result = SendHttpPostRequest(url, values, 180000);
                    if (result != null)
                    {
                        var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                        if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                        {
                            ValList = responseData.data.ToList();

                            foreach (var item in ValList)
                            {
                                stt += 1;
                                item.Index = stt.ToString();
                            }
                            lstvDuLieuVaoRaLParking.ItemsSource = ValList;
                            CountlData = responseData.RecordsTotal;
                            MaxPage = (int)Math.Ceiling((double)CountlData / Int32.Parse(PageSize));
                            lblCurentPage.Text = "Trang " + CurentPage + "/ " + MaxPage;
                            btnFristPage.IsEnabled = false;
                            btnBackPage.IsEnabled = false;
                            btnNextPage.IsEnabled = true;
                            btnLastPage.IsEnabled = true;
                        }
                        else
                        {
                            lstvDuLieuVaoRaLParking.ItemsSource = "";
                            lblCurentPage.Text = "";
                            btnNextPage.IsEnabled = false;
                            btnFristPage.IsEnabled = false;
                            btnLastPage.IsEnabled = false;
                            btnBackPage.IsEnabled = false;

                        }
                    }
                    else
                    {
                        lstvDuLieuVaoRaLParking.ItemsSource = "";
                        lblCurentPage.Text = "";
                        btnNextPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = false;
                        btnBackPage.IsEnabled = false;
                    }
                }
            }
            Start = false;
            await PopupNavigation.Instance.PopAllAsync();

        }
    }
}