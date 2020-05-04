using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class ValueLParkingModel
    {
       public string Index { get; set; }
       public string StationName { get; set; }
       public string CheckInUser { get; set; }
       public string CardCode { get; set; }
       public string InvoiceNumber { get; set; }
       public string VehicleName { get; set; }
       public string VehicleNumberPlate { get; set; }
       public string TimeIn { get; set; }
       public string TimeOut { get; set; }
       public string ParkingTime { get; set; }
       public string CardType { get; set; }
       public string Price { get; set; }

        public string InFrontImagePath //khuon mat vao
        {
            get;
            set;
        }

        public string InBackImagePath // bien so vao
        {
            get;
            set;
        }

        public string InFrontScanImagePath //scan 1
        {
            get;
            set;
        }

        public string InBackScanImagePath //scan 2
        {
            get;
            set;
        }

        public string OutFrontImagePath //khuon mat ra
        {
            get;
            set;
        }

        public string OutBackImagePath //bien so ra
        {
            get;
            set;
        }

        public string OutFrontScanImagePath
        {
            get;
            set;
        }

        public string OutBackScanImagePath
        {
            get;
            set;
        }

        #region [LoseCard]

        #endregion

    }
}
