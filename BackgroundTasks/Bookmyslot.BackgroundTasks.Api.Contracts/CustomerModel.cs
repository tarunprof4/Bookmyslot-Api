using Nest;

namespace Bookmyslot.BackgroundTasks.Api.Contracts
{
    public class CustomerModel
    {
        [Keyword]
        public string Id { get; set; }


        [Keyword]
        public string Email { get; set; }

        [Keyword]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        [Text]
        public string BioHeadLine { get; set; }

        [Keyword]
        public string PhotoUrl { get; set; }
    }
}
