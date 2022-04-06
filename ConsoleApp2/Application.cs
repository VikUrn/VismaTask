using MeetingsApp.Model;
using MeetingsApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingsApp
{
    public class Application
    {
        private readonly IFunctions _functions;

        private readonly string path = @"C:\Users\vikto\OneDrive\Stalinis kompiuteris\uzduotys praktikai\visma\ConsoleApp2\meetings.json";

        public Application(IFunctions functions)
        {
            _functions = functions;
        }

        public void Run()
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(System.Text.Json.JsonSerializer.Serialize(new List<Meeting>()));
                }
            }

            do
            {
                bool stopProgram = false;

                Console.WriteLine("1.Create Meeting");
                Console.WriteLine("2.Delete meeting");
                Console.WriteLine("3.Add user to the meeting");
                Console.WriteLine("4.Remove person from meeting");
                Console.WriteLine("5.Show all meetings");
                Console.WriteLine("6.Clear Console");
                Console.WriteLine("7.Exit");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        if (_functions.CreateMeeting(path) != null)
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("Meet created");
                        }
                        else
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("Meet with this name already exists");
                        }

                        Console.WriteLine("-----------------------------------------");
                        break;

                    case "2":
                        Console.WriteLine("Enter your name");
                        string userName = Console.ReadLine();

                        var result = _functions.UsersMeeting(path, userName);
                        if (result == null)
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("You got no rooms");
                        }
                        else
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("Your meetings as responsible person");
                            foreach (var item in result)
                            {
                                Console.WriteLine(item.Name);
                            }

                            Console.WriteLine("Enter meeting name which you want to delete");
                            string meetingNameInput = Console.ReadLine();

                            var delete = _functions.DeleteMeeting(path, userName, meetingNameInput);
                            if (delete != null)
                            {
                                Console.WriteLine("-----------------------------------------");
                                Console.WriteLine("Room Deleted");
                            }
                            else
                            {
                                Console.WriteLine("-----------------------------------------");
                                Console.WriteLine("You don't have accese to delete this room");
                            }
                        }
                        Console.WriteLine("-----------------------------------------");
                        break;

                    case "3":
                        var meetings = _functions.AllMeetings(path);
                        if (meetings.Count != 0)
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("Meetings");
                            foreach (var meeting in meetings)
                            {
                                Console.WriteLine(meeting.Name);
                            }
                        } else
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("No Meetings found");
                            Console.WriteLine("-----------------------------------------");
                            break;
                        }
                        


                        Console.WriteLine("Enter meeting room in which you want to add user");
                        string meetingName = Console.ReadLine();

                        Console.WriteLine("Enter user name");
                        string userNamee = Console.ReadLine();

                        var personAdd = _functions.AddPerson(path, meetingName, userNamee);

                        if (personAdd == null)
                        {
                            Console.WriteLine("This user is already in meeting");
                        }
                        else
                        {
                            Console.WriteLine("User added");
                        }
                        Console.WriteLine("-----------------------------------------");
                        break;

                    case "4":
                        Console.WriteLine("Enter user name");
                        string userNamea = Console.ReadLine();

                        var personRemove = _functions.RemovePerson(path, userNamea);

                        if (personRemove == null)
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("User not found");
                        }
                        else
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("User deleted from meeting");
                        }
                        Console.WriteLine("-----------------------------------------");
                        break;

                    case "5":

                        var allMetings = _functions.AllMeetings(path);

                        Console.WriteLine("Choose");
                        Console.WriteLine("1.Return all meetings");
                        Console.WriteLine("2.Filter by description");
                        Console.WriteLine("3.Filter by responsible person");
                        Console.WriteLine("4.Filter by category");
                        Console.WriteLine("5.Filter by type");
                        Console.WriteLine("6.Filter by dates");
                        Console.WriteLine("7.Filter by attendees");
                        var o = Console.ReadLine();

                        if (o == "1")
                        {
                            Console.WriteLine("-----------------------------------------");
                            if (allMetings.Count != 0)
                            {
                                foreach (var meet in allMetings)
                                {
                                    Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No meetings found");
                            }

                            Console.WriteLine("-----------------------------------------");
                        }
                        else if (o == "2")
                        {

                            var count = 0;
                            Console.WriteLine("Enter description");
                            var descriptionInput = Console.ReadLine();
                            Console.WriteLine("-----------------------------------------");

                            foreach (var meet in allMetings)
                            {
                                var meetDescription = meet.Description.Split(" ");
                                foreach (var word in meetDescription)
                                {
                                    if (descriptionInput == word)
                                    {
                                        count++;
                                        Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                    }
                                }
                            }
                            if (count == 0)
                            {
                                Console.WriteLine("No meetings found");
                            }
                            Console.WriteLine("-----------------------------------------");

                        }
                        else if (o == "3")
                        {
                            Console.WriteLine("Enter responsible person name");
                            var filterByOption = Console.ReadLine();

                            var filteredBy = allMetings.Where((meet) => meet.ResponsiblePerson == filterByOption).ToList();
                            Console.WriteLine("-----------------------------------------");
                            if (filteredBy.Count != 0)
                            {
                                foreach (var meet in filteredBy)
                                {
                                    Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("This user not responsible for any meeting");
                            }

                            Console.WriteLine("-----------------------------------------");


                        }
                        else if (o == "4")
                        {
                            Console.WriteLine("Enter category");
                            var filterByOption = Console.ReadLine();

                            var filteredBy = allMetings.Where((meet) => meet.Category == filterByOption).ToList();
                            Console.WriteLine("-----------------------------------------");
                            if (filteredBy.Count != 0)
                            {
                                foreach (var meet in filteredBy)
                                {
                                    Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Result 0");
                            }
                            Console.WriteLine("-----------------------------------------");
                        }
                        else if (o == "5")
                        {
                            Console.WriteLine("Enter type");
                            var filterByOption = Console.ReadLine();

                            var filteredBy = allMetings.Where((meet) => meet.Type == filterByOption).ToList();
                            Console.WriteLine("-----------------------------------------");
                            if (filteredBy.Count != 0)
                            {
                                foreach (var meet in filteredBy)
                                {
                                    Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Result 0");
                            }
                            Console.WriteLine("-----------------------------------------");
                        }
                        else if (o == "6")
                        {

                            Console.WriteLine("Enter start date");
                            var startDateInput = Console.ReadLine();

                            Console.WriteLine("Enter start date");
                            var endtDateInput = Console.ReadLine();

                            var filteredBy = allMetings.Where((meet) => meet.StartDate.CompareTo(startDateInput) >= 0 && meet.StartDate.CompareTo(endtDateInput) <= 0).ToList();

                            Console.WriteLine("-----------------------------------------");
                            if (filteredBy.Count != 0)
                            {
                                foreach (var meet in filteredBy)
                                {
                                    Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Result 0");
                            }
                            Console.WriteLine("-----------------------------------------");

                        }
                        else if (o == "7")
                        {

                            var filteredBy = allMetings.Where((meet) => meet.Users.Count >= 10).ToList();
                            Console.WriteLine("-----------------------------------------");
                            if (filteredBy.Count != 0)
                            {
                                foreach (var meet in filteredBy)
                                {
                                    Console.WriteLine($"Meet name: {meet.Name}| Responsible person: {meet.ResponsiblePerson}| Description: {meet.Description}| Category: {meet.Category}| Type: {meet.Type}| Start date: {meet.StartDate}| End date {meet.EndDate}| Users length {meet.Users.Count()}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Result 0");
                            }
                            Console.WriteLine("-----------------------------------------");
                        }

                        break;

                    case "6":
                        Console.Clear();
                        break;

                    case "7":
                        stopProgram = true;
                        break;

                    default:

                        break;
                }


                if (stopProgram == true) break;


            } while (true);


        }
    }
}
