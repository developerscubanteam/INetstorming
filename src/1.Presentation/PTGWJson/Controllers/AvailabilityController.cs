using System.Diagnostics;
using Application.Dto.AvailabilityService;
using Application.WorkFlow.Contracts;
using Domain.Availability;
using Domain.Valuation;
using Domain.Error;
using Microsoft.AspNetCore.Mvc;

namespace IGwApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post([FromBody] AvailabilityQuery query)
        {
            var t = Stopwatch.StartNew();
            var res = new Availability();
            try
            {
                res = await _availabilityService.GetAvailability(query);
                t.Stop();
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Hub);
                res.Errors = new List<Error>() { error };                
            }
            return Ok(res);
        }
    }
}
