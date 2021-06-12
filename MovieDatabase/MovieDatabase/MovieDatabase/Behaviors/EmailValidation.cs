using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MovieDatabase.Behaviors
{
    public class EmailValidation : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.TextChanged -= OnTextChanged;
        }

        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            string email = e.NewTextValue;
            Regex emailPattern = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (emailPattern.IsMatch(email))
            {
                //AppTheme appTheme = AppInfo.RequestedTheme;
                AppTheme appTheme = AppTheme.Light;
                switch (appTheme)
                {
                    case AppTheme.Light:
                        entry.TextColor = Color.Black; break;
                    case AppTheme.Dark:
                        entry.TextColor = Color.White; break;
                    default:
                        entry.TextColor = Color.Black; break;
                }
            }
            else
            {
                entry.TextColor = Color.FromHex("cc0000");
            }
        }
    }
}
