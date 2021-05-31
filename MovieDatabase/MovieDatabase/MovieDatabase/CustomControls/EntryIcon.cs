using Xamarin.Forms;

namespace MovieDatabase.CustomControls
{
    public class EntryIcon : Entry
    {
        public EntryIcon()
        {
            this.HeightRequest = 50;
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create("Imagemage", typeof(string), typeof(EntryIcon), string.Empty);

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly BindableProperty ImageHeightProperty =
            BindableProperty.Create("ImageHeight", typeof(int), typeof(EntryIcon), 40);

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly BindableProperty ImageWidthProperty =
            BindableProperty.Create("ImageWidth", typeof(int), typeof(EntryIcon), 40);

        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }

        public static readonly BindableProperty ImageAlignmentProperty =
            BindableProperty.Create("ImageAlignment", typeof(ImageAlignment), typeof(EntryIcon), ImageAlignment.Left);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor", typeof(Color), typeof(EntryIcon), Color.White);

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create("BorderThickness", typeof(int), typeof(EntryIcon), 0);

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(int), typeof(EntryIcon), 0);

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create("Padding", typeof(Thickness), typeof(EntryIcon), new Thickness(5));

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create("BackgroundColor", typeof(Color), typeof(EntryIcon), Color.White);
    }
    public enum ImageAlignment
    {
        Left,
        Right
    }
}