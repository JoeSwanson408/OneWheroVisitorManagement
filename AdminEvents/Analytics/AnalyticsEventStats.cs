namespace OneWheroVisitorManagement.AdminEvents.Analytics
{
    using System;
    using System.Text.Json.Serialization;

    public class AnalyticsEventStats
    {
        [JsonPropertyName("eventId")]
        public string EventId { get; set; }

        [JsonPropertyName("eventName")]
        public string EventName { get; set; }

        [JsonPropertyName("count")]
        public long Count { get; set; }

        // last time this event was booked
        [JsonPropertyName("lastBookedUtc")]
        public DateTime? LastBookedUtc { get; set; }
    }
}