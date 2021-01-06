using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Compression.Interfaces
{
    public interface IKeyEncryptor
    {
        string Encrypt(string dataToEncript);
        string Decrypt(string dataToDecript);
    }
}
