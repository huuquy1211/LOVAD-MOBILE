using LOVAD_Xamarin.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class PlaceModel : BaseViewModel
    {
        

        public string Id { get; set; }
        public int TypePlace { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string IpAddress { get; set; }
        public int PortDB { get; set; }
        public string UserNameDB { get; set; }
        public string PassWordDB { get; set; }
        public int PortAPI { get; set; }
        public string UserNameAPI { get; set; }
        public string PassWordAPI { get; set; }
        public double CountPage { get; set; }

        private bool _isSelected;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged("IsSelected"); } }
    }
}
