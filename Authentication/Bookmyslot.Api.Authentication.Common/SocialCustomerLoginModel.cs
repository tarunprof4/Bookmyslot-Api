using System.ComponentModel;

namespace Bookmyslot.Api.Authentication.Common
{
    public class SocialCustomerLoginModel
    {
        [DefaultValue("facebok")]
        public string Provider { get; set; }
        public string IdToken { get; set; }

        [DefaultValue("EAACkRCl4ZACYBAOtP5eOfbikJgrZCTlUAklZAFs6HZBNvy2BnurEktYlz5oG5RJSBEXxvf8Ej5SG6NjUkXc7oPswFljeIYOE0cIbgSZBWFFbaakTZA4CUgLKWAVhSWRTqlucZCccZCiMKNKytAz8NH0pkCavaTp0MaZAKOcdzCXvZAOH1iuDfHZCdI3njBIe1Hcv4WZAAQCPy5IkCuSwLaJCF5cQAyaq2ZA2McYkZD")]
        public string AuthToken { get; set; }
    }
}
