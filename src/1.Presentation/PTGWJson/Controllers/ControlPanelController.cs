using Crosscutting.ControlPanel;
using Microsoft.AspNetCore.Mvc;

namespace IGwApi.Controllers
{
    [Route("api/[controller]")]
    public class ControlPanelController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            var t = new ControlPanelStatus();
            t.HostName = System.Net.Dns.GetHostName();
            t.Status = ControlPanel.Status;
            t.Reason = ControlPanel.Reason;
            t.MealPlanMappingsCount = ControlPanel.MealPlanMappingsCount;
            t.DatabaseOk = ControlPanel.DatabaseOk;
            t.BookingConsecutiveErrors = ControlPanel.BookingConsecutiveErrors;
            t.AvailabilityConsecutiveErrors = ControlPanel.AvailabilityConsecutiveErrors;
            t.AccommodationMappingsCount = ControlPanel.AccommodationMappingsCount;

            return new OkObjectResult(t);
        }

        public class ControlPanelStatus
        {
            public string? HostName { get; set; }
            public HealthStatus Status { get; set; }
            public string? Reason { get; set; }
            public int AccommodationMappingsCount { get; set; }
            public int MealPlanMappingsCount { get; set; }
            public int AvailabilityConsecutiveErrors { get; set; }
            public int BookingConsecutiveErrors { get; set; }
            public int QueueLength { get; set; }
            public int QueueFailedStats { get; set; }
            public bool DatabaseOk { get; set; }
        }
    }
}