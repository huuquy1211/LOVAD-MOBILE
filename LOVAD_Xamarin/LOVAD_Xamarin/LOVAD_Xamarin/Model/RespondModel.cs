using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class RespondModel
    {
        public bool Result { get; set; }
        public List<string> Content { get; set; }
        public UserModel UserRespond { get; set; }
        public RespondModel()
        {
            Result = true;
            Content = new List<string>();
            UserRespond = null;
        }
    }

    

}
