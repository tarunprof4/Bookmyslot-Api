using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Azure.Contracts.Configuration
{
    public class AzureConfiguration
    {
        private readonly string bmsTopic;
        private readonly string bmsTopicKey;

        public AzureConfiguration(IConfiguration configuration)
        {
            var eventGridSettings = configuration.GetSection(AppSettingKeysConstants.AzureSettings).GetSection(AppSettingKeysConstants.EventGridSettings);

            this.bmsTopic = eventGridSettings.GetSection(AppSettingKeysConstants.BmsTopic).Value;
            this.bmsTopicKey = eventGridSettings.GetSection(AppSettingKeysConstants.BmsTopicKey).Value;
        }

        public string BmsTopic => this.bmsTopic;
        public string BmsTopicKey => this.bmsTopicKey;
    }
}
