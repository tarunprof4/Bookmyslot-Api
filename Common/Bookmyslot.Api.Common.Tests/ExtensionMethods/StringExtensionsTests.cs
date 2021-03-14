using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.ExtensionMethods;
using NUnit.Framework;

namespace Bookmyslot.Api.Common.Tests.ExtensionMethods
{


    public class StringExtensionsTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("01-01-2000")]
        [TestCase("12-12-2000")]
        [TestCase("11-13-2000")]
        public void IsDateValid_ValidDateToCheck_ReturnsTrueResponse(string dateString)
        {
            var isDateValid = dateString.isDateValid(DateTimeConstants.ApplicationInputDatePattern);
            Assert.IsTrue(isDateValid);
        }

        [TestCase("1-13-2000")]
        [TestCase("13-1-2000")]
        [TestCase("13-12-2000")]
        public void IsDateValid_InValidDateToCheck_ReturnsFalseResponse(string dateString)
        {
            var isDateValid = dateString.isDateValid(DateTimeConstants.ApplicationInputDatePattern);
            Assert.IsFalse(isDateValid);
        }

     
    }
}
