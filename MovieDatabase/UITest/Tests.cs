using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        AndroidApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        /*[SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }*/

        [SetUp]
        public void BeforeEachTest()
        {
            //app = ConfigureApp.Android.StartApp();

            app = ConfigureApp.Android
                 .ApkFile(@"C:\Users\Dominik\Desktop\MovieDatabase\MovieDatabase\MovieDatabase\MovieDatabase.Android\bin\Release\com.companyname.moviedatabase.apk")
                 .DeviceSerial("ce0517154af094a20d")
                 .PreferIdeSettings()
                 .EnableLocalScreenshots()
                 .StartApp();
        }


        /*[Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }*/

        [Test]
        public void OpenRepl()
        {
            app.Repl();
        }

        [Test]
        public void TryingToLogin()
        {
            app.Tap("loginEntry");
            app.ClearText(x => x.Marked("loginEntry"));
            app.EnterText(x => x.Marked("loginEntry"), "dominikr26@interia.pl");
            app.DismissKeyboard();

            app.Tap("passwordEntry");
            app.ClearText(x => x.Marked("passwordEntry"));
            app.EnterText(x => x.Marked("passwordEntry"), "123456");
            app.DismissKeyboard();

            app.Tap("loginButton");
            app.WaitForElement("welcomeBox");

            bool result = app.Query(x => x.Marked("welcomeBox")).Any();

            Assert.IsTrue(result);

        }
    }
}
