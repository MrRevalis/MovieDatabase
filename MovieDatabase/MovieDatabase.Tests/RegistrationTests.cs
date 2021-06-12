using NUnit.Framework;
using MovieDatabase.ViewModels;

namespace MovieDatabase.Tests
{
    [TestFixture]
    public class RegistrationTests
    {
        RegistrationViewModel vm;

        [SetUp]
        public void Setup()
        {
            vm = new RegistrationViewModel();
        }

        [Test]
        public void EmptyCredentials()
        {
            vm.Email.Value = "";
            vm.Password.Item1.Value = "";
            vm.Password.Item2.Value = "";

            bool results = vm.ValidateFields();

            Assert.AreEqual(false, results);
        }

        [Test]
        public void DifferentPasswords()
        {
            vm.Email.Value = "dominikr26@interia.pl";
            vm.Password.Item1.Value = "123456";
            vm.Password.Item2.Value = "123578";

            bool results = vm.ValidateFields();


            Assert.AreEqual(false, results);
            Assert.AreEqual("Password and confirm password don't match", vm.Password.Validations[0].ValidationMessage);
        }

        [Test]
        public void CorrectEmail()
        {
            vm.Email.Value = "dominikr26@";

            vm.Email.Validate();

            Assert.AreEqual("Email Required", vm.Email.Validations[0].ValidationMessage);
            Assert.AreEqual("Invalid Email", vm.Email.Validations[1].ValidationMessage);
        }
    }
}