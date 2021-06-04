using Bookmyslot.Api.Common.Encryption.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Encryption.Helpers
{
    public class EncryptionHelper
    {
		public static string UrlEncode(string encryptedMessage)
		{
			return encryptedMessage.Replace(EncryptionConstants.SlashDelimiter, EncryptionConstants.UnderScoreDelimiter).
				Replace(EncryptionConstants.PlusDelimiter, EncryptionConstants.HypenDelimiter);
		}

		public static string UrlDecode(string encodedEcryptedMesage)
		{
			return encodedEcryptedMesage.Replace(EncryptionConstants.UnderScoreDelimiter, EncryptionConstants.SlashDelimiter)
				.Replace(EncryptionConstants.HypenDelimiter, EncryptionConstants.PlusDelimiter);
		}
	}
}
