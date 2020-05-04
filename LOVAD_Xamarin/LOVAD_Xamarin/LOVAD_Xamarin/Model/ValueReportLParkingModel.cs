using LOVAD_Xamarin.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class ValueReportLParkingModel : BaseViewModel
    {
       

        #region [Data StationCode]
        public string Id { get; set; }
        public string Name { get; set; }

        private bool _isSelected = true;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged("IsSelected"); } }
        #endregion
        public string StationName { get; set; }
        public string CheckUser { get; set; }
        public string VehicleType { get; set; }
        public string CustomerType { get; set; }
        //Tổng xe vào
        public int InCount { get; set; }
        //Tổng xe ra
        public int OutCount { get; set; }
        //Tổng tiền
        public double TotalPrice { get; set; }

        public int CustomerTypeId { get; set; }
        public Guid VehicleTypeId { get; set; }
    }
}
