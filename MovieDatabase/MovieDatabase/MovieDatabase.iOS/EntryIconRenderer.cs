using CoreAnimation;
using CoreGraphics;
using Foundation;
using MovieDatabase.CustomControls;
using MovieDatabase.iOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryIcon), typeof(EntryIconRenderer))]
namespace MovieDatabase.iOS
{
    public class EntryIconRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var element = this.Element as EntryIcon;
            var textField = this.Control;

            if (!String.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        textField.LeftViewMode = UITextFieldViewMode.Always;
                        textField.LeftView = GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                        break;
                    case ImageAlignment.Right:
                        textField.RightViewMode = UITextFieldViewMode.Always;
                        textField.RightView = GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                        break;
                }
            }

            textField.BorderStyle = UITextBorderStyle.None;
            CALayer bottomBorder = new CALayer
            {
                Frame = new CGRect(0.0f, element.HeightRequest - 1, this.Frame.Width, 1.0f),
                BorderWidth = 2.0f,
                BorderColor = element.BorderColor.ToCGColor()
            };

            textField.Layer.AddSublayer(bottomBorder);
            textField.Layer.MasksToBounds = true;

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var element = this.Element as EntryIcon;
            var textField = this.Control;

            if (e.PropertyName == "Imagemage")
            {
                if (!String.IsNullOrEmpty(element.Image))
                {
                    switch (element.ImageAlignment)
                    {
                        case ImageAlignment.Left:
                            textField.LeftViewMode = UITextFieldViewMode.Always;
                            textField.LeftView = GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                            break;
                        case ImageAlignment.Right:
                            textField.RightViewMode = UITextFieldViewMode.Always;
                            textField.RightView = GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                            break;
                    }
                }
            }


            if (e.PropertyName == "TextColor")
            {
                OSAppTheme currentTheme = Xamarin.Forms.Application.Current.RequestedTheme;

                if (currentTheme == OSAppTheme.Light)
                {
                    textField.TextColor = UIColor.Black;

                }
                else
                {
                    textField.TextColor = UIColor.White;
                }
            }
        }

        private UIView GetImageView(string imagePath, int height, int width)
        {
            var uiImageView = new UIImageView(UIImage.FromBundle(imagePath))
            {
                Frame = new RectangleF(0, 0, width, height)
            };
            UIView objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, width + 10, height));
            objLeftView.AddSubview(uiImageView);

            return objLeftView;
        }
    }
}