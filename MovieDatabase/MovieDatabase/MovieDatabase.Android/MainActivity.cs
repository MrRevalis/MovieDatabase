using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using Firebase;
using Xamarin.Forms.Platform.Android;

namespace MovieDatabase.Droid
{
    [Activity(Label = "MovieDatabase", Icon = "@mipmap/launcher_icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetStatusBarColor(Xamarin.Forms.Color.FromHex("1b242c").ToAndroid());

            var options = new FirebaseOptions.Builder()
                .SetApplicationId("moviedatabase-5e229")
                .SetApiKey("AIzaSyAPTgYJUDh13vmKNwrAhW73-tF9OGPtMhk")
                .SetStorageBucket("moviedatabase-5e229.appspot.com")
                .Build();

            if (FirebaseApp.GetApps(Application.Context).Count == 0)
            {
                FirebaseApp.InitializeApp(Application.Context, options);
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}