﻿namespace Bookmyslot.Api.Common.Contracts.Constants
{
    public class AppBusinessMessagesConstants
    {
        public const string ApplicationName = "BookMySlot";
        public const string RegisterCustomerDetailsMissing = "Customer Registration details missing";
        public const string ProfileSettingDetailsMissing = "Profile Settings details missing";
        public const string AdditionalProfileSettingDetailsMissing = "additional profile Settings details missing";
        public const string CustomerNotFound = "Customer Doesnt Exists";

        public const string EmptyCache = "Empty Cache";


        public const string FirstNameRequired = "First name is required";
        public const string LastNameRequired = "Last name is required";
        public const string EmailRequired = "Email is required";
        public const string GenderRequired = "Gender is required";
        public const string BioHeadLineRequired = "Bio headline is required";

        public const string FirstNameInValid = "First name is not Valid";
        public const string LastNameInValid = "Last name is not valid";
        public const string GenderNotValid = "Gender is not valid";
        public const string BioHeadLineNotValid = "Bio headline is not valid";


        public const string EmailIdNotValid = "Email id is not valid";
        public const string CustomerIdNotValid = "Customer id is not valid";
        public const string EmailIdExists = "Email id exists";
        public const string EmailIdDoesNotExists = "Email id does not exists";
        
      
        public const string NoRecordsFound = "no records found";

        public const string SlotTitleRequired = "slot title is required";
        public const string SlotDetailsMissing = "slot details missing";
        public const string CountryRequired = "country is required";
        public const string TimeZoneRequired = "time zone is required";
        public const string SlotDateRequired = "slot date is required";
        public const string InValidSlotDate = "invaild slot date";
        public const string DayLightSavinngDateNotAllowed = "slots on day light saving day are not allowed to avoid issues";
        public const string SlotStartDateInvalid = "slot start date cannot be less than todays date";
        public const string SlotEndTimeInvalid = "slot end time  cannot be less than slot start time";
        public const string SlotDurationInvalid = "slot duration  cannot be less than 20 mins";
        public const string SlotScheduleDateInvalid = "slot cannot be booked for past date";
        public const string SlotScheduleCannotBookOwnSlot = "You cannot book your own slot please";
        

        public const string SlotIdInvalid = "slot id is not valid";
        
        public const string SlotIdDoesNotExists = "slot does not exists";

        public const string NoLastSlotShared = "customer hasnt shared any slot yet";

        public const string CorruptData = "Data is corrupt";

        public const string NoSlotsFound = "no slots found";

        public const string MinimumDaysForSlotMeetingLink = "slot meeting link will be sent 1 day before the meeting";

        public const string InValidSearchKey = "invalid search key";

        
        public const string FirstNameMaxLength = "Please keep first name less than 100 characters";
        public const string LastNameMaxLength = "Please keep last name less than 100 characters";
        public const string EmailMaxLength = "Please keep email less than 150 characters";
        public const string UserNameMaxLength = "Please keep user name less than 150 characters";
        public const string PhoneMaxLength = "Please keep phone number less than 15 characters";
        public const string BioHeadLineMaxLength = "Please keep bio headline less than 1024 characters";
        public const string ProviderMaxLength = "Please keep socail provider less than 20 characters";

        public const string GenderMaxLength = "Please keep gender less than 20 characters";

        public const string LoginFailed = "Login Failed";

        public const string SocialLoginTokenDetailsMissing = "Token details Missing";

        public const string AuthTokenRequired = "auth token is required";
        public const string IdTokenRequired = "id token is required";
        public const string TokenProviderRequired = "token provider is required";
        public const string InValidTokenProvider = "invalid token provider";

        public const string InValidCountry = "invaild country";
        public const string InValidTimeZone = "invaild timeZone";


        public const string CustomerSettingsMissing = "customer settings missing";
        public const string CustomerSettingsNotFound = "Customer settings information Doesnt Exists";

        public const string PaginationSettingsMissing = "pagination settings missing";
        public const string InValidPageSize = "page size has to be greater than 0";
        public const string InValidPageNumber = "page number has to be greater than 0";

        public const string ResendSlotInfoMissing = "resend slot Info missing";
        public const string ResendSlotInfoRequired = "resend slot Info required";

        public const string CancelSlotInfoMissing = "cancel slot info missing";
        public const string CancelSlotRequired = "cancel slot info required";


        public const string SlotScheduleInfoMissing = "slot scheduler Info missing";
        public const string SlotScheduleInfoRequired = "slot scheduler Info required";

        public const string NoCustomerSearchResults = "no customer search results found";

        public const string FileMissing = "File is missing";
        public const string ImageSizeTooLong = "Image size is too long";
        public const string InvalidImageExtension = "Image image extension";
        public const string InvalidImageExtensionSignature = "Image image extension signature";
    }
}
