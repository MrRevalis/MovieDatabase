using NUnit.Framework;
using MovieDatabase.ViewModels;
using MovieDatabase.Behaviors;
using Xamarin.Forms;

namespace MovieDatabase.Tests
{
    [TestFixture]
    public class EmailValidateTest
    {
        EmailValidation email;

        [SetUp]
        public void Setup()
        {
            email = new EmailValidation();
        }

        [Test]
        public void EmptyCredentials()
        {
            Entry entry = new Entry();
            TextChangedEventArgs emailCorrect = new TextChangedEventArgs("dominikr26@interia.pl", "dominikr26@interia.pl");
            TextChangedEventArgs emailIncorrect = new TextChangedEventArgs("qwer", "qwer");

            email.OnTextChanged(entry, emailCorrect);
            Assert.AreEqual(Color.Black, entry.TextColor);

            email.OnTextChanged(entry, emailIncorrect);
            Assert.AreEqual(Color.FromHex("cc0000"), entry.TextColor);
        }

    }
}