using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Contracts.Money
{
    public struct Money
    {
        public Money(decimal amount, string currencyCode)
            : this()
        {
            this.Amount = amount;
            this.CurrencyCode = currencyCode;
        }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public static Money Empty
        {
            get
            {
                return new Money(0, string.Empty);
            }
        }

        public static Money operator +(Money x, Money y)
        {
            if (y.IsEmpty())
            {
                return x;
            }

            if (x.IsEmpty())
            {
                return y;
            }

            if (string.Equals(x.CurrencyCode, y.CurrencyCode) == false)
            {
                throw new ArgumentException("Argument must be equal", nameof(CurrencyCode));
            }

            return new Money(x.Amount + y.Amount, x.CurrencyCode);
        }

        public static Money operator *(Money x, decimal y)
        {
            return new Money(x.Amount * y, x.CurrencyCode);
        }

        public static Money operator *(decimal x, Money y)
        {
            return new Money(y.Amount * x, y.CurrencyCode);
        }

        public static Money operator -(Money x, Money y)
        {
            if (y.IsEmpty())
            {
                return x;
            }

            if (x.IsEmpty())
            {
                return y * -1;
            }

            if (string.Equals(x.CurrencyCode, y.CurrencyCode) == false)
            {
                throw new ArgumentException("Argument must be equal", nameof(CurrencyCode));
            }

            return new Money(x.Amount - y.Amount, x.CurrencyCode);
        }

        public static bool operator ==(Money x, Money y)
        {
            return x.CurrencyCode == y.CurrencyCode && x.Amount == y.Amount;
        }

        public static bool operator <(Money x, Money y)
        {
            if (string.Equals(x.CurrencyCode, y.CurrencyCode) == false)
            {
                throw new ArgumentException("Argument must be equal", nameof(CurrencyCode));
            }

            return x.Amount < y.Amount;
        }

        public static bool operator >(Money x, Money y)
        {
            if (string.Equals(x.CurrencyCode, y.CurrencyCode) == false)
            {
                throw new ArgumentException("Argument must be equal", nameof(CurrencyCode));
            }

            return x.Amount > y.Amount;
        }

        public static bool operator <=(Money x, Money y)
        {
            if (string.Equals(x.CurrencyCode, y.CurrencyCode) == false)
            {
                throw new ArgumentException("Argument must be equal", nameof(CurrencyCode));
            }

            return x.Amount <= y.Amount;
        }

        public static bool operator >=(Money x, Money y)
        {
            if (string.Equals(x.CurrencyCode, y.CurrencyCode) == false)
            {
                throw new ArgumentException("Argument must be equal", nameof(CurrencyCode));
            }

            return x.Amount >= y.Amount;
        }

        public static bool operator !=(Money x, Money y)
        {
            return (x == y) == false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Money == false)
            {
                return false;
            }

            return this.Equals((Money)obj);
        }

        public bool Equals(Money other)
        {
            return this.Amount == other.Amount && string.Equals(this.CurrencyCode, other.CurrencyCode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Amount.GetHashCode() * 397) ^ (this.CurrencyCode?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return this.CurrencyCode + " " + this.Amount;
        }

        private bool IsEmpty()
        {
            return this.Equals(Empty);
        }
    }
}
