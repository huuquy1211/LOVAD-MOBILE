using LOVAD_Xamarin.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class DataSearchModel : BaseViewModel
    {
        #region [VehicleList]
        public string Id { get; set; }
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }

        private bool _isSelected;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged("IsSelected"); } }
        #endregion

        #region [CustomerTypeListName]
        public string IdCustomerType { get; set; }

        private string _nameCustomerType;
        public string NameCustomerType { get { return _nameCustomerType; } set { _nameCustomerType = value; OnPropertyChanged("NameCustomerType"); } }

        private bool _isSelectedCustomerType;
        public bool IsSelectedCustomerType { get { return _isSelectedCustomerType; } set { _isSelectedCustomerType = value; OnPropertyChanged("IsSelectedCustomerType"); } }

        #endregion

        #region [InPaymentTypeList]
        public string IdInPaymentType { get; set; }

        private string _nameInPaymentType;
        public string NameInPaymentType { get { return _nameInPaymentType; } set { _nameInPaymentType = value; OnPropertyChanged("NameInPaymentType"); } }

        private bool _isSelectedInPaymentType;
        public bool IsSelectedInPaymentType { get { return _isSelectedInPaymentType; } set { _isSelectedInPaymentType = value; OnPropertyChanged("IsSelectedInPaymentType"); } }

        #endregion

        #region [VehicleDataType]
        public string IdVehicleDataType { get; set; }
        private string _nameVehicleDataType;
        public string NameVehicleDataType { get { return _nameVehicleDataType; } set { _nameVehicleDataType = value; OnPropertyChanged("NameVehicleDataType"); } }

        #endregion

        #region [SearchField]
        public string IdSearchField { get; set; }
        private string _nameSearchField;
        public string NameSearchField { get { return _nameSearchField; } set { _nameSearchField = value; OnPropertyChanged("NameSearchField"); } }

        #endregion

        #region [CartType]
        public string IdCartType { get; set; }
        private string _nameCartType;
        public string NameCartType { get { return _nameCartType; } set { _nameCartType = value; OnPropertyChanged("NameCartType"); } }

        #endregion

    }
}
