using System.Diagnostics;
using Application.Dto.ValuationService;
using Application.WorkFlow.Contracts;
using Domain.Error;
using Domain.Valuation;
using Microsoft.AspNetCore.Mvc;

namespace IGwApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuationController : ControllerBase
    {
        private readonly IValuationService _valuationService;

        public ValuationController(IValuationService valuationService)      
        {
            _valuationService = valuationService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post([FromBody] ValuationQuery query)
        {
            var res = new Valuation();
            var t = Stopwatch.StartNew();
            
            try
            {
                res = await _valuationService.GetValuation(query);  
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
