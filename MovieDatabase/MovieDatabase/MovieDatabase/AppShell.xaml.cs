using MovieDatabase.Views;
using Xamarin.Forms;

namespace MovieDatabase
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("detail", typeof(DetailPage));
            Routing.RegisterRoute("search", typeof(SearchPage));
            Routing.RegisterRoute("video", typeof(PlayerPage));
            Routing.RegisterRoute("registration", typeof(RegistrationPage));
            Routing.RegisterRoute("internet", typeof(NoInternetPage));
        }
    }
}