﻿namespace Bookmyslot.Api.Common.Contracts.Constants
{
    public class AppBusinessMessages
    {
        public const string ApplicationName = "BookMySlot";
        public const string CustomerDetailsMissing = "Customer details missing";
        public const string CustomerNotFound = "Customer Doesnt Exists";



        public const string EmailIdMissing = "Email id is missing";
        public const string EmailIdNotValid = "Email id is not valid";
        public const string CustomerIdNotValid = "Customer id is not valid";
        public const string EmailIdExists = "Email id exists";
        public const string EmailIdDoesNotExists = "Email id does not exists";
        public const string FirstNameRequired = "First name is required";
        public const string FirstNameInValid = "First name is not Valid";
        public const string LastNameRequired = "Last name is required";
        public const string LastNameInValid = "Last name is not valid";
        public const string GenderNotValid = "Gender is not valid";
        public const string NoRecordsFound = "no records found";

        public const string SlotTitleMissing = "slot title is missing";
        public const string UserIdMissing = "user id is missing";
        public const string SlotDetailsMissing = "slot details missing";
        public const string TimeZoneMissing = "time zone missing";
        public const string SlotStartDateInvalid = "slot start date cannot be less than todays date";
        public const string SlotEndTimeInvalid = "slot end time  cannot be less than slot start time";
        public const string SlotScheduleDateInvalid = "slot cannot be booked for past date";

        public const string SlotIdInvalid = "slot id is not valid";
        public const string SlotIdDoesNotExists = "slot does not exists";

        public const string CorruptData = "Data is corrupt";

        public const string NoSlotsFound = "no slots found";

        public const string MinimumDaysForSlotMeetingLink = "slot meeting link will be sent 1 day before the meeting";

        public const int NameMaxLength = 50;
        public const string FirstNameMaxLength = "Please keep first name less than 50 characters";
        public const string LastNameMaxLength = "Please keep last name less than 50 characters";
    }
}
