using Application.Dto.AvailabilityService;
using Application.Dto.BookingCancelService;
using Application.Dto.BookingCreateService;
using Application.Dto.BookingsService;
using Application.Dto.Common;
using Application.Dto.ValuationService;
using Domain.Availability;
using Domain.Booking;
using Domain.Valuation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using Prg = IJuniperApi

namespace Ijuniper.test
{
    public class Tests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var application = new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>();
            _client = application.CreateClient();
        }


        [Test]
        public async Task MethodIbossyAvailability()
        {
            AvailabilityQuery query = new AvailabilityQuery()
            {

                ExternalSupplier = new ExternalSupplier()
                {

                    Code = "IJuniper",
                    Connection = new System.Collections.Generic.Dictionary<string, string>()
                    {
                        {"Url", "https://test.netstorming.net/kalima/call.php" },
                        {"User", "xmlusers" },
                        {"Password", "methabookxml" },
                        {"Actor", "METHABOOK" }
                    }
                    ,
                    Params = new System.Collections.Generic.Dictionary<string, string>()
                    {
                        { "NumberAccommodationPerRequest", "150" }
                    }
                },
                SearchCriteria = new SearchCriteria()
                {
                    AccommodationCodes = [
                       //"225776", "98009", "64234", "9411", "225539", "225753"
                       "182560"
                     ],
                    CheckIn = new DateTime(2025, 07, 26),
                    CheckOut = new DateTime(2025, 07, 27),
                    Currency = "USD",
                    Nationality = "ES",
                    Language = "es",
                    RoomCandidates = [
                         new Application.Dto.AvailabilityService.Room(){
                             PaxesAge = [20,30],
                             RoomRefId = 1
                         },
                         //new Application.Dto.AvailabilityService.Room(){
                         //    PaxesAge = new List<byte>(){ 30},
                         //    RoomRefId = 2
                         //},
                         //new Application.Dto.AvailabilityService.Room(){
                         //    PaxesAge = new List<byte>(){ 40,45},
                         //    RoomRefId = 3
                         //},
                         //new Application.Dto.AvailabilityService.Room(){
                         //    PaxesAge = new List<byte>(){ 40,50},
                         //    RoomRefId = 4
                         //},
                     ]
                },
                Timeout = 100000,
                AuditRequests = true,
                Include = new Dictionary<string, List<string>>()
                {
                    {"accommodations", new(){"name"} },
                    {"remarks", new(){ } },
                    {"fees", new(){ } },
                    {"rooms", new(){"name", "description", "occupancy" } },
                    {"occupancy", new(){ } },
                    {"cancellationpolicy", new(){ } },
                    {"promotions", new(){ } },
                    {"mealplan", new(){ "name" } },
                    {"root", new (){"paymenttype", "code", "name", "remarks" }},
                    {"price", new(){ } },
                    {"bookings", new (){"cancellocator", "hcn", "checkin", "checkout", "clientreference", "comments" } },
                    {"holder", new(){ } },
                    {"hotel", new(){ "name" } },
                    {"paxes", new(){ "title", "address", "country", "city", "age", "document", "email", "idpax", "phonenumber", "postalcode" } }
                },
            };
            HttpRequestMessage request = GetRequest("api/Availability"
                , "BEDBDDDB5813A41E2B248329CDB4C884B23D0FF4F95C6AA10840B8B761B059F3");
            request.Content = new StringContent(Serializar(query), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
            // responseString = responseString.Replace("\"", "");
            Availability trueObject = Deserializar<Availability>(responseString);
            Assert.IsNotNull(trueObject.Accommodations);

            TestContext.Out.WriteLine(trueObject.Accommodations[0].Mealplans[0].Combinations[0].ValuationCode);
            var refundableComb = trueObject.Accommodations[0].Mealplans[0].Combinations.FirstOrDefault(x => !(bool)x.NonRefundable);
            if (refundableComb != null)
            {
                TestContext.Out.WriteLine("valcode:" + refundableComb.ValuationCode);
                TestContext.Out.WriteLine("Policy:" + refundableComb.CancellationPolicy.PenaltyRules[0].DateFrom);
            }
        }

        [Test]
        public async Task MethodValuation()
        {
            var vc = "182560^[LCL.10000022^[11800,00^[9998605^[1~,dbl~,2~,~,False~,False~,20,30^[2025-07-26^[2025-07-27^[ES^[100000";

            var valRequest = GetRequest("api/Valuation", "BEDBDDDB5813A41E2B248329CDB4C884B23D0FF4F95C6AA10840B8B761B059F3");
            ValuationQuery valQuery = new ValuationQuery()
            {
                ExternalSupplier = new ExternalSupplier()
                {

                    Code = "IPaximum",
                    Connection = new System.Collections.Generic.Dictionary<string, string>()
                    {
                        {"Url", "https://test.netstorming.net/kalima/call.php" },
                        {"User", "xmlusers" },
                        {"Password", "methabookxml" },
                        {"Actor", "METHABOOK" }
                    }
                    ,
                    Params = new System.Collections.Generic.Dictionary<string, string>()
                    {
                        { "NumberAccommodationPerRequest", "150" }
                    }
                },
                ValuationCode = vc,
                Include = new Dictionary<string, List<string>>()
                {
                    {"accommodations", new(){"name"} },
                    {"remarks", new(){ } },
                    {"fees", new(){ } },
                    {"rooms", new(){"name", "description", "occupancy" } },
                    {"occupancy", new(){ } },
                    {"cancellationpolicy", new(){ } },
                    {"promotions", new(){ } },
                    {"mealplan", new(){ "name" } },
                    {"root", new (){"paymenttype", "code", "name", "remarks" }},
                    {"price", new(){ } },
                    {"bookings", new (){"cancellocator", "hcn", "checkin", "checkout", "clientreference", "comments" } },
                    {"holder", new(){ } },
                    {"hotel", new(){ "name" } },
                    {"paxes", new(){ "title", "address", "country", "city", "age", "document", "email", "idpax", "phonenumber", "postalcode" } }
                },
                Timeout = 180000,
            };
            // valQuery = JsonSerializer.Deserialize<ValuationQuery>(vc, Extension.Configure());
            valRequest.Content = new StringContent(Serializar(valQuery), Encoding.UTF8, "application/json");

            var valResponse = await _client.SendAsync(valRequest);
            var varResponseString = await valResponse.Content.ReadAsStringAsync();
            var valTrueObject = Deserializar<Valuation>(varResponseString);
            TestContext.Out.WriteLine(Serializar(valTrueObject));
            TestContext.Out.WriteLine(valTrueObject.Price.Purchase.Cost.Amount.ToString());

            Assert.IsNotNull(valTrueObject.Rooms);
        }

        [Test]
        public async Task MethodBooking()
        {
            var bc = "d1fc6709-991c-4ae4-8826-5cf05718efda^[1~,20,23,__^[DE^[^]d1fc6709-991c-4ae4-8826-5cf05718efda^]6b34d56e-3740-407c-833a-82d3fc05dfdc^]DE^]11/21/2024 12:01:45 AM^]";


            var bookRequest = GetRequest("api/Booking/create", "BEDBDDDB5813A41E2B248329CDB4C884B23D0FF4F95C6AA10840B8B761B059F3");
            BookingCreateQuery bookQuery = new BookingCreateQuery()
            {
                ExternalSupplier = new ExternalSupplier()
                {
                    Code = "IPaximum",
                    Connection = new System.Collections.Generic.Dictionary<string, string>()
                    {

                        { "Url","http://api.stage.paximum.com"},
                        { "ApiKey","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkYjkwZmJmZi03YWNkLTQyNDctYmZlNi01MTk4NzkwNWJlNzIiLCJyb2xlIjoiYjJjOmFwcCIsIm5iZiI6MTcyNDY1NjQwMSwiZXhwIjoxODgyNDIyNzYyLCJpYXQiOjE3MjQ2NTY0MDEsImlzcyI6Imh0dHBzOi8vYXV0aC5wYXhpbXVtLmNvbSIsImF1ZCI6Imh0dHBzOi8vYXBpLnBheGltdW0uY29tIn0.iBfdfn6wSUFsjC1lGcZNd9cYFRmAjktNx_cFiIx0Oak"},
                    }
                }
                ,
                BookingCode = bc
                ,
                Locator = "Test" + DateTime.Now.Ticks.ToString()
                ,
                Rooms =
                [
                     new Application.Dto.BookingCreateService.Room(){
                        Paxes = [
                            new Pax(){ Id = 1,Name = "Test", Surname = "Test", Age = 20, Title="Mr", Email = "test@test.test"},
                            new Pax(){ Id = 2,Name = "Test1", Surname = "Test1", Age = 23, Title="Mr",  Email = "test@test.test"}
                        ],

                },


                ],
                Holder = new Pax() { Id = 1, Name = "Nino", Surname = "rac" },
                Include = new Dictionary<string, List<string>>()
                {
                    { "accommodations", new List<string>(){ "name" } },
                    { "remarks", null },
                    { "cancellationpolicy", null},
                    { "mealplan", new List<string>(){ "name" } },
                    { "fees", null},
                    { "rooms", null},
                    { "price", null}

                },
                Remarks = "Test",
                Tolerance = new Tolerance()
                {
                    Type = ToleranceType.Percentage,
                    Value = 10
                }
            };
            bookRequest.Content = new StringContent(Serializar(bookQuery), Encoding.UTF8, "application/json");
            var bookResponse = await _client.SendAsync(bookRequest);
            var bookresponseString = await bookResponse.Content.ReadAsStringAsync();
            var booktrueObject = Deserializar<Booking>(bookresponseString);
            TestContext.Out.WriteLine(booktrueObject.BookingId);
            TestContext.Out.WriteLine(booktrueObject.HCN);
            TestContext.Out.WriteLine(booktrueObject.ClientReference);
        }

        [Test]
        public async Task MethodCancelBooking()
        {

            var cancelrequest = GetRequest("api/Booking/cancel", "BEDBDDDB5813A41E2B248329CDB4C884B23D0FF4F95C6AA10840B8B761B059F3", HttpMethod.Put);
            var resNo = "GOB07105409WDA4A9JW1P|Test638665772348560358"; //
            BookingCancelQuery cancelQuery = new BookingCancelQuery()
            {
                ExternalSupplier = new ExternalSupplier()
                {
                    Code = "IPaximum",
                    Connection = new System.Collections.Generic.Dictionary<string, string>()
                    {

                        { "Url","http://api.stage.paximum.com"},
                        { "ApiKey","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkYjkwZmJmZi03YWNkLTQyNDctYmZlNi01MTk4NzkwNWJlNzIiLCJyb2xlIjoiYjJjOmFwcCIsIm5iZiI6MTcyNDY1NjQwMSwiZXhwIjoxODgyNDIyNzYyLCJpYXQiOjE3MjQ2NTY0MDEsImlzcyI6Imh0dHBzOi8vYXV0aC5wYXhpbXVtLmNvbSIsImF1ZCI6Imh0dHBzOi8vYXBpLnBheGltdW0uY29tIn0.iBfdfn6wSUFsjC1lGcZNd9cYFRmAjktNx_cFiIx0Oak"},
                    }
                },
                BookingId = resNo,
                Include = new Dictionary<string, List<string>>()
                {
                    { "accommodations", new List<string>(){ "name" } },
                    { "remarks", null },
                    { "cancellationpolicy", null},
                    { "mealplan", new List<string>(){ "name" } },
                    { "fees", null},
                    { "rooms", null},
                    { "price", null},
                    { "cancellocator", null}

                },


            };
            cancelrequest.Content = new StringContent(Serializar(cancelQuery), Encoding.UTF8,
           "application/json");
            var cancelresponse = await _client.SendAsync(cancelrequest);
            var cancelresponseString = await cancelresponse.Content.ReadAsStringAsync();

            var canceltrueObject = Deserializar<Booking>(cancelresponseString);
            Assert.IsNotNull(canceltrueObject.CancelLocator);
        }

        [Test]
        public async Task MethodGetBooking()
        {
            var resNo = "d5217632-eb64-4983-bfae-d5943926f1c4"; //
            var clientReference = "Test638664114604602909";
            //8047080112
            var request = GetRequest("api/Booking/get", "BEDBDDDB5813A41E2B248329CDB4C884B23D0FF4F95C6AA10840B8B761B059F3", HttpMethod.Post);
            BookingsQuery query = new BookingsQuery()
            {

                ExternalSupplier = new ExternalSupplier()
                {
                    Code = "IPaximum",
                    Connection = new System.Collections.Generic.Dictionary<string, string>()
                    {

                        { "Url","http://api.stage.paximum.com"},
                        { "ApiKey","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkYjkwZmJmZi03YWNkLTQyNDctYmZlNi01MTk4NzkwNWJlNzIiLCJyb2xlIjoiYjJjOmFwcCIsIm5iZiI6MTcyNDY1NjQwMSwiZXhwIjoxODgyNDIyNzYyLCJpYXQiOjE3MjQ2NTY0MDEsImlzcyI6Imh0dHBzOi8vYXV0aC5wYXhpbXVtLmNvbSIsImF1ZCI6Imh0dHBzOi8vYXBpLnBheGltdW0uY29tIn0.iBfdfn6wSUFsjC1lGcZNd9cYFRmAjktNx_cFiIx0Oak"},
                    }
                },
                //ClientReference = clientReference,
                BookingId = resNo,

                Include = new Dictionary<string, List<string>>()
                {
                    { "accommodations", new List<string>(){ "name" } },
                    { "remarks", null },
                    { "cancellationpolicy", null},
                    { "mealplan", new List<string>(){ "name" } },
                    { "fees", null},
                    { "rooms", null},
                    { "price", null}

                },


            };
            request.Content = new StringContent(Serializar(query), Encoding.UTF8, "application/json");
            var bookResponse = await _client.SendAsync(request);
            var bookresponseString = await bookResponse.Content.ReadAsStringAsync();

            var booktrueObject = Deserializar<Bookings>(bookresponseString);
            Assert.IsNotNull(booktrueObject.BookingList);
        }



        private HttpRequestMessage GetRequest(string method, string apikey, HttpMethod Hmethod = null)
        {
            if (Hmethod == null)
            {
                Hmethod = HttpMethod.Post;
            }
            var request = new HttpRequestMessage(Hmethod, method);
            request.Headers.Add("x-api-key", apikey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return request;
        }

        public static string Serializar<T>(T objeto)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true // Formato legible con sangría (opcional)
            };
            return JsonSerializer.Serialize(objeto, opciones);
        }

        // Método para deserializar un JSON a un objeto
        public static T Deserializar<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

    }
}