````markdown
# AdminEvents Analytics

What I added
- Analytics subsystem placed under OneWheroVisitorManagement.AdminEvents.Analytics
- Admin API controller at admin/events/analytics with endpoints:
  - GET /admin/events/analytics/top?top=50&descending=true  -> returns sorted events (most → least by default)
  - GET /admin/events/analytics/totals -> returns raw totals per event
  - POST /admin/events/analytics/reset -> clears analytics (protect this behind auth)

How to integrate
1. Add the files in this AdminEvents folder into your project.
2. Register the singleton in DI (example for ASP.NET Core):
   services.AddSingleton<OneWheroVisitorManagement.AdminEvents.Analytics.IAnalyticsService>(_ => 
       new OneWheroVisitorManagement.AdminEvents.Analytics.AnalyticsService("data/analytics.json"));
3. In your booking flow, after a successful booking save, call:
   analytics.TrackBooking(eventId, eventName, DateTime.UtcNow);
4. Use the AdminEventsAnalyticsController endpoints in your admin UI to show sorted lists.

Notes & suggestions
- The storage file default is data/analytics.json; change path when registering if needed.
- For multi-server deployments, replace AnalyticsRepository with a shared DB-backed store.
- Protect the admin endpoints with authentication/authorization.
````