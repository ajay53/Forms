using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CrossWordHelper
{
   public class MessageCenterHelper
    {
        public void SendMessageMenu(Type PageObj)
        {
            MessagingCenter.Send(this,"Sample", PageObj);
        }
    }
}
