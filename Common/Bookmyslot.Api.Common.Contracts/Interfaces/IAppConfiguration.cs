namespace Bookmyslot.Api.Common.Contracts.Interfaces
{
    public interface IAppConfiguration
    {
        string AppVersion { get; }

        string LogOutputTemplate { get; }

        string ElasticSearchUrl { get; }

        string ReadDatabaseConnectionString { get; }
    }
}
