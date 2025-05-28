using Domain.Common;
using Domain.Error;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;
using Infrastructure.Connectivity.Connector.Models.Message.Common;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries.Base;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using Message = Infrastructure.Connectivity.Connector.Models.Message;

namespace Infrastructure.Connectivities.Iboosy.Connector.HttpWrapper
{
    public class UrlPath
    {
        //TODO: Change the path to the correct one
        //public const string Shopping = "shopping/multihotels";
        public const string LiveCheck = "RQtP.Availability";
        public const string PreBook = "RQtP.Valuation";
        public const string Booking = "RQtP.CreateBooking";
        public const string CancelBooking = "RQtP.CancelBooking";
        public const string GetBookings = "RQtP.GetBooking";
    }
    public class HttpWrapper : IHttpWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpWrapper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(Message.AvailabilityRS.AvailabilityRS? AvailabilityRs, List<Domain.Error.Error>? Errors, AuditData AuditData)> Availability(ConnectionData connectionData, bool auditRequests, int? timeout, object query)
        {
            const string c_Method = UrlPath.LiveCheck;
            var timeoutRq = timeout.HasValue ? timeout.Value : 20000;
            var processTime = Stopwatch.StartNew();
            var auditData = new AuditData() { Requests = [] };
            var auditRequest = new Request() { RequestName = c_Method, TimeStamp = DateTime.UtcNow };
            var availRQ = (query as AvailabilityRQ);
            var rqString = SerializeExtension.SerializeObjectToXmlString<NetStormingAvailabilityRQ>(availRQ.rq);
            var responseString = "";

            try
            {
                if (auditRequests)
                    auditData.Requests.Add(new Request() { RQ = rqString });

                var client = _httpClientFactory.CreateClient(ServiceConf.ClientName);
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connectionData.Token);
                client.Timeout = TimeSpan.FromMilliseconds(timeout.GetValueOrDefault());
                var separator = connectionData.Url.EndsWith("/") ? "" : "/";
                var uri = new Uri(connectionData.Url + separator + UrlPath.LiveCheck);

                var message = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(rqString, Encoding.UTF8, "application/XML")
                };

                var responseMessage = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                responseString = await responseMessage.Content.ReadAsStringAsync();


                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var resultString = responseString;
                    var response = resultString.DeserializateObjectToXmlString<NetStormingAvailabilityRS>();

