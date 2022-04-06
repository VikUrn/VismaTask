using MeetingsApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeetingsApp.Services
{
    public class Functions : IFunctions
    {
        enum Type
        {
            Live,
            InPerson
        }

        enum Category
        {
            CodeMonkey,
            Hub,
            Short,
            TeamBuilding
        }

        public Person AddPerson(string path, string meetingName, string userName)
        {

            var time = DateTime.Now.ToString("HH:mm");

            var data = File.ReadAllText(path);

            var dataObject = JsonConvert.DeserializeObject<List<Meeting>>(data);

            var meet = dataObject.Where((meet) => meet.Name == meetingName).SingleOrDefault();

            var person = new Person() { Name = userName, AddedTime = time};

            var found = false;

            foreach (var user in meet.Users)
            {
                if (user.Name == userName)
                {
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                meet.Users.Add(person);
            } else
            {
                return null;
            }


            var dataJson = System.Text.Json.JsonSerializer.Serialize(dataObject);
            File.WriteAllText(path, dataJson);

            return person;

        }

        public List<Meeting> AllMeetings(string path)
        {

            var data = File.ReadAllText(path);

            var dataObject = JsonConvert.DeserializeObject<List<Meeting>>(data);

            return dataObject;
        }

        public Meeting CreateMeeting(string path)
        {

            var data = File.ReadAllText(path);

            var dataObject = JsonConvert.DeserializeObject<List<Meeting>>(data);

            Console.WriteLine("Enter meeting name");
            string name = Console.ReadLine();

            var meet = dataObject.Where((meet) => meet.Name == name).SingleOrDefault();

            if (meet != null)
            {
                return null;
            }

            Console.WriteLine("Enter responsible person");
            string responsiblePerson = Console.ReadLine();

            Console.WriteLine("Enter description");
            string description = Console.ReadLine();

            string categoryOption = "";

            do
            {
                Console.WriteLine($"Pick category : 1.{Category.CodeMonkey.ToString()}, 2.{Category.Hub.ToString()}, 3.{Category.Short.ToString()}, 4.{Category.TeamBuilding.ToString()}");
                string category = Console.ReadLine();

                var picked = false;

                switch (category)
                {
                    case "1":
                        categoryOption = Category.CodeMonkey.ToString();
                        picked = true;
                        break;

                    case "2":
                        categoryOption = Category.Hub.ToString();
                        picked = true;
                        break;

                    case "3":
                        categoryOption = Category.Short.ToString();
                        picked = true;
                        break;

                    case "4":
                        categoryOption = Category.TeamBuilding.ToString();
                        picked = true;
                        break;

                    default:

                        break;
                }

                if (picked == true) break;

            } while (true);


            string typeOption = "";

            do
            {
                Console.WriteLine($"Pick type : 1.{Type.InPerson.ToString()}, 2.{Type.Live.ToString()}");
                string type = Console.ReadLine();

                var picked = false;

                switch (type)
                {
                    case "1":
                        typeOption = Type.InPerson.ToString();
                        picked = true;
                        break;
                    case "2":
                        typeOption = Type.Live.ToString();
                        picked = true;
                        break;
                    default:
                        break;
                }
                if (picked == true) break;
            } while (true);


            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var splitedDate = date.Split("-");

            int howMuchMonthGotdays = DateTime.DaysInMonth(int.Parse(splitedDate[0]), int.Parse(splitedDate[1]));

            Console.WriteLine($"Enter meeting day (Today is {splitedDate[2]} This month have {howMuchMonthGotdays} days)");
            
            string starEndDate = "";

            do
            {
                bool correct = false;

                int count = 0;

                string startEndDayInput = Console.ReadLine();

                if (int.Parse(startEndDayInput) < int.Parse(splitedDate[2]))
                {
                    
                } else
                {
                    count++;
                }

                if (int.Parse(startEndDayInput) > howMuchMonthGotdays)
                {
                    
                } else
                {
                    count++;
                }

                if (count == 2)
                {
                    starEndDate = $"{splitedDate[0]}-{splitedDate[1]}-{startEndDayInput}";
                    correct = true;
                }

                if (correct == true) break;

            } while (true);

            var newMeating = new Meeting() 
            { 
                Name = name,
                ResponsiblePerson = responsiblePerson,
                Description = description,
                Category = categoryOption,
                Type = typeOption,
                StartDate = starEndDate,
                EndDate = starEndDate,
                Users = new List<Person>()
            };

            dataObject.Add(newMeating);

            var dataJson = System.Text.Json.JsonSerializer.Serialize(dataObject);
            File.WriteAllText(path, dataJson);

            return newMeating;
        }

        public Meeting DeleteMeeting(string path, string userName, string meetingName)
        {
            var data = File.ReadAllText(path);

            var dataObject = JsonConvert.DeserializeObject<List<Meeting>>(data);

            var meet = dataObject.Where((room) => room.Name == meetingName).SingleOrDefault();

            if (meet.ResponsiblePerson == userName)
            {
                dataObject.Remove(meet);
            } else
            {
                return null;
            }

            var dataJson = System.Text.Json.JsonSerializer.Serialize(dataObject);

            File.WriteAllText(path, dataJson);

            return meet;
        }

        public List<Meeting> UsersMeeting(string path, string userName)
        {
            var data = File.ReadAllText(path);

            var dataObject = JsonConvert.DeserializeObject<List<Meeting>>(data);

            var getRooms = dataObject.Where((room) => room.ResponsiblePerson == userName).ToList();

            if (getRooms.Count == 0)
            {
                return null;
            }

            return getRooms;

        }

        public Person RemovePerson(string path, string userName)
        {

            var data = File.ReadAllText(path);

            var dataObject = JsonConvert.DeserializeObject<List<Meeting>>(data);

            var roomNameInWhichIsUser = "";

            foreach (var meeting in dataObject)
            {
                bool found = false;

                if (meeting.Users != null)
                {
                    foreach (var user in meeting.Users)
                    {
                        if (user.Name == userName)
                        {
                            found = true;
                        }
                    }
                }

                if(found == true)
                {
                    roomNameInWhichIsUser = meeting.Name;
                    break;
                }

            }

            if (roomNameInWhichIsUser != "")
            {
                var meetingRoom = dataObject.Where((room) => room.Name == roomNameInWhichIsUser).SingleOrDefault();

                var userFromMeeting = meetingRoom.Users.Where((user) => user.Name == userName).SingleOrDefault();

                meetingRoom.Users.Remove(userFromMeeting);

                var dataJson = System.Text.Json.JsonSerializer.Serialize(dataObject);
                File.WriteAllText(path, dataJson);

                return userFromMeeting;
            } else
            {
                return null;
            }

        }

    }
}
