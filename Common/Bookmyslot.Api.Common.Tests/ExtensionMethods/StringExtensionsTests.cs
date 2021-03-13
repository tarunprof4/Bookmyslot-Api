using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.ExtensionMethods;
using NUnit.Framework;
using System;

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
        [TestCase("13-11-2000")]
        public void IsDateValid_ValidDateToCheck_ReturnsTrueResponse(string validDate)
        {
            var isDateValid = validDate.isDateValid();
            Assert.IsTrue(isDateValid);
        }


        [TestCase("01-13-2000")]
        [TestCase("12-13-2000")]
        public void IsDateValid_InValidDateToCheck_ReturnsFalseResponse(string inValidDate)
        {
            var isDateValid = inValidDate.isDateValid();
            Assert.IsFalse(isDateValid);
        }
        
    }
}
