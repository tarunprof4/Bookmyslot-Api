namespace Bookmyslot.Api.Customers.ViewModels
{
    public class AdditionalProfileSettingsViewModel
    {
        public string BioHeadLine { get; set; }

        public AdditionalProfileSettingsViewModel(string bioHeadLine)
        {
            this.BioHeadLine = bioHeadLine;
        }
    }
}
