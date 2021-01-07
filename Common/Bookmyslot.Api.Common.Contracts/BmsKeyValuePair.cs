﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Contracts
{
    public class BmsKeyValuePair<TKey, TValue>
    {
        public BmsKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public TKey Key { get; set; }

        public TValue Value { get; set; }

    }

}