                    if (response.response.type == "availability")
                    {
                        // Si el resultado no trae error, pero no viene vacío (no tiene hotele) se debe devolver error "NO Results"
                        var error = IsEmptyResult(response); //implement the logic to check if the response is empty
                        if (error != null)
                        {
                            auditRequest.Type = AuditDataType.NoResults;
                            return (null, error, auditData);
                        }

                        auditRequest.Type = AuditDataType.Ok;
                        return (new AvailabilityRS { rs = response }, null, auditData);
                    }
                    else
                    {
                        auditRequest.Type = AuditDataType.KO;
                        var error = new Message.Common.Error
                        {
                            Code = "500",
                            Message = response.response.Value
                        };
                        return (null, SupplierError(error), auditData);
                    }
                }
                else
                {
                    //TODO: Implement the error handling
                    var error = new Message.Common.Error()
                    {
                        Code = ((int)responseMessage.StatusCode).ToString(),
                        Message = await responseMessage.Content.ReadAsStringAsync()
                    };
                    auditRequest.Type = AuditDataType.KO;
                    return (null, SupplierError(error), auditData);
                }
            }
            catch (TaskCanceledException)
            {
                auditRequest.Type = AuditDataType.Timeout;
                return (null, TimeoutError(), auditData);
            }
            catch (Exception ex)
            {
                auditRequest.Type = AuditDataType.KO;
                return (null, UncontrolledError(ex), auditData);
            }
            finally
            {
                auditData.NumberOfRequests = 1;
                if (auditRequests)
                {
                    auditRequest.ProcessTime = processTime.ElapsedMilliseconds;
                    auditRequest.RQ = rqString;
                    auditRequest.RS = responseString;
                }
                auditData.Requests.Add(auditRequest);
            }
        }

        public async Task<(ValuationRS? ValuationRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Valuation(ConnectionData connectionData, int? timeout, object query)
        {
            var timeoutRq = timeout.HasValue ? timeout.Value : ServiceConf.TimeoutValuation;
            var c_Method = UrlPath.PreBook;
            var processTime = Stopwatch.StartNew();

            var valuationRq = (Message.ValuationRQ.ValuationRQ)query;
            var auditData = new AuditData() { Requests = [] };
            var auditRequest = new Request() { RequestName = c_Method, TimeStamp = DateTime.UtcNow };
            var rqString = SerializeExtension.SerializeObjectToXmlString<NetStormingAvailabilityRQ>(valuationRq.rq);
            var responseString = "";
            var response = new ValuationRS();

            try
            {
                var client = _httpClientFactory.CreateClient("Av");
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connectionData.Token);
                client.Timeout = TimeSpan.FromMilliseconds(timeoutRq);

                var separator = connectionData.Url.EndsWith("/") ? "" : "/";
                var url = connectionData.Url + separator + UrlPath.PreBook;
                var uri = new Uri(url);
                client.BaseAddress = uri;

                var message = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(rqString, Encoding.UTF8, "application/XML")
                };

                var responseMessage = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                responseString = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var resultLiveCheckString = responseString;
                    response.rs = resultLiveCheckString.DeserializateObjectToXmlString<NetStormingAvailabilityRS>();

                    if (response.rs.response.type == "availability")
                    {
                        if (response.rs.response.hotels != null && response.rs.response.hotels.hotel != null && response.rs.response.hotels.hotel.Any() && response.rs.response.hotels.hotel[0].agreement != null)
                        {
                            var agreement = valuationRq.rq.query.search.agreement;
                            var agreementRate = response.rs.response.hotels?.hotel[0].agreement.Where(x => x.id == agreement).ToArray();
                            response.rs.response.hotels.hotel[0].agreement = agreementRate;

                            //Si no viene el tag <evaluate> o si no se reciben las políticas estructuradas (Deadline y Policies), se debe llamar al GetDeadline
                            if (response.rs.response.evaluate == null
                                || (response.rs.response.evaluate.result.code != "not_available" &&
                                   (response.rs.response.hotels?.hotel[0].agreement[0].policies == null || response.rs.response.hotels?.hotel[0].agreement[0].policies.Length == 0)
                                   && response.rs.response.hotels?.hotel[0].agreement[0].deadline == null)
                               )
                            {
                                var deadlineQuery = GetDeadLineQuery(connectionData, response.rs.response.hotels.hotel[0].agreement[0].id,
                                                                    response.rs.response.search.number,
                                                                    response.rs.response.hotels.hotel[0].code);
                                var getDeadLineResult = await this.GetDeadLine(connectionData, deadlineQuery, auditData);

                                if (getDeadLineResult.Errors != null)
                                {
                                    auditRequest.Type = AuditDataType.KO;
                                    return (null, getDeadLineResult.Errors, auditData);
                                }
                                //En este caso actualizo las politicas y remarks del evaluate con las que se obtienen del GetDeadLine
                                response.rs.response.hotels.hotel[0].agreement[0].deadline = getDeadLineResult.GetDeadlineRS.response.deadline;
                                response.rs.response.hotels.hotel[0].agreement[0].policies = getDeadLineResult.GetDeadlineRS.response.policies;
                                response.rs.response.hotels.hotel[0].agreement[0].remarks = getDeadLineResult.GetDeadlineRS.response.remarks;

                            }

                            // Si viene el tag <evaluate>, se debe verificar que el estado de la tarifa sea "blocked".
                            // Si no viene el tag <evaluate> es porque el proveedor no permite prebook, se debe seguir con la reserva
                            // Se debe validar que el estado de la tarifa es "available" porque según la doc, puede pasar que una que estaba "available" en el search, luego cambie a onrequest en el evaluate
                            // Aunque en esta último caso "option_blocked" debería ser "false", pero  por las dudas...
                            if ((response.rs.response.evaluate == null || (response.rs.response.evaluate != null && response.rs.response.evaluate.result.option_blocked == true))
                                    && response.rs.response.hotels.hotel[0].agreement[0].available)
                            {
                                auditRequest.Type = AuditDataType.Ok;
                                return (response, null, auditData);
                            }
                            else
                            {
                                var error = new Message.Common.Error
                                {
                                    Code = "500",
                                    Message = "The selected rate is no longer available"
                                };
                                auditRequest.Type = AuditDataType.KO;
                                return (null, SupplierError(error), auditData);
                            }

                        }
                        else
                        {
                            auditRequest.Type = AuditDataType.KO;
                            var error = new Message.Common.Error
                            {
                                Code = "500",
                                Message = "The agreement is no longer available"
                            };
                            return (null, SupplierError(error), auditData);

                        }
                    }
                    else
                    {
                        var error = new Message.Common.Error
                        {
                            Code = "500",
                            Message = response.rs.response.Value
                        };
                        auditRequest.Type = AuditDataType.KO;
                        return (null, SupplierError(error), auditData);

                    }
                }
                else
                {
                    //TODO: Implement the error handling
                    var error = new Message.Common.Error()
                    {
                        Code = ((int)responseMessage.StatusCode).ToString(),
                        Message = responseString
                    };
                    auditRequest.Type = AuditDataType.KO;
                    return (null, SupplierError(error), auditData);
                }
            }
            catch (TaskCanceledException)
            {
                auditRequest.Type = AuditDataType.Timeout;
                return (null, TimeoutError(), auditData);
            }
            catch (Exception ex)
            {
                auditRequest.Type = AuditDataType.KO;
                return (null, UncontrolledError(ex), auditData);
            }
            finally
            {
                auditData.NumberOfRequests = 1;
                auditRequest.ProcessTime = processTime.ElapsedMilliseconds;
                auditRequest.RQ = rqString;
                auditRequest.RS = responseString;

                auditData.Requests.Add(auditRequest);
            }
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Booking(ConnectionData connectionData, object query)
        {
            const string c_Method = UrlPath.Booking;
            var rqString = JsonSerializer.Serialize(query);
            var auditData = new AuditData() { Requests = [] };
            var auditRequest = new Request() { RequestName = c_Method, TimeStamp = DateTime.UtcNow };
            var responseString = "";
            var processTime = Stopwatch.StartNew();

            try
            {
                var client = _httpClientFactory.CreateClient("Av");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connectionData.Token);
                client.Timeout = TimeSpan.FromMilliseconds(ServiceConf.TimeoutBooking);

                var separator = connectionData.Url.EndsWith("/") ? "" : "/";
                var url = connectionData.Url + separator + UrlPath.Booking;
                var uri = new Uri(url);
                client.BaseAddress = uri;

                var message = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(rqString, Encoding.UTF8, "application/json")
                };

                var responseMessage = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                responseString = await responseMessage.Content.ReadAsStringAsync();


                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var response = JsonSerializer.Deserialize<Message.BookingRS.BookingRS>(responseString, SerializeExtension.Configure());
                    auditRequest.Type = AuditDataType.Ok;
                    return (response, null, auditData);
                }
                else
                {
                    //TODO: Implement the error handling
                    var error = new Message.Common.Error() { };
                    auditRequest.Type = AuditDataType.KO;
                    return (null, SupplierError(error), auditData);
                }
            }
            catch (TaskCanceledException)
            {
                auditRequest.Type = AuditDataType.Timeout;
                return (null, TimeoutError(), auditData);
            }
            catch (Exception ex)
            {
                auditRequest.Type = AuditDataType.KO;
                return (null, UncontrolledError(ex), auditData);
            }
            finally
            {
                auditData.NumberOfRequests = 1;
                auditRequest.ProcessTime = processTime.ElapsedMilliseconds;
                auditRequest.RQ = rqString;
                auditRequest.RS = responseString;

                auditData.Requests.Add(auditRequest);
            }
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CancelBooking(ConnectionData connectionData, object query)
        {
            const string c_Method = UrlPath.CancelBooking;
            var processTime = Stopwatch.StartNew();
            var rqString = JsonSerializer.Serialize(query);
            var auditData = new AuditData() { Requests = [] };
            var auditRequest = new Request() { RequestName = c_Method, TimeStamp = DateTime.UtcNow };
            var responseString = "";

            try
            {
                var client = _httpClientFactory.CreateClient("Av");

                client.Timeout = TimeSpan.FromMilliseconds(ServiceConf.TimeoutBooking);

                var separator = connectionData.Url.EndsWith("/") ? "" : "/";
                var url = connectionData.Url + separator + UrlPath.CancelBooking;
                var uri = new Uri(url);
                client.BaseAddress = uri;

                var message = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(rqString, Encoding.UTF8, "application/json")
                };

                var responseMessage = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                responseString = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var response = JsonSerializer.Deserialize<Message.BookingRS.BookingRS>(responseString, SerializeExtension.Configure());
                    auditRequest.Type = AuditDataType.Ok;
                    return (response, null, auditData);
                }
                else
                {
                    //TODO: Implement the error handling
                    var error = new Message.Common.Error() { };
                    auditRequest.Type = AuditDataType.KO;
                    return (null, SupplierError(error), auditData);
                }
            }
            catch (TaskCanceledException)
            {
                auditRequest.Type = AuditDataType.Timeout;
                return (null, TimeoutError(), auditData);
            }
            catch (Exception ex)
            {
                auditRequest.Type = AuditDataType.KO;
                return (null, UncontrolledError(ex), auditData);
            }
            finally
            {
                auditData.NumberOfRequests = 1;
                auditRequest.ProcessTime = processTime.ElapsedMilliseconds;
                auditRequest.RQ = rqString;
                auditRequest.RS = responseString;

                auditData.Requests.Add(auditRequest);
            }
        }

        public async Task<(Message.BookingRS.BookingRS? GetBookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> GetBookings(ConnectionData connectionData, object query)
        {
            const string c_Method = UrlPath.GetBookings;
            var rqString = JsonSerializer.Serialize(query);
            var auditData = new AuditData() { Requests = [] };
            var auditRequest = new Request() { RequestName = c_Method, TimeStamp = DateTime.UtcNow };
            var processTime = Stopwatch.StartNew();
            var responseString = "";

            try
            {
                var client = _httpClientFactory.CreateClient("Av");
                client.Timeout = TimeSpan.FromMilliseconds(ServiceConf.TimeoutBooking);

                var separator = connectionData.Url.EndsWith("/") ? "" : "/";
                var url = connectionData.Url + separator + UrlPath.GetBookings;
                var uri = new Uri(url);
                client.BaseAddress = uri;

                var message = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(rqString, Encoding.UTF8, "application/json")
                };

                var responseMessage = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                responseString = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {

                    var response = JsonSerializer.Deserialize<Message.BookingRS.BookingRS>(responseString, SerializeExtension.Configure());
                    auditRequest.Type = AuditDataType.Ok;
                    return (response, null, auditData);
                }
                else
                {
                    //TODO: Implement the error handling
                    var error = new Message.Common.Error() { };
                    auditRequest.Type = AuditDataType.KO;
                    return (null, SupplierError(error), auditData);
                }
            }
            catch (TaskCanceledException)
            {
                auditRequest.Type = AuditDataType.Timeout;
                return (null, TimeoutError(), auditData);
            }
            catch (Exception ex)
            {
                auditRequest.Type = AuditDataType.KO;
                return (null, UncontrolledError(ex), auditData);
            }
            finally
            {
                auditData.NumberOfRequests = 1;
                auditRequest.ProcessTime = processTime.ElapsedMilliseconds;
                auditRequest.RQ = rqString;
                auditRequest.RS = responseString;

                auditData.Requests.Add(auditRequest);
            }
        }


        private List<Domain.Error.Error> TimeoutError()
        {
            var errors = new List<Domain.Error.Error>
            {
                new Domain.Error.Error(((int)HttpStatusCode.RequestTimeout).ToString(), HttpStatusCode.RequestTimeout.ToString(), ErrorType.Timeout, CategoryErrorType.Provider)
            };
            return errors;
        }
        private List<Domain.Error.Error> TooManyRequests()
        {
            var errors = new List<Domain.Error.Error>
            {
                new Domain.Error.Error(((int)HttpStatusCode.RequestTimeout).ToString(), HttpStatusCode.RequestTimeout.ToString(), ErrorType.Timeout, CategoryErrorType.Provider)
            };
            return errors;
        }

        private List<Domain.Error.Error> UncontrolledError(Exception ex)
        {
            var errors = new List<Domain.Error.Error>
            {
                  new Domain.Error.Error(((int)HttpStatusCode.InternalServerError).ToString(), ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Hub)
            };
            return errors;
        }

        private List<Domain.Error.Error> SupplierError(Message.Common.Error errorRS)
        {
            //TODO: Implement the error handling
            var errors = new List<Domain.Error.Error>
            {
                new Domain.Error.Error(errorRS.Code, errorRS.Message, ErrorType.Error, CategoryErrorType.Provider)
            };
            return errors;
        }

        private List<Domain.Error.Error>? IsEmptyResult(NetStormingAvailabilityRS? response)
        {
            if (response?.response?.hotels != null && response.response.hotels.hotel.Any()) //Logic to check if the response is empty                
            {
                return null;
            }
            return [new Domain.Error.Error("NO_AVAIL_FOUND", "No availability found", ErrorType.NoResults, CategoryErrorType.Provider)];
        }

        public NetstormingGetDeadLineRQ GetDeadLineQuery(ConnectionData connectionData, string agreementCode, string availabilityId, uint hotelId)
        {
            var deadLineQuery = new NetstormingGetDeadLineRQ()
            {
                header = new RequestEnvelopeHeader()
                {
                    actor = connectionData.Actor,
                    user = connectionData.User,
                    password = connectionData.Password,
                    version = ServiceConf.ApiVersion,
                    timestamp = DateTimeExtension.GetTimeStamp()
                },
                query = new GetDeadlineEnvelopeQuery()
                {
                    product = "hotel",
                    type = "get_deadline",
                    agreement = new GetDeadLineAgreement()
                    {
                        code = agreementCode
                    },
                    availability = new DeadlineAvailability
                    {
                        id = availabilityId
                    },
                    hotel = new envelopeQueryHotel()
                    {
                        id = hotelId
                    }
                }
            };

            return deadLineQuery;
        }

        private async Task<(NetstormingGetDeadLineRS? GetDeadlineRS, List<Domain.Error.Error>? Errors)> GetDeadLine(ConnectionData connectionData, NetstormingGetDeadLineRQ deadLineQuery, AuditData auditData)
        {
            const string c_Method = "GetDeadLine";
            var processTime = Stopwatch.StartNew();
            var auditRequest = new Request() { RequestName = c_Method, TimeStamp = DateTime.UtcNow };

            string rqString = System.SerializeExtension.SerializeObjectToXmlString<NetstormingGetDeadLineRQ>(deadLineQuery);

            HttpClient client = _httpClientFactory.CreateClient(ServiceConf.Name);

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, connectionData.Url)
            {
                Content = new StringContent(rqString, Encoding.UTF8, "application/XML")
            };
            var responseString = "";
            try
            {
                NetstormingGetDeadLineRS result = null;
                using (HttpResponseMessage response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead))
                {
                    var strResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        responseString = strResponse;
                        result = strResponse.DeserializeXml<NetstormingGetDeadLineRS>();

                        if (result.response.type == "get_deadline")
                        {
                            auditRequest.Type = AuditDataType.Ok;
                            return (result, null);
                        }
                        else // Devolvió error
                        {
                            var error = new Message.Common.Error
                            {
                                Code = "500",
                                Message = strResponse
                            };

                            auditRequest.Type = AuditDataType.KO;
                            return (null, SupplierError(error));
                        }
                    }
                    else // Error HTTP
                    {
                        var error = new Message.Common.Error
                        {
                            Code = ((int)response.StatusCode).ToString(),
                            Message = strResponse
                        };

                        auditRequest.Type = AuditDataType.KO;
                        return (null, SupplierError(error));
                    }
                }
            }
            catch (TaskCanceledException)
            {
                auditRequest.Type = AuditDataType.Timeout;
                return (null, TimeoutError());
            }
            catch (Exception ex)
            {
                auditRequest.Type = AuditDataType.KO;
                return (null, UncontrolledError(ex));
            }
            finally
            {
                auditData.NumberOfRequests = 1;
                auditRequest.ProcessTime = processTime.ElapsedMilliseconds;
                auditRequest.RQ = rqString;
                auditRequest.RS = responseString;

                auditData.Requests.Add(auditRequest);
            }
        }

    }
}
