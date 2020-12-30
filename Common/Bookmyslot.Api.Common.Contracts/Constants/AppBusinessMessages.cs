namespace Bookmyslot.Api.Common.Contracts.Constants
{
    public class AppBusinessMessages
    {
        public const string CustomerDetailsMissing = "Customer details missing";
        public const string CustomerNotFound = "Customer Doesnt Exists";
        
        public const string EmailIdNotValid = "Email id is not valid";
        public const string EmailIdExists = "Email id exists";
        public const string EmailIdDoesNotExists = "Email id does not exists";
        public const string FirstNameInValid = "Fist name is not Valid";
        public const string MiddleNameInValid = "Middle name is not Valid";
        public const string LastNameInValid = "Last name is not valid";
        public const string GenderNotValid = "Gender is not valid";
        public const string NoRecordsFound = "no records";

        public const string UserIdMissing = "user id is missing";
        public const string SlotDetailsMissing = "slot details missing";
        public const string SlotStartDateInvalid = "slot start date cannot be less than todays date";
        public const string SlotEndDateInvalid = "slot end date cannot be less than start date";

        public const string SlotIdInvalid = "slot id is not valid";
        public const string SlotIdDoesNotExists = "slot does not exists";
    }
}
