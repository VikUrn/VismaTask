using System.Collections.Generic;

namespace MeetingsApp.Model
{
    public class Meeting
    {
        public string Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<Person> Users { get; set; }
    }
}
