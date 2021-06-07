using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MovieDatabase.CustomControls
{
    public class CustomSearchBar : SearchBar
    {
        public Color ActualColor
        {
            get { return (Color)GetValue(ActualColorProperty); }
            set { SetValue(ActualColorProperty, value); }
        }

        public static readonly BindableProperty ActualColorProperty =
            BindableProperty.Create("ActualColor", typeof(Color), typeof(CustomSearchBar), Color.White);
    }
}
