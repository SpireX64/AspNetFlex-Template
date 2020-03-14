using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AspNetFlex.Domain.Interactions.Users.Utils;
using NUnit.Framework;

namespace AspNetFlex.Domain.UnitTests.Interactions.Users
{
    [TestFixture]
    public class AuthUtilsTest
    {
        [Test]
        public void GetKeyMethod_ProvidePassword_ReturnSymmetricKey()
        {
            const string password = "ImOb0Iz74krSM";

            var key = AuthUtils.GetSymmetricKey(password);

            Assert.NotNull(key);
            Assert.NotZero(key.KeySize);
            Assert.AreEqual(key.KeySize, password.Length * 8);
        }

        [Test]
        public void GetKeyMethod_ProvideEmptyPassword_ThrowException()
        {
            Assert.Throws<ArgumentException>(() => AuthUtils.GetSymmetricKey(string.Empty));
        }
    
        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void ValidateEmailMethod_PresentValidEmail_ReturnsTrue()
        {
            // ---- Arrange ----
            
            var validEmails = new List<string>()
            {
                "email@example.com",
                "firstname.lastname@example.com",
                "email@subdomain.example.com",
                "firstname+lastname@example.com",
                "email@123.123.123.123",
                "1234567890@example.com",
                "email@example-one.com",
                "_______@example.com",
                "email@example.name",
                "email@example.museum",
                "email@example.co.jp",
                "firstname-lastname@example.com"
            };

            // ---- Act & Assert ----
            
            foreach (var email in validEmails)
            {
                Assert.IsTrue(AuthUtils.ValidateEmail(email), $"Email:  {email}");
            }
        }

        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void ValidateEmailMethod_PresentWrongFormattedEmail_ReturnFalse()
        {
            // ---- Arrange ----

            var invalidEmails = new List<string>()
            {
                "plainaddress",
                "#@%^%#$@#$@#.com",
                "@example.com",
                "Artem Spirex <email@example.com>",
                "email.example.com",
                "email@example@example.com",
                "A@b@c@example.com",
                ".email@example.com",
                "email.@example.com",
                "email..email@example.com",
                "あいうえお@example.com",
                "email@example.com (Artem SpireX)",
                "email@example",
                "email@-example.com",
                "email@example..com",
                "Abc..123@example.com",
                "(),\":;<>[\\]@example.com",
                "just\"not\"right@example.com",
                "this\\ is\"really\"not\\allowed@example.com"
            };
                
            foreach (var email in invalidEmails)
            {
                Assert.IsFalse(AuthUtils.ValidateEmail(email), $"Email:  {email}");
            }
        }

        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void GetMd5HashMethod__CheckGeneration_ValidMd5Hash()
        {
            const string examplePassword = @"EuYw7?{UsBVwQuCz";
            const string generatedMd5 = @"3977efafcd79547c092d22bc3b0bf70f";

            var md5 = AuthUtils.GetMd5Hash(examplePassword);

            StringAssert.AreEqualIgnoringCase(generatedMd5, md5);
            StringAssert.AreEqualIgnoringCase(md5, AuthUtils.GetMd5Hash(examplePassword));
        }

        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void CheckPasswordComplexityMethod_StrongPasswords_ReturnsTrue()
        {
            var strongPasswords = new List<string>()
            {
                "7hGob5~y2l@Q",
                "?b%WVEvh6c8*sr",
                "JR2hkaVQMVmLFB2m",
                "ImOb0Iz74krSM",
                "0MwV9RBZLH#|A}%iqyTacvYkKFrXQYqdV8kx1ufa",
                "PwVivpX8DLUmC5e7agqbIA1lHOxQrMuhenLsJetF"
            };

            foreach (var password in strongPasswords)
            {
                Assert.IsTrue(AuthUtils.CheckPasswordComplexity(password));
            }
        }

        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void CheckPasswordComplexityMethod_WeakPasswords_ReturnsFalse()
        {
            var weakPasswords = new List<string>()
            {
                "",
                "820646",
                "837949205512",
                "#15$@2",
                "}0231@2*8|93",
                "mmxhfu",
                "TFEDEG",
                "taraomqcnppa",
                "TXBURONYUTOW",
                "m@@cyqk?zw~w",
                "RZ*I}#T{XZHQ"
            };
            
            foreach (var password in weakPasswords)
            {
                Assert.IsFalse(AuthUtils.CheckPasswordComplexity(password));
            }
        }

        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void ValidateNameMethod_PresentCorrectNames_ReturnsTrue()
        {
            var correctNames = new List<string>()
            {

            };

            foreach (var name in correctNames)
            {
                Assert.IsTrue(AuthUtils.ValidateName(name));
            }
        }
        
        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void ValidateNameMethod_PresentIncorrectNames_ReturnsTrue()
        {
            var incorrectNames = new List<string>()
            {

            };

            foreach (var name in incorrectNames)
            {
                Assert.IsFalse(AuthUtils.ValidateName(name));
            }
        }
    }
}