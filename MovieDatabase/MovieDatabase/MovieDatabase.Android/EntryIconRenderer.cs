using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using MovieDatabase.CustomControls;
using MovieDatabase.Droid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryIcon), typeof(EntryIconRenderer))]
namespace MovieDatabase.Droid
{
    class EntryIconRenderer : EntryRenderer
    {
        EntryIcon entryIcon;

        public EntryIconRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            entryIcon = this.Element as EntryIcon;

            FormsEditText editText = this.Control;
            if (!String.IsNullOrEmpty(entryIcon.Image))
            {
                switch (entryIcon.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(entryIcon.Image), null, null, null); break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(entryIcon.Image), null); break;
                }
            }
            editText.CompoundDrawablePadding = 25;

            GradientDrawable gradientDrawable = new GradientDrawable();
            //Zaokraglenie
            gradientDrawable.SetCornerRadius(Context.ToPixels(entryIcon.CornerRadius));
            //Obramowka
            gradientDrawable.SetStroke((int)Context.ToPixels(entryIcon.BorderThickness), entryIcon.BorderColor.ToAndroid());
            //Tlo elementu
            gradientDrawable.SetColor(entryIcon.BackgroundColor.ToAndroid());

            Control.SetBackground(gradientDrawable);

            int paddingLeft = (int)Context.ToPixels(entryIcon.Padding.Left);
            int paddingTop = (int)Context.ToPixels(entryIcon.Padding.Top);
            int paddingRight = (int)Context.ToPixels(entryIcon.Padding.Right);
            int paddingBottom = (int)Context.ToPixels(entryIcon.Padding.Bottom);

            Control.SetPadding(paddingLeft, paddingTop, paddingRight, paddingBottom);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            FormsEditText editText = this.Control;

            if (e.PropertyName == "Imagemage")
            {
                if (!String.IsNullOrEmpty(entryIcon.Image))
                {
                    switch (entryIcon.ImageAlignment)
                    {
                        case ImageAlignment.Left:
                            editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(entryIcon.Image), null, null, null); break;
                        case ImageAlignment.Right:
                            editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(entryIcon.Image), null); break;
                    }
                }
                OSAppTheme currentTheme = Xamarin.Forms.Application.Current.RequestedTheme;

                if (currentTheme == OSAppTheme.Light)
                {
                    editText.SetTextColor(Android.Graphics.Color.Black);
                }
                else
                {
                    editText.SetTextColor(Android.Graphics.Color.White);
                }
            }
        }

        private BitmapDrawable GetDrawable(string image)
        {
            int resourceID = Resources.GetIdentifier(image, "drawable", this.Context.PackageName);
            Drawable drawable = ContextCompat.GetDrawable(this.Context, resourceID);
            Bitmap bitmap = (drawable as BitmapDrawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, entryIcon.ImageWidth, entryIcon.ImageHeight, true));
        }
    }
}