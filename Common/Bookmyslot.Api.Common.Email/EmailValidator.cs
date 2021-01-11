using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Email
{
    public class EmailValidator 
    {
       
        public static Response<bool> Validate(EmailModel emailModel)
        {
            List<string> validationErrors = new List<string>();
            RequiredAttribute requiredAttribute = new RequiredAttribute();
            EmailAddressAttribute emailAddressAttribute = new EmailAddressAttribute();
            if (!requiredAttribute.IsValid(emailModel.From))
            {
                 validationErrors.Add(EmailConstants.EmailFromAddressIsRequired);
            }

            if (!emailAddressAttribute.IsValid(emailModel.From))
            {
                validationErrors.Add(string.Format(EmailConstants.EmailFromAddressIsInvalid, emailModel.From));
            }

            if (emailModel.To == null || emailModel.To.Count == 0)
            {
                validationErrors.Add(EmailConstants.EmailToAddressIsRequired);
            }

            foreach (var to in emailModel.To)
            {
                if (!requiredAttribute.IsValid(to))
                {
                    validationErrors.Add(EmailConstants.EmailToAddressIsRequired);
                }

                if (!emailAddressAttribute.IsValid(to))
                {
                    validationErrors.Add(string.Format(EmailConstants.EmailToAddressIsInvalid, to));
                }
            }

            if (emailModel.Cc != null && emailModel.Cc.Count > 0)
            {
                foreach (var cc in emailModel.Cc)
                {
                    if (cc == null || !emailAddressAttribute.IsValid(cc))
                    {
                        validationErrors.Add(string.Format(EmailConstants.EmailccAddressIsInvalid, cc));
                    }
                }
            }

            if (emailModel.Bcc != null && emailModel.Bcc.Count > 0)
            {
                foreach (var bcc in emailModel.Bcc)
                {
                    if (bcc == null || !emailAddressAttribute.IsValid(bcc))
                    {
                        validationErrors.Add(string.Format(EmailConstants.EmailBccAddressIsInvalid, bcc));
                    }
                }
            }

            if (!requiredAttribute.IsValid(emailModel.Subject))
            {
                validationErrors.Add(EmailConstants.EmailSubjectIsRequired);
            }

            if (!requiredAttribute.IsValid(emailModel.Body))
            {
                validationErrors.Add(EmailConstants.EmailBodyIsRequired);
            }

            if (validationErrors.Count > 0)
            {
                return Response<bool>.ValidationError(validationErrors);
            }

            return  Response<bool>.Success(true);
        }
    }
}
