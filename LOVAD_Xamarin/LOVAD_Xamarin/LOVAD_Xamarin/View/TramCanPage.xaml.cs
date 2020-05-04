using LOVAD_Xamarin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOVAD_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TramCanPage : MasterDetailPage
    {
        List<MenuItems> menu;
        UserModel _UserProfileModel = new UserModel();
        public TramCanPage(UserModel user)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _UserProfileModel = user;
            menu = new List<MenuItems>();

            menu.Add(new MenuItems { OptionName = "Thống kê dữ liệu", OptionIndex = 1 });
            menu.Add(new MenuItems { OptionName = "Danh sách cơ sở", OptionIndex = 2 });
           
            navigationList.ItemsSource = menu;

            Detail = new NavigationPage(new TramCanDataPage());

        }

        private void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as MenuItems;

                switch (item.OptionIndex)
                {
                    case 1:
                        {
                            Detail = new NavigationPage(new TramCanDataPage());
                            IsPresented = false;
                        }
                        break;
                }
                switch (item.OptionIndex)
                {
                    case 2:
                        {
                            Detail = new NavigationPage(new PlaceTramCanPage(_UserProfileModel, true));
                            IsPresented = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public class MenuItems
        {
            public string OptionName { get; set; }
            public int OptionIndex { get; set; }
        }
    }
}