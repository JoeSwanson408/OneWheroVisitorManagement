using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OneWheroVisitorManagement.AdminEvents.Analytics
{
    public class AnalyticsService : IAnalyticsService, IDisposable
    {
        private readonly ConcurrentDictionary<string, AnalyticsEventStats> _store;
        private readonly AnalyticsRepository _repo;
        private readonly Timer _autosaveTimer;
        private readonly object _saveLock = new object();

        // Autosave every N seconds to persist data periodically
        private const int AutosaveSeconds = 30;

        public AnalyticsService(string storageFilePath = "data/analytics.json")
        {
            _repo = new AnalyticsRepository(storageFilePath);
            var dict = _repo.Load();
            _store = new ConcurrentDictionary<string, AnalyticsEventStats>(dict, StringComparer.OrdinalIgnoreCase);
            _autosaveTimer = new Timer(_ => Save(), null, AutosaveSeconds * 1000, AutosaveSeconds * 1000);
        }

        public void TrackBooking(string eventId, string eventName, DateTime when)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                throw new ArgumentException("eventId is required", nameof(eventId));

            _store.AddOrUpdate(eventId,
                id => new AnalyticsEventStats
                {
                    EventId = eventId,
                    EventName = eventName ?? string.Empty,
                    Count = 1,
                    LastBookedUtc = when.ToUniversalTime()
                },
                (id, existing) =>
                {
                    existing.EventName = !string.IsNullOrWhiteSpace(eventName) ? eventName : existing.EventName;
                    existing.Count += 1;
                    existing.LastBookedUtc = when.ToUniversalTime();
                    return existing;
                });
        }

        public IReadOnlyDictionary<string, AnalyticsEventStats> GetEventTotals()
        {
            return _store.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<AnalyticsEventStats> GetEventsSorted(bool descending = true)
        {
            var snapshot = _store.Values.ToArray();
            return descending
                ? snapshot.OrderByDescending(e => e.Count).ThenBy(e => e.EventName, StringComparer.OrdinalIgnoreCase)
                : snapshot.OrderBy(e => e.Count).ThenBy(e => e.EventName, StringComparer.OrdinalIgnoreCase);
        }

        public void Save()
        {
            lock (_saveLock)
            {
                try
                {
                    var dict = _store.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
                    _repo.Save(dict);
                }
                catch
                {
                    // swallow - best-effort
                }
            }
        }

        public void Load()
        {
            lock (_saveLock)
            {
                var dict = _repo.Load();
                _store.Clear();
                foreach (var kv in dict)
                {
                    _store.TryAdd(kv.Key, kv.Value);
                }
            }
        }

        public void Reset()
        {
            _store.Clear();
            Save();
        }

        public void Dispose()
        {
            _autosaveTimer?.Dispose();
            Save();
        }
    }
}