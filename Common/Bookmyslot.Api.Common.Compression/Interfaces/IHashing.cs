using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Compression.Interfaces
{
    public interface IHashing
    {
        public string Create(object value);
    }
}
