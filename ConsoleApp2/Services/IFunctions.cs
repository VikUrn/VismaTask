using MeetingsApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingsApp.Services
{
    public interface IFunctions
    {
        Meeting CreateMeeting (string path);
        Meeting DeleteMeeting (string path, string userName, string meetingName);
        List<Meeting> UsersMeeting (string path, string userName);
        Person AddPerson (string path, string meetingName, string userName);
        List<Meeting> AllMeetings (string path);
        Person RemovePerson (string path, string userName);

    }
}
