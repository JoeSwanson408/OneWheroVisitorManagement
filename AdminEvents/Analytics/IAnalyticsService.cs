using System;
using System.Collections.Generic;

namespace OneWheroVisitorManagement.AdminEvents.Analytics
{
    public interface IAnalyticsService
    {
        // Record a booking for an event. eventId must be stable (e.g., GUID or numeric id).
        void TrackBooking(string eventId, string eventName, DateTime when);

        // Returns a dictionary keyed by eventId
        IReadOnlyDictionary<string, AnalyticsEventStats> GetEventTotals();

        // Returns events sorted by Count. descending=true => most popular first
        IEnumerable<AnalyticsEventStats> GetEventsSorted(bool descending = true);

        // Force save to persistent storage
        void Save();

        // Load from persistent storage (called in ctor typically)
        void Load();

        // Clear analytics (for testing/admin)
        void Reset();
    }
}