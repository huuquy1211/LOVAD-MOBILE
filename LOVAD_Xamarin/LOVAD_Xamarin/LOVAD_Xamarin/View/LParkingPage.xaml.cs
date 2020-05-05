using LOVAD_Xamarin.Model;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LParkingPage : MasterDetailPage
    {
        List<MenuItems> menu;
        UserModel _UserProfileModel;
        PlaceModel PlaceSelect;
        public LParkingPage(UserModel user, PlaceModel Place)
        {
            InitializeComponent();
            
            Global.Intance.masterDetailPage = this;
            _UserProfileModel = user;
            PlaceSelect = Place;



            if (PlaceSelect.TypePlace == 0)
                lblTypePlace.Text = "LParking";
            if (PlaceSelect.TypePlace == 1)
                lblTypePlace.Text = "Trạm cân";


            Global.Intance.SerIpAdressLParking = PlaceSelect.IpAddress;
            Global.Intance.SerPortAPILParking =  PlaceSelect.PortAPI.ToString();


            lblNamePlace.Text = PlaceSelect.Name;
            NavigationPage.SetHasNavigationBar(this, false);

            CheckUserLogin();//Kiểm tra quyền
        }

        private void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new LoadingView("search"));
            try
            {
                var item = e.Item as MenuItems;

                switch (item.OptionIndex)
                {
                    case 1:
                        {
                            Detail = new NavigationPage(new LParkingDataInAndOutPage());
                            IsPresented = false;
                        }
                        break;
                    case 2:
                        {
                            //string Err = "Chức năng sớm truy cập!";
                            //DependencyService.Get<IMessage>().LongTime(Err);
                            //IsPresented = false;
                            //PopupNavigation.Instance.PopAllAsync();
                            //return;
                            Detail = new NavigationPage(new LParkingBlackListPage());
                            IsPresented = false;
                        }
                        break;
                    case 3:
                        {
                            Detail = new NavigationPage(new LParkingReportRevenuePage());
                            IsPresented = false;
                        }
                        break;
                    case 4:
                        {
                            Detail = new NavigationPage(new LParkingCustomerPage());
                            IsPresented = false;
                        }
                        break;
                    case 5:
                        {
                            Detail = new NavigationPage(new LParkingListLostCardsPage());
                            IsPresented = false;
                           
                        }
                        break;
                    case 6:
                        {
                            Detail = new NavigationPage(new PlaceLParkingPage(_UserProfileModel, true));
                            IsPresented = false;
                        }
                        break;
                }
                
            }
            catch (Exception ex)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            PopupNavigation.Instance.PopAllAsync();
        }

        public async void CheckUserLogin()
        {
            string url = "http://" + Global.Intance.SerIpAdress + ":" + Global.Intance.SerPortAPI + "/api/GetRole?UserName=" + _UserProfileModel.UserName;
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();
                var responseData = Int32.Parse(result);

                //var message = responseData.Content;
                if (responseData == 1)
                {
                    menu = new List<MenuItems>();

                    menu.Add(new MenuItems { OptionName = "Dữ liệu vào ra", OptionIndex = 1 });
                    menu.Add(new MenuItems { OptionName = "Danh sách đen", OptionIndex = 2 });
                    menu.Add(new MenuItems { OptionName = "Báo cáo doanh thu", OptionIndex = 3 });
                    menu.Add(new MenuItems { OptionName = "Khách hàng", OptionIndex = 4 });
                    menu.Add(new MenuItems { OptionName = "Danh sách thẻ mất", OptionIndex = 5 });
                    menu.Add(new MenuItems { OptionName = "Quay lại danh sách cơ sở", OptionIndex = 6 });
                    navigationList.ItemsSource = menu;
                    Detail = new NavigationPage(new LParkingDataInAndOutPage());
                }
                else if (responseData == 2)
                {
                    menu = new List<MenuItems>();

                    menu.Add(new MenuItems { OptionName = "Dữ liệu vào ra", OptionIndex = 1 });
                    menu.Add(new MenuItems { OptionName = "Danh sách đen", OptionIndex = 2 });
                    menu.Add(new MenuItems { OptionName = "Báo cáo doanh thu", OptionIndex = 3 });
                    menu.Add(new MenuItems { OptionName = "Khách hàng", OptionIndex = 4 });
                    menu.Add(new MenuItems { OptionName = "Danh sách thẻ mất", OptionIndex = 5 });
                    menu.Add(new MenuItems { OptionName = "Quay lại danh sách cơ sở", OptionIndex = 6 });
                    navigationList.ItemsSource = menu;
                    Detail = new NavigationPage(new LParkingDataInAndOutPage());
                }
                else if (responseData == 3)
                {
                    menu = new List<MenuItems>();

                    menu.Add(new MenuItems { OptionName = "Dữ liệu vào ra", OptionIndex = 1 });
                    //menu.Add(new MenuItems { OptionName = "Danh sách đen", OptionIndex = 2 });
                    menu.Add(new MenuItems { OptionName = "Báo cáo doanh thu", OptionIndex = 3 });
                    //menu.Add(new MenuItems { OptionName = "Khách hàng", OptionIndex = 4 });
                    //menu.Add(new MenuItems { OptionName = "Danh sách thẻ mất", OptionIndex = 5 });
                    menu.Add(new MenuItems { OptionName = "Quay lại danh sách cơ sở", OptionIndex = 6 });
                    navigationList.ItemsSource = menu;
                    Detail = new NavigationPage(new LParkingDataInAndOutPage());

                }
                else if (responseData == 4)
                {
                    menu = new List<MenuItems>();
                    menu.Add(new MenuItems { OptionName = "Quay lại danh sách cơ sở", OptionIndex = 6 });
                    navigationList.ItemsSource = menu;
                    return;
                    //Detail = new NavigationPage(new LParkingDataInAndOutPage());
                }
                else if (responseData == 5)
                {
                    await DisplayAlert("Thông báo", "Không tìm thấy tài khoản!", "OK");
                }
                else if (responseData == 6)
                {
                    await DisplayAlert("Thông báo", "Tài khoản này không được quyền thao tác trên ứng dụng!", "OK");
                }
            }
            catch (Exception ex)
            {
                var Err = "Không nết nối được máy chủ";
                DependencyService.Get<IMessage>().LongTime(Err);
            }
        }
    }
    public class MenuItems
    {
        public string OptionName { get; set; }
        public int OptionIndex { get; set;}
    }
}