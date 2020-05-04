using LOVAD_Xamarin.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TramCanDataPage : ContentPage
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

        private string _inPaymentTypeList;
        public string inPaymentTypeList { get { return _inPaymentTypeList; } set { _inPaymentTypeList = value; OnPropertyChanged("inPaymentTypeList"); } }

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


        #endregion

        public long CountlData = 0;
        public int CurentPage = 1;
        public long stt = 0;
        public int TotalCountEnd = 0;
        public bool flag = false;
        public bool flagViewImage = false;

        public List<ValueLParkingModel> ValList = new List<ValueLParkingModel>();
        public DataImageLParkingModel dataImageLParkingModel = new DataImageLParkingModel();
        public TramCanDataPage()
        {
            InitializeComponent();

            BindingContext = this;
            PageSize = "30";
            pkrSearchField.SelectedIndex = 0;
            pkrStationCodeListName.SelectedIndex = 0;
            pkrVehicleDataType.SelectedIndex = 0;
            pkrCustomerTypeListName.SelectedIndex = 0;
            pkrInPaymentTypeList.SelectedIndex = 0;
            vehicleNotCheckOut = "0";
            searchContent = "";
            GetDataTramCan(PageSize);
            btnBackPage.IsEnabled = false;
        }

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
        private void btnSeach_Clicked(object sender, EventArgs e)
        {
            stt = 0;
            CurentPage = 1;
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            if (vehicleNotCheckOut.ToString() == "true")
            {
                vehicleNotCheckOut = "1";
            }
            else
                vehicleNotCheckOut = "0";

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
            values.Add("vehicleDataType", vehicleDataType);
            values.Add("searchField", searchField);
            values.Add("searchContent", searchContent);
            values.Add("stationCodeListName", "All");
            values.Add("vehicleTypeListName", null);
            values.Add("inVehicleTypeList", "");
            values.Add("customerTypeListName", customerTypeListName);
            values.Add("inCustomerTypeList", "");
            values.Add("paymentTypeListName", null);
            values.Add("inPaymentTypeList", "");
            values.Add("pageSize", PageSize);
            values.Add("currentPage", CurentPage.ToString());


            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData.data.Count != 0)
                {
                    ValList = responseData.data.ToList();

                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }
                    lstvDataTramCan.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage;
                    CountlData = responseData.RecordsTotal;
                    btnNextPage.IsEnabled = true;
                    btnLastPage.IsEnabled = true;
                    var message = "Tìm dữ liệu thành công!";
                    DependencyService.Get<IMessage>().ShortTime(message);
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = false;
                    DependencyService.Get<IMessage>().LongTime(message);
                }
            }
            else
            {
                var message = "Dữ liệu rỗng!";
                btnNextPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
                btnBackPage.IsEnabled = false;
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

        public void GetDataTramCan(string PageSize)
        {
            CurentPage = 1;
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            if (vehicleNotCheckOut.ToString() == "true")
            {
                vehicleNotCheckOut = "1";
            }
            else
                vehicleNotCheckOut = "0";
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
            values.Add("vehicleDataType", vehicleDataType);
            values.Add("searchField", searchField);
            values.Add("searchContent", searchContent);
            values.Add("stationCodeListName", "All");
            values.Add("vehicleTypeListName", null);
            values.Add("inVehicleTypeList", "");
            values.Add("customerTypeListName", customerTypeListName);
            values.Add("inCustomerTypeList", "");
            values.Add("paymentTypeListName", null);
            values.Add("inPaymentTypeList", "");
            values.Add("pageSize", PageSize);
            values.Add("currentPage", CurentPage.ToString());

            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData.data.Count != 0 && responseData != null)
                {
                    ValList = responseData.data.ToList();

                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }
                    lstvDataTramCan.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage;
                    CountlData = responseData.RecordsTotal;
                    btnNextPage.IsEnabled = true;
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = false;
                    DependencyService.Get<IMessage>().LongTime(message);
                }
            }
            else
            {
                var message = "Dữ liệu rỗng!";
                btnNextPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
                btnBackPage.IsEnabled = false;
                DependencyService.Get<IMessage>().LongTime(message);
            }
        }

        private void btnNextPage_Clicked(object sender, EventArgs e)
        {
            btnBackPage.IsEnabled = true;
            CurentPage++;

            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            if (vehicleNotCheckOut.ToString() == "true")
            {
                vehicleNotCheckOut = "1";
            }
            else
                vehicleNotCheckOut = "0";
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            values.Add("vehicleNotCheckOut", "0");
            values.Add("vehicleDataType", "0");
            values.Add("searchField", "0");
            values.Add("searchContent", "");
            values.Add("stationCodeListName", stationCodeListName);
            values.Add("vehicleTypeListName", null);
            values.Add("inVehicleTypeList", "");
            values.Add("customerTypeListName", null);
            values.Add("inCustomerTypeList", "");
            values.Add("paymentTypeListName", null);
            values.Add("inPaymentTypeList", "");
            values.Add("pageSize", PageSize);
            values.Add("currentPage", CurentPage.ToString());


            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData.data.Count != 0 && responseData != null)
                {
                    ValList = responseData.data.ToList();

                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }

                    lstvDataTramCan.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage;
                    btnFristPage.IsEnabled = true;
                    if (stt == responseData.RecordsTotal)
                        btnNextPage.IsEnabled = false;
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    btnNextPage.IsEnabled = false;
                    DependencyService.Get<IMessage>().LongTime(message);
                }
                if (CurentPage == (int)Math.Ceiling((double)CountlData / Int32.Parse(PageSize)))
                {
                    btnNextPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnFristPage.IsEnabled = true;
                    btnBackPage.IsEnabled = true;
                }
            }
            else
            {
                var message = "Dữ liệu rỗng!";
                btnNextPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
                btnLastPage.IsEnabled = false;
                btnBackPage.IsEnabled = false;
                DependencyService.Get<IMessage>().LongTime(message);
            }
        }

        private void btnBackPage_Clicked(object sender, EventArgs e)
        {
            btnNextPage.IsEnabled = true;
            if (CurentPage > 0)
            {
                CurentPage--;
                startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
                endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
                if (vehicleNotCheckOut.ToString() == "true")
                {
                    vehicleNotCheckOut = "1";
                }
                else
                    vehicleNotCheckOut = "0";
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("startDate", startDate);
                values.Add("endDate", endDate);
                values.Add("vehicleNotCheckOut", "0");
                values.Add("vehicleDataType", "0");
                values.Add("searchField", "0");
                values.Add("searchContent", "");
                values.Add("stationCodeListName", stationCodeListName);
                values.Add("vehicleTypeListName", null);
                values.Add("inVehicleTypeList", "");
                values.Add("customerTypeListName", null);
                values.Add("inCustomerTypeList", "");
                values.Add("paymentTypeListName", null);
                values.Add("inPaymentTypeList", "");
                values.Add("pageSize", PageSize);
                values.Add("currentPage", CurentPage.ToString());


                string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
                var result = SendHttpPostRequest(url, values, 180000);
                if (result != null)
                {
                    var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                    if (responseData.data.Count != 0 && responseData != null)
                    {
                        ValList = responseData.data.ToList();
                        long i = 1;
                        long count = stt;

                        if (((double)CountlData / Int32.Parse(PageSize)) % 2 != 0 && flag == true)
                        {
                            count = count + (Int32.Parse(PageSize) - TotalCountEnd);
                            foreach (var item in ValList)
                            {
                                stt = count - ((Int32.Parse(PageSize) * 2) - i);
                                i++;
                                item.Index = stt.ToString();
                            }
                            flag = false;
                        }
                        else
                        {
                            foreach (var item in ValList)
                            {
                                stt = count - ((Int32.Parse(PageSize) * 2) - i);
                                i++;
                                item.Index = stt.ToString();
                            }
                        }

                        lstvDataTramCan.ItemsSource = ValList;
                        lblCurentPage.Text = "Trang " + CurentPage;
                        btnLastPage.IsEnabled = true;
                        btnNextPage.IsEnabled = true;
                    }
                    else
                    {
                        var message = "Dữ liệu rỗng!";
                        btnNextPage.IsEnabled = false;
                        btnFristPage.IsEnabled = false;
                        btnLastPage.IsEnabled = false;
                        btnBackPage.IsEnabled = false;
                        DependencyService.Get<IMessage>().LongTime(message);
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
            if (CurentPage == 1)
            {
                btnBackPage.IsEnabled = false;
                btnFristPage.IsEnabled = false;
            }

        }

        private void btnFristPage_Clicked(object sender, EventArgs e)
        {
            flag = false;
            stt = 0;
            CurentPage = 1;
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            if (vehicleNotCheckOut.ToString() == "true")
            {
                vehicleNotCheckOut = "1";
            }
            else
                vehicleNotCheckOut = "0";
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
            values.Add("vehicleDataType", vehicleDataType);
            values.Add("searchField", searchField);
            values.Add("searchContent", searchContent);
            values.Add("stationCodeListName", "All");
            values.Add("vehicleTypeListName", null);
            values.Add("inVehicleTypeList", "");
            values.Add("customerTypeListName", customerTypeListName);
            values.Add("inCustomerTypeList", "");
            values.Add("paymentTypeListName", null);
            values.Add("inPaymentTypeList", "");
            values.Add("pageSize", PageSize);
            values.Add("currentPage", CurentPage.ToString());

            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData.data.Count != 0 && responseData != null)
                {
                    ValList = responseData.data.ToList();

                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }
                    lstvDataTramCan.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage;
                    CountlData = responseData.RecordsTotal;
                    btnNextPage.IsEnabled = true;
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = false;
                    DependencyService.Get<IMessage>().LongTime(message);
                }
                btnLastPage.IsEnabled = true;
                btnNextPage.IsEnabled = true;
                btnFristPage.IsEnabled = false;
                btnBackPage.IsEnabled = false;
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

        private void btnLastPage_Clicked(object sender, EventArgs e)
        {
            flag = true;//Người dùng bấm vào nút cuối trang
            int page = (int)(CountlData / Int32.Parse(PageSize));
            //Làm tròn lên
            CurentPage = (int)Math.Ceiling((double)CountlData / Int32.Parse(PageSize));
            stt = page * Int32.Parse(PageSize);
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            if (vehicleNotCheckOut.ToString() == "true")
            {
                vehicleNotCheckOut = "1";
            }
            else
                vehicleNotCheckOut = "0";
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            values.Add("vehicleNotCheckOut", vehicleNotCheckOut);
            values.Add("vehicleDataType", vehicleDataType);
            values.Add("searchField", searchField);
            values.Add("searchContent", searchContent);
            values.Add("stationCodeListName", "All");
            values.Add("vehicleTypeListName", null);
            values.Add("inVehicleTypeList", "");
            values.Add("customerTypeListName", customerTypeListName);
            values.Add("inCustomerTypeList", "");
            values.Add("paymentTypeListName", null);
            values.Add("inPaymentTypeList", "");
            values.Add("pageSize", PageSize);
            values.Add("currentPage", CurentPage.ToString());

            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/search-vehicle-data";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData.data.Count != 0 && responseData != null)
                {
                    ValList = responseData.data.ToList();
                    TotalCountEnd = ValList.Count();
                    foreach (var item in ValList)
                    {
                        stt += 1;
                        item.Index = stt.ToString();
                    }
                    lstvDataTramCan.ItemsSource = ValList;
                    lblCurentPage.Text = "Trang " + CurentPage;

                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = true;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = true;
                }
                else
                {
                    var message = "Dữ liệu rỗng!";
                    btnNextPage.IsEnabled = false;
                    btnFristPage.IsEnabled = false;
                    btnLastPage.IsEnabled = false;
                    btnBackPage.IsEnabled = false;
                    DependencyService.Get<IMessage>().LongTime(message);
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

        private void lstvDuLieuVaoRaLParking_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var DataLParkingSelect = (ValueLParkingModel)lstvDataTramCan.SelectedItem;
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("InFrontImagePath", DataLParkingSelect.InFrontImagePath);
            values.Add("InBackImagePath", DataLParkingSelect.InBackImagePath);
            values.Add("InFrontScanImagePath", DataLParkingSelect.InFrontScanImagePath);
            values.Add("InBackScanImagePath", DataLParkingSelect.InBackScanImagePath);
            values.Add("OutFrontImagePath", DataLParkingSelect.OutFrontImagePath);
            values.Add("OutBackImagePath", DataLParkingSelect.OutBackImagePath);
            values.Add("OutFrontScanImagePath", DataLParkingSelect.OutFrontScanImagePath);
            values.Add("OutBackScanImagePath", DataLParkingSelect.OutBackScanImagePath);

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
                var message = "Hình ảnh không đầy đủ!";
                DependencyService.Get<IMessage>().LongTime(message);
                return "NoImage.png";
            }


        }

        private void imgInBefore_Tapped(object sender, EventArgs e)
        {
            flagViewImage = true;
            this.IsBusy = true;
            PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.InFrontImagePath));
        }

        private void imgInAfter_Tapped(object sender, EventArgs e)
        {
            flagViewImage = true;
            this.IsBusy = true;
            PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.OutFrontImagePath));
        }

        private void imgOutBefore_Tapped(object sender, EventArgs e)
        {
            flagViewImage = true;
            this.IsBusy = true;
            PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.InBackImagePath));
        }

        private void imgOutAfter_Tapped(object sender, EventArgs e)
        {
            flagViewImage = true;
            this.IsBusy = true;
            PopupNavigation.Instance.PushAsync(new ImageView(dataImageLParkingModel.data.OutBackImagePath));
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