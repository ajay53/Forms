using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Forms.Utility
{
    class MessagingCenterHelper
    {
        public void SendMessageMenu(string data)
        {
            MessagingCenter.Send(this, "Sample", data);
        }
    }
}
