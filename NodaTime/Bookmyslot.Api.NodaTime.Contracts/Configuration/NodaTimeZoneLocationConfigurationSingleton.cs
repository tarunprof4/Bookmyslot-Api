﻿namespace Bookmyslot.Api.NodaTime.Contracts.Configuration
{
    public class NodaTimeZoneLocationConfigurationSingleton
    {
        private static NodaTimeZoneLocationConfigurationSingleton instance;
        private static readonly object padlock = new object();

        public ReadOnlyDictionary<string, string> ZoneWithCountryId { get; }
        public ReadOnlyDictionary<string, string> Countries { get; }

        private NodaTimeZoneLocationConfigurationSingleton(Dictionary<string, string> zoneWithCountryId, Dictionary<string, string> countries)
        {
            this.ZoneWithCountryId = new ReadOnlyDictionary<string, string>(zoneWithCountryId);
            this.Countries = new ReadOnlyDictionary<string, string>(countries);

        }

        public static NodaTimeZoneLocationConfigurationSingleton CreateInstance(Dictionary<string, string> zoneWithCountryId, Dictionary<string, string> countries)
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


        public bool IsCountryValid(string country)
        {
            return this.Countries.ContainsKey(country);
        }

        public bool IsTimeZoneValid(string timeZone)
        {
            return this.ZoneWithCountryId.ContainsKey(timeZone);
        }
    }
}
