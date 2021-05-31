using MovieDatabase.Views;
using Xamarin.Forms;

namespace MovieDatabase
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("internet", typeof(NoInternetPage));
        }

    }
}
