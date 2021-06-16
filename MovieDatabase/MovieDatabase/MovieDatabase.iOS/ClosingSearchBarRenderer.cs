using Foundation;
using MovieDatabase.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//Egzamin
namespace MovieDatabase.iOS
{
    public class ClosingSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var searchBar = (UISearchBar)Control;
            var control = this.Element as ClosingSearchBar;

            control.Unfocused += Control_Unfocused;

            //???
            searchBar.SetImageforSearchBarIcon(UIImage.FromBundle("ic_action_arrow_back.png"), UISearchBarIcon.Search, UIControlState.Normal);


            searchBar.TintColor = control.ActualColor.ToUIColor();
        }

        private void Control_Unfocused(object sender, FocusEventArgs e)
        {
            var searchBar = (UISearchBar)Control;
            var control = this.Element as ClosingSearchBar;

            if (searchBar.Text.Length == 0)
                control.Text = "";
                control.ShouldClosed = false;
        }
    }
}