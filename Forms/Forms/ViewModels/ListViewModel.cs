using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Forms.ViewModels
{
    class ListViewModel
    {
        ListView listView;

        public ListViewModel(ListView listView)
        {
            this.listView = listView;
            //listView.ItemsSource = items;
        }
    }
}
