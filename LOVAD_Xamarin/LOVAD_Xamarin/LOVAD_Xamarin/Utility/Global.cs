using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace LOVAD_Xamarin.Model
{
    public class Global
    {
        private static object syncLock = new object();
       

        private static Global _intance;
        public static Global Intance 
        {
            get 
            {
                lock (syncLock)
                {
                    if (_intance == null)
                    {
                        _intance = new Global();
                    }
                }
                return _intance;
            } 
        }
        public string Cookie = "";
        public string SerIpAdress = "192.168.1.140";
        public string SerPortAPI = "5001";

        public string SerIpAdressLParking = "192.168.1.51";
        public string SerPortAPILParking = "10080";

        //Tổng cơ sở
        public string untPlace;
        //Tổng xe vào
        public string CountVehicleIn;
        //Tổng xe ra
        public string CountVehicleOut;
        //Tổng doanh thu
        public string TotalRevenue;

        public MasterDetailPage masterDetailPage = new MasterDetailPage();
    }
}
