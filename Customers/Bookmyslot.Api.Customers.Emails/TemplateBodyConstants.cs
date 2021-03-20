﻿namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateBodyConstants
    {

        public const string CustomerRegistrationWelcomeEmailTemplateBody = @"<!DOCTYPE html>

<html>
<head>
    <style>
       
        body {
            font-family: Arial;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <div>
        Hi @Model.FirstName @Model.LastName Welcome to BookMyslot.
    </div>
</body>
</html>";

        public const string SlotScheduledTemplateBody = @"<!DOCTYPE html>

<html>
<head>
    <style>
        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        th{
            background-color: green;
        }

        td, th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }

        body{
            font-family:Arial;
            font-size:14px;
        }
    </style>
</head>
<body>
    <div>
        Hi @Model.BookedBy has booked below mentioned slot
    </div>
    <table>
        <tr>
            <th>Title</th>
            <th>Country</th>
            <th>Timezone</th>
            <th>Date</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Duration</th>
        </tr>
        <tr>
            <td>@Model.Title</td>
            <td>@Model.Country</td>
            <td>@Model.TimeZone</td>
            <td>@Model.SlotDate</td>
            <td>@Model.StartTime</td>
            <td>@Model.EndTime</td>
            <td>@Model.Duration</td>
        </tr>
    </table>

</body>
</html>";





        public const string SlotCancelledTemplateBody = @"<!DOCTYPE html>

<html>
<head>
    <style>
        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        th {
            background-color: red;
        }

        td, th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }

        body {
            font-family: Arial;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <div>
        Hi @Model.CancelledBy has cancelled your below mentioned slot
    </div>
    <table>
        <tr>
            <th>Title</th>
            <th>Country</th>
            <th>Timezone</th>
            <th>Date</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Duration</th>
        </tr>
        <tr>
            <td>@Model.Title</td>
            <td>@Model.Country</td>
            <td>@Model.TimeZone</td>
            <td>@Model.SlotDate</td>
            <td>@Model.StartTime</td>
            <td>@Model.EndTime</td>
            <td>@Model.Duration</td>
        </tr>
    </table>

</body>
</html>";





        public const string SlotMeetingInformationTemplateBody = @"<!DOCTYPE html>

<html>
<head>
    <style>
        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        th {
            background-color: green;
        }

        td, th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }

        body {
            font-family: Arial;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <div>
        Hi below is your meeting link for your slot
    </div>
    <table>
        <tr>
            <th>Title</th>
            <th>Country</th>
            <th>Timezone</th>
            <th>Date</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Duration</th>
            <th>MeetingLink</th>
        </tr>
        <tr>
            <td>@Model.Title</td>
            <td>@Model.Country</td>
            <td>@Model.TimeZone</td>
            <td>@Model.SlotDate</td>
            <td>@Model.StartTime</td>
            <td>@Model.EndTime</td>
            <td>@Model.Duration</td>
            <td>@Model.MeetingLink</td>
        </tr>
    </table>

</body>
</html>";


    }
}
