using System.Collections.Generic;

namespace Bookmyslot.SharedKernel
{
    public class EmailModel
    {

        public string From { get; set; }

        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public List<string> Bcc { get; set; }

        public List<object> Attachments { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public bool IsBodyHtml { get; set; }

        public string HtmlAttachment { get; set; }

        public string FileName { get; set; }
    }

}
