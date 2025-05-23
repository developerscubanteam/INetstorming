using System.Diagnostics;
using Application.Dto.BookingCancelService;
using Application.Dto.BookingCreateService;
using Application.Dto.BookingsService;
using Application.WorkFlow.Contracts;
using Domain.Booking;
using Domain.Error;
using Microsoft.AspNetCore.Mvc;

namespace IGwApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingCreateService _bookingCreateService;
        private readonly IBookingsService _bookingsService;
        private readonly IBookingCancelService _bookingCancelService;

        public BookingController(IBookingCreateService bookingCreateService, IBookingsService bookingGetService, IBookingCancelService bookingCancelService)        
        {
            _bookingCreateService = bookingCreateService;
            _bookingsService = bookingGetService;
            _bookingCancelService = bookingCancelService;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> Post([FromBody] BookingCreateQuery query)
        {
            var res = new Booking();
            var t = Stopwatch.StartNew();
            
            try
            {
                res = await _bookingCreateService.CreateBooking(query);                 
                t.Stop();
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Hub);
                res.Errors = new List<Error>() { error };
            }
            return Ok(res);
        }

        [HttpPost("get")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> PostGet([FromBody] BookingsQuery query)
        {
            var res = new Bookings();
            var t = Stopwatch.StartNew();
    
            try
            {
                res = await _bookingsService.GetBookings(query);                
                t.Stop();
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Hub);
                res.Errors = new List<Error>() { error };
            }
            return Ok(res);
        }

        [HttpPut("cancel")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> PostCancel([FromBody] BookingCancelQuery query)
        {
            var res = new Booking();
            var t = Stopwatch.StartNew();            
            try
            {
                res = await _bookingCancelService.CancelBooking(query);
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
