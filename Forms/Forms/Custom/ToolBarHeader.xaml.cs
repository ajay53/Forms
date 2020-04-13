using Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolBarHeader : Grid
    {
        public ToolBarHeader()
        {
            InitializeComponent();
            BindingContext = new BaseViewModel();
        }
    }
}