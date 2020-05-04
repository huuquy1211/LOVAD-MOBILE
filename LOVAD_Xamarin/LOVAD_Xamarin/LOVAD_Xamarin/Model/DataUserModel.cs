using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class DataUserModel
    {
        public bool Result { get; set; }
        public double CountPage { get; set; }
        public int TotalUser { get; set; }
        public List<UserModel> ListUser { get; set; }
    }
}
