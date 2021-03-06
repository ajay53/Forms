﻿using Forms.Utility;
using Forms.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel(parentView);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            UtilityFunction.ToastMessage("asdfg");
        }
    }
}