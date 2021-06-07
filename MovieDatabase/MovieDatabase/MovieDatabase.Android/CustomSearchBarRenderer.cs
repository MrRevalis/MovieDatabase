using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MovieDatabase.CustomControls;
using MovieDatabase.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace MovieDatabase.Droid
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        CustomSearchBar searchBar;
        public CustomSearchBarRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            searchBar = this.Element as CustomSearchBar;

            var searchIconId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchIconId > 0)
            {
                var searchPlateIcon = Control.FindViewById(searchIconId);
                (searchPlateIcon as ImageView).SetColorFilter(searchBar.ActualColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            }

            int searchCloseIconId = Control.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            if (searchCloseIconId > 0)
            {
                var closeIcon = Control.FindViewById(searchCloseIconId);
                (closeIcon as ImageView).SetColorFilter(searchBar.ActualColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            }

            LinearLayout linearLayout = Control.GetChildAt(0) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(1) as LinearLayout;
            if (linearLayout != null)
            {
                linearLayout.Background.ClearColorFilter();
                linearLayout.Background.SetColorFilter(searchBar.ActualColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            }
        }
    }
}