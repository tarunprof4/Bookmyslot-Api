﻿using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bookmyslot.Api.NodaTime.Contracts.Configuration
{
    public class NodaTimeZoneLocationConfigurationSingleton
    {
        private static NodaTimeZoneLocationConfigurationSingleton instance;
        private static readonly object padlock = new object();

        public ReadOnlyDictionary<string, string> ZoneWithCountryId { get; }
        public ReadOnlyCollection<string> Countries { get; }

        private NodaTimeZoneLocationConfigurationSingleton(Dictionary<string, string> zoneWithCountryId, List<string> countries)
        {
            this.ZoneWithCountryId = new ReadOnlyDictionary<string, string>(zoneWithCountryId);
            this.Countries = new ReadOnlyCollection<string>(countries);

        }

        public static NodaTimeZoneLocationConfigurationSingleton CreateInstance(Dictionary<string, string> zoneWithCountryId, List<string> countries)
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NodaTimeZoneLocationConfigurationSingleton(zoneWithCountryId, countries);
                    }
                }
            }
            return instance;
        }

        public static NodaTimeZoneLocationConfigurationSingleton GetInstance()
        {
            if (instance == null)
            {
                throw new InvalidOperationException(ExceptionMessagesConstants.NodaTimeZoneLocationConfigurationSingletonNotInitialized);
            }
            return instance;
        }


    }

}
