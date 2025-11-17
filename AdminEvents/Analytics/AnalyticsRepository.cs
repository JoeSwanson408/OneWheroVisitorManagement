using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OneWheroVisitorManagement.AdminEvents.Analytics
{
    internal class AnalyticsRepository
    {
        private readonly string _filePath;

        public AnalyticsRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public Dictionary<string, AnalyticsEventStats> Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new Dictionary<string, AnalyticsEventStats>(StringComparer.OrdinalIgnoreCase);

                var json = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, AnalyticsEventStats>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return data ?? new Dictionary<string, AnalyticsEventStats>(StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                return new Dictionary<string, AnalyticsEventStats>(StringComparer.OrdinalIgnoreCase);
            }
        }

        public void Save(Dictionary<string, AnalyticsEventStats> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, json);
        }
    }
}