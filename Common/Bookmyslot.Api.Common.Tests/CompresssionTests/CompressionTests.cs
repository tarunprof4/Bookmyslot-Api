using Bookmyslot.Api.Common.Compression;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

namespace Bookmyslot.Api.Common.Tests.CompresssionTests
{
    public class CompressionTests
    {
        private const string From = "from@gmail.com";
        private const string InvalidFrom = "InvalidFrom";
        private const string To = "to@gmail.com";
        private const string InvalidTo = "InvalidTo";
        private const string cc = "cc@gmail.com";
        private const string InvalidCC = "InvalidCC";
        private const string bcc = "bcc@gmail.com";
        private const string InvalidBcc = "InvalidBcc";
        private const string Subject = "subject";
        private const string Body = "body";
        private ICompression compression;
        [SetUp]
        public void Setup()
        {
            this.compression = new GZipCompression();
        }

        [Test]
        public void Compression_DataValidation()
        {
            var emailModel = GetDefaultValidEmailModel();
            var preCompressedSerialization = JsonConvert.SerializeObject(emailModel);

            var compressedEmailModel = this.compression.Compress(emailModel);
            var deCompressedEmailModel = this.compression.Decompress<EmailModel>(compressedEmailModel);
            var postCompressedSerialization = JsonConvert.SerializeObject(deCompressedEmailModel);

            Assert.AreEqual(preCompressedSerialization, postCompressedSerialization);
        }

     
        private EmailModel GetDefaultValidEmailModel()
        {
            var emailModel = new EmailModel();
            emailModel.To = new List<string>() { To };
            emailModel.From = From;
            emailModel.Cc = new List<string>() { cc };
            emailModel.Bcc = new List<string>() { bcc };
            emailModel.Subject = Subject;
            emailModel.Body = Body;

            return emailModel;
        }

       

       
    }
}