using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public class UserProfileModel
    {
        public string id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirm { get; set; }
        public string Phone { get; set; }
        
        //public UserProfileModel()
        //{
        //    UserId = "";
        //    UserName = "";
        //    FullName = "";
        //    Email = "";
        //    EmailConfirm = false;
        //    Phone = "";
        //}
    }
}
