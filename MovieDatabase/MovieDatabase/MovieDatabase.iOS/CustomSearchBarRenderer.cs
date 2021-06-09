using Foundation;
using MovieDatabase.CustomControls;
using MovieDatabase.iOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]

namespace MovieDatabase.iOS
{
    class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var searchBar = (UISearchBar)Control;
            var control = this.Element as CustomSearchBar;

            /*Foundation.NSString _searchField = new Foundation.NSString("searchField");
            var textFieldInsideSearchBar = (UITextField)searchBar.ValueForKey(_searchField);
            textFieldInsideSearchBar.TextColor = control.ActualColor.ToUIColor();*/

            searchBar.TintColor = control.ActualColor.ToUIColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var searchBar = (UISearchBar)Control;
            var control = this.Element as CustomSearchBar;

            searchBar.TintColor = control.ActualColor.ToUIColor();
        }
    }
}