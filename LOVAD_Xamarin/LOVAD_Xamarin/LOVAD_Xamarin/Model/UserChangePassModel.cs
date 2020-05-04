using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class UserChangePassModel
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
