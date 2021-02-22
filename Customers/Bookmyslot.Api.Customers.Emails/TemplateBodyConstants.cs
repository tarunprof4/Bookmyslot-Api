//namespace Bookmyslot.Api.Customers.Emails
//{
//    public class TemplateBodyConstants
//    {

//        public const string CustomerRegistrationWelcomeEmailTemplateBody = @"<!DOCTYPE html>

//<html>
//<head>
//    <style>

//        body {
//            font-family: Arial;
//            font-size: 14px;
//        }
//    </style>
//</head>
//<body>
//    <div>
//        Hi @Model.FirstName @Model.LastName Welcome to BookMyslot.
//    </div>
//</body>
//</html>";

//        public const string SlotScheduledTemplateBody = @"<!DOCTYPE html>

//<html>
//<head>
//    <style>
//        table {
//            font-family: arial, sans-serif;
//            border-collapse: collapse;
//            width: 100%;
//        }

//        td, th {
//            border: 1px solid #dddddd;
//            text-align: left;
//            padding: 8px;
//        }

//        body{
//            font-family:Arial;
//            font-size:14px;
//        }
//    </style>
//</head>
//<body>
//    <div>
//        Hi @Model.CustomerModel.FirstName @Model.CustomerModel.LastName  has booked below mentioned slot
//    </div>
//    <table>
//        <tr>
//            <th>Slot Title</th>
//            <th>Slot Date</th>
//            <th>Slot Timezone</th>
//            <th>Slot Start Time</th>
//            <th>Slot End Time</th>
//            <th>Slot Duration</th>
//        </tr>
//        <tr>
//            <td>@Model.SlotModel.Title</td>
//            <td>@Model.SlotModel.SlotDate</td>
//            <td>@Model.SlotModel.TimeZone</td>
//            <td>@Model.SlotModel.SlotStartTime</td>
//            <td>@Model.SlotModel.SlotEndTime</td>
//            <td>@Model.SlotModel.SlotDuration</td>
//        </tr>
//    </table>

//</body>
//</html>";





//        public const string SlotCancelledTemplateBody = @"<!DOCTYPE html>

//<html>
//<head>
//    <style>
//        table {
//            font-family: arial, sans-serif;
//            border-collapse: collapse;
//            width: 100%;
//        }

//        td, th {
//            border: 1px solid #dddddd;
//            text-align: left;
//            padding: 8px;
//        }

//        body {
//            font-family: Arial;
//            font-size: 14px;
//        }
//    </style>
//</head>
//<body>
//    <div>
//        Hi @Model.CustomerModel.FirstName @Model.CustomerModel.LastName  has cancelled below booked slot
//    </div>
//    <table>
//        <tr>
//            <th>Slot Title</th>
//            <th>Slot Date</th>
//            <th>Slot Timezone</th>
//            <th>Slot Start Time</th>
//            <th>Slot End Time</th>
//            <th>Slot Duration</th>
//        </tr>
//        <tr>
//            <td>@Model.SlotModel.Title</td>
//            <td>@Model.SlotModel.SlotDate</td>
//            <td>@Model.SlotModel.TimeZone</td>
//            <td>@Model.SlotModel.SlotStartTime</td>
//            <td>@Model.SlotModel.SlotEndTime</td>
//            <td>@Model.SlotModel.SlotDuration</td>
//        </tr>
//    </table>

//</body>
//</html>";





//        public const string ResendSlotInformationTemplateBody = "Hello @Model.FirstName, welcome to RazorEngine!";


//    }
//}
