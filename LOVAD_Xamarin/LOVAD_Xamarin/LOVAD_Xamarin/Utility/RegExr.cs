using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOVAD_Xamarin.Model
{
    public class RegExr
    {
       
        public bool UsernameReg(string s)
        {
            if(s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");

                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra tài khoản!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra tài khoản Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
            
            
        }
        public bool PhoneNumberReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^(0|\+84)(\s|\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.)?(\d{3})(\s|\.)?(\d{3})$");

                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra số điện thoại!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra số điện thoại Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
           
        }

        public bool IpAddressReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^[-a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶ" +
               "ẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợ" +
               "ụủứừỬỮỰỲỴÝỶỸửữựỳýỵỷỹ\\s0-9_.,/]+$");
                    //return Regex.IsMatch(s, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra địa chỉ IP!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra địa chỉ Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
        }
        public bool AddressReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^[-a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶ" +
               "ẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợ" +
               "ụủứừỬỮỰỲỴÝỶỸửữựỳýỵỷỹ\\s0-9_.,/]+$");
                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra địa chỉ!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra địa chỉ Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
        }

        public bool NameReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^[a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶ" +
              "ẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽếềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợ" +
              "ụủứừỬỮỰỲỴÝỶỸửữựỳýỵỷỹ\\s]+$");
                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra tên!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra tên Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
          
        }

        public bool EmailReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");

                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra Email!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra Email Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
            
        }

        public bool PasswordReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$");

                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra mật khẩu!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra mật khẩu Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }
           
        }

        public bool NumberReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^\d+$");

                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra số!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra số Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }

        }

        public bool StringOrNumberReg(string s)
        {
            if (s != null)
            {
                try
                {
                    return Regex.IsMatch(s, @"^[a-zA-Z0-9]+$");

                }
                catch (Exception)
                {
                    string ex = "Lỗi kiểm tra tài khoản Database!";
                    DependencyService.Get<IMessage>().LongTime(ex.ToString());
                    return false;
                }
            }
            else
            {
                string ex = "Lỗi kiểm tra tài khoản Database Null!";
                DependencyService.Get<IMessage>().LongTime(ex.ToString());
                return false;
            }


        }
    }
}
