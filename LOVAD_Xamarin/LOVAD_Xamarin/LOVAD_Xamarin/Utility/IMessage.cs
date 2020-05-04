using System;
using System.Collections.Generic;
using System.Text;

namespace LOVAD_Xamarin.Model
{
    public interface IMessage
    {
        void LongTime(string message);
        void ShortTime(string message);
    }
}
