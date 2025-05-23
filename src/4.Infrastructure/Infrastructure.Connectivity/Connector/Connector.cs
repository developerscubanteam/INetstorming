using Domain.Common;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using System.Globalization;
using AvailabilityRq = Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ;
using BookingRq = Infrastructure.Connectivity.Connector.Models.Message.BookingRQ;



namespace Infrastructure.Connectivity.Connector
{
    public class Connector : IConnector
    {
        private readonly IHttpWrapper _httpWrapper;
        public Connector(IHttpWrapper httpWrapper)
        {
            _httpWrapper = httpWrapper;
        }

        public async Task<(AvailabilityRS? AvailabilityRs, List<Domain.Error.Error>? Errors, AuditData AuditData)> Availability(AvailabilityConnectorQuery query)
        {
            var availabilityRQ = BuildAvalabilityRequest(query);
            var availabilityResult = await _httpWrapper.Availability(query.ConnectionData, query.AuditRequests, query.Timeout, availabilityRQ);

            return availabilityResult;
        }

        public async Task<(ValuationRS? ValuationRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Valuation(ValuationConnectorQuery query)
        {
            var hotelValuationRq = BuildValuationRequest(query);
            var response = await _httpWrapper.Valuation(query.ConnectionData, query.Timeout, hotelValuationRq);
            return response;
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CreateBooking(BookingConnectorQuery query)
        {
            var hotelBookingRulesRQ = BuildBookingRequest(query);
            var response = await _httpWrapper.Booking(query.ConnectionData, hotelBookingRulesRQ);

            return response;
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> GetBookings(BookingsConnectorQuery query)
        {
            var getBookingRQ = BuildBookingGetRequest(query);
            var response = await _httpWrapper.GetBookings(query.ConnectionData, getBookingRQ);
            return response;
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CancelBooking(BookingCancelConnectorQuery query)
        {
            var getBookingRQ = BuildCancelBookingRequest(query);
            var response = await _httpWrapper.CancelBooking(query.ConnectionData, getBookingRQ);

            return response;
        }

        private AvailabilityRq.AvailabilityRQ BuildAvalabilityRequest(AvailabilityConnectorQuery query)
        {
            //TODO: Implement this method
            var hotelAvailRQ = new AvailabilityRq.AvailabilityRQ();

            return hotelAvailRQ;
        }


        private Models.Message.ValuationRQ.ValuationRQ BuildValuationRequest(ValuationConnectorQuery query)
        {
            // TODO: Implement this method
            var valuationRq = new Models.Message.ValuationRQ.ValuationRQ();
            return valuationRq;
        }

      

        private Models.Message.BookingRQ.BookRQ BuildBookingRequest(BookingConnectorQuery query)
        {
            // TODO: Implement this method
            var vc = query.ValuationCode;
            var bc = query.BookingCode;

            var result = new Models.Message.BookingRQ.BookRQ();            

            return result;
        }

        private Models.Message.BookingRQ.BookRQ BuildBookingGetRequest(BookingsConnectorQuery query)
        {
            //TODO: Implement this method
            var result = new Models.Message.BookingRQ.BookRQ();

            return result;
        }
        private Models.Message.BookingRQ.BookRQ BuildCancelBookingRequest(BookingCancelConnectorQuery query)
        {
            //TODO: Implement this method
            var result = new Models.Message.BookingRQ.BookRQ();

            return result;
        }

    }
}
