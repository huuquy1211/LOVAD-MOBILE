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
    public partial class LParkingBlackListPage : ContentPage
    {
        private string _pageSize;
        public string PageSize { get { return _pageSize; } set { _pageSize = value; OnPropertyChanged("PageSize"); } }

        private string _startDate;
        public string startDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("startDate"); } }

        private string _endDate;
        public string endDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("endDate"); } }
        private string _start;
        public string Start { get { return _start; } set { _start = value; OnPropertyChanged("Start"); } }
        private string _length;
        public string Length { get { return _length; } set { _length = value; OnPropertyChanged("Length"); } }


        private ObservableCollection<ValueLParkingModel> _listLParkingBlack;
        public ObservableCollection<ValueLParkingModel> ListLParkingBlack { get { return _listLParkingBlack; } set { _listLParkingBlack = value; OnPropertyChanged("ListLParkingBlack");}}

        public Dictionary<string, string> values;

        public LParkingBlackListPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetDataLParkingInAndOut();
            pkrPageSize.SelectedIndex = 1;
        }
        #region [Xử lý Data main page]
        public void GetDataLParkingInAndOut()
        {
            //CurentPage = 1;
            AddDataValues();
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/counting-in-out";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ListLParkingBlack = new ObservableCollection<ValueLParkingModel>(responseData.data);
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
            string url = "http://" + Global.Intance.SerIpAdressLParking + ":" + Global.Intance.SerPortAPILParking + "/api/counting-in-out";
            var result = SendHttpPostRequest(url, values, 180000);
            if (result != null)
            {
                var responseData = JsonConvert.DeserializeObject<DataLParkingModel>(result);
                if (responseData != null && responseData.data != null && responseData.data.Count != 0)
                {
                    ListLParkingBlack = new ObservableCollection<ValueLParkingModel>(responseData.data);
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
        public void AddDataValues()
        {
            Length = PageSize;
            Start = "0";
            startDate = pkrStartDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            endDate = pkrEndDate.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");

            values = new Dictionary<string, string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            values.Add("start", Start);
            values.Add("length", Length);

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


        #endregion

        private void pkrPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataLParkingInAndOut();
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