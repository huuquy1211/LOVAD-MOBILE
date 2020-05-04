using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class DataImageLParkingModel
    {
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public ValueImageLParkingModel data { get; set; }
        public string file { get; set; }
    }
}
