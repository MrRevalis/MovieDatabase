using MovieDatabase.Services;
using Xamarin.Forms;

namespace MovieDatabase
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IMovieDB, MovieDB>();
            DependencyService.Register<IFirebaseDatabase, FirebaseDatabase>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
