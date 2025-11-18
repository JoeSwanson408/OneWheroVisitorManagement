using System;

namespace YourNamespace.Models
{
    // Minimal Event model used for display/sorting.
    public class Event
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        // Number of visitors/attendees — used for sorting
        public int Visitors { get; set; }

        // Add other properties as needed (Id, Location, Description, etc.)
    }
}