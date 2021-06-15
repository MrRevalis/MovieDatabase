using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MovieDatabase.CustomControls
{
    public class ClosingSearchBar : SearchBar
    {
        public Color ActualColor
        {
            get { return (Color)GetValue(ActualColorProperty); }
            set { SetValue(ActualColorProperty, value); }
        }

        public static readonly BindableProperty ActualColorProperty =
            BindableProperty.Create("ActualColor", typeof(Color), typeof(ClosingSearchBar), Color.White);


        public bool ShouldClosed
        {
            get { return (bool)GetValue(ShouldClosedProperty); }
            set { SetValue(ShouldClosedProperty, value); }
        }

        public static readonly BindableProperty ShouldClosedProperty =
            BindableProperty.Create("ShouldClosed", typeof(bool), typeof(ClosingSearchBar), default(bool));
    }
}
