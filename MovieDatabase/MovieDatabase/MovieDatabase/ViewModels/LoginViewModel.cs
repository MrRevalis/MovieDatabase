using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MovieDatabase.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand Login { get; }
        public ICommand Register { get; }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel()
        {
            Login = new Command(async () => await LoginMethod());
            Register = new Command(async () => await Shell.Current.GoToAsync("//login/registration"));
        }

        private async Task LoginMethod()
        {
            if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password) && !IsNotConnected)
            {
                IsBusy = true;
                var token = await FirebaseAuth.Login(Email, Password);
                if (token != String.Empty)
                {
                    await Shell.Current.GoToAsync("//main");
                    Email = "";
                    Password = "";
                }
                IsBusy = false;
            }
        }
    }
}
