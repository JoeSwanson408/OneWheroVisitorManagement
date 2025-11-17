using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OneWheroVisitorManagement.AdminEvents.Analytics;

namespace OneWheroVisitorManagement.AdminEvents.Controllers
{
    [ApiController]
    [Route("admin/events/analytics")]
    public class AdminEventsAnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analytics;

        public AdminEventsAnalyticsController(IAnalyticsService analytics)
        {
            _analytics = analytics ?? throw new ArgumentNullException(nameof(analytics));
        }

        // GET admin/events/analytics/top?top=50&descending=true
        [HttpGet("top")]
        public ActionResult<IEnumerable<AnalyticsEventStats>> GetTopEvents([FromQuery] int top = 50, [FromQuery] bool descending = true)
        {
            var sorted = _analytics.GetEventsSorted(descending);
            var result = System.Linq.Enumerable.Take(sorted, Math.Max(0, top));
            return Ok(result);
        }

        // GET admin/events/analytics/totals
        [HttpGet("totals")]
        public ActionResult<IDictionary<string, AnalyticsEventStats>> GetTotals()
        {
            return Ok(_analytics.GetEventTotals());
        }

        // POST admin/events/analytics/reset   (admin only - protect this endpoint in real app)
        [HttpPost("reset")]
        public ActionResult Reset()
        {
            _analytics.Reset();
            return NoContent();
        }
    }
}