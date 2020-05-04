using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class RespondPlaceModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public object ObjRespond { get; set; }
        public double CountPage { get; set; }
        public List<string> Content { get; set; }
        public List<PlaceModel> LstPlaces { get; set; }
        public RespondPlaceModel()
        {
            Result = false;
            ObjRespond = new object();
            Content = new List<string>();
        }
    }
}
