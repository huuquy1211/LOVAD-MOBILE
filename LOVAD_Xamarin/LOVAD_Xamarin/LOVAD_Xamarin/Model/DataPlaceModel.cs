using LOVAD_Xamarin.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class DataPlaceModel: BaseViewModel
    {
        public bool Result { get; set; }
        public double CountPage { get; set; }
        public List<PlaceModel> LstPlaces { get; set; }
    }
}
