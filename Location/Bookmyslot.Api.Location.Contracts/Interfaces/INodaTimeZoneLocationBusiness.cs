using Bookmyslot.Api.Location.Contracts;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.Location.Interfaces
{
    public interface INodaTimeZoneLocationBusiness
    {
        NodaTimeZoneLocation GetNodaTimeZoneLocationInformation();
    }
}
