using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class WorkPlaceModel
    {
        public string Id_User { get; set; }
        public string UserId { get; set; }
        public string Id_Place { get; set; }

        public List<string> ListPlaceId = new List<string>();
    }
}
