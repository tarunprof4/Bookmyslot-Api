namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CustomerViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public CustomerViewModel(string firstName, string lastName, string bioHeadLine)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BioHeadLine = bioHeadLine;
        }

    }
}
