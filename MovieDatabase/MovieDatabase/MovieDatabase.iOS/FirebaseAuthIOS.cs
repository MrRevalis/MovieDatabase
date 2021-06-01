using Foundation;
using MovieDatabase.iOS;
using MovieDatabase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Firebase.Auth;

[assembly: Dependency(typeof(FirebaseAuthIOS))]
namespace MovieDatabase.iOS
{
    public class FirebaseAuthIOS : IFirebase
    {
        public async Task<string> CreateAccount(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.CreateUserAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        public string GetUserName()
        {
            if (IsSignIn())
            {
                return Auth.DefaultInstance.CurrentUser.Email;
            }
            return string.Empty;
        }

        public bool IsSignIn()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }

        public async Task<string> Login(string email, string password)
        {
            //APPLE DEVELOPER ACCOUNT
            /*try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch(Exception e)
            {
                return String.Empty;
            }*/
            return "working";
        }

        public bool SignOut()
        {
            try
            {
                _ = Auth.DefaultInstance.SignOut(out NSError error);
                return error == null;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}