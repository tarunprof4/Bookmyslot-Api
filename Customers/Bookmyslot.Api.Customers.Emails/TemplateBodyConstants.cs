namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateBodyConstants
    {

        public const string CustomerRegistrationWelcomeEmailTemplateBody = @"<!DOCTYPE html>
<html>
<head>
    
</head>
<body style = 'font-family: Arial; font-size: 14px;' >
    <div>
        Hi @Model.FirstName! Welcome to BookMyslot.
    </div>
</body>
</html>";

        public const string SlotScheduledTemplateBody = @"<!DOCTYPE html>

<html>
<head>

</head>
<body style='font-family: Arial; font-size: 14px;'>
    <div>
        Hi @Model.CustomerModel.FirstName &nbsp; &nbsp; &nbsp; @Model.CustomerModel.LastName has booked your below mentioned slot
   </div>
    <table >
        <tr>
            <th>Slot Title</th>
            <th>Slot Date</th>
            <th>Slot Timezone</th>
            <th>Slot Start Time</th>
            <th>Slot End Time</th>
            <th>Slot Duration</th>
        </tr>
        <tr>
            <td>@Model.SlotModel.Title</td>
            <td>@Model.SlotModel.SlotDate</td>
            <td>@Model.SlotModel.TimeZone</td>
            <td>@Model.SlotModel.SlotStartTime</td>
            <td>@Model.SlotModel.SlotEndTime</td>
            <td>@Model.SlotModel.SlotDuration</td>
        </tr>
    </table>

</body>
</html>";

        public const string ResendSlotInformationTemplateBody = "Hello @Model.FirstName, welcome to RazorEngine!";

        public const string SlotCancelledTemplateBody = "";
    }
}
