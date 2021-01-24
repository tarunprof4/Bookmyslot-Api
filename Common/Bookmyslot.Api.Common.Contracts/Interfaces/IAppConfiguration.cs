namespace Bookmyslot.Api.Common.Contracts.Interfaces
{
    public interface IAppConfiguration
    {
        string EmailStmpHost { get; }

        string EmailPort { get; }

        string EmailUserName { get;  }

        string EmailPassword { get;  }

        string AppVersion { get; }

        string LogOutputTemplate { get; }
    }
}
