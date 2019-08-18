using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CrossWordHelper
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("XamarinDocs.TouchEffect")
        {
        }

        public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}
