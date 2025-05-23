using Domain.Common.CancellationPolicy;
using Domain.Common.Price;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.ErrorRS;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRQ;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Queries.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Infrastructure.Connectivity.Connector.Extension
{
    public static partial class Extension
    {
        public static string SerializeObjectXml<T>(this T toSerialize)
        {

            XmlSerializer ser = new XmlSerializer(toSerialize.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlTextWriter tw = new XmlTextWriter(ms, Encoding.UTF8))
                {
                    tw.Formatting = System.Xml.Formatting.Indented;
                    ser.Serialize(tw, toSerialize);
                    // Now we get the serialized data as a string in the desired encoding
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        public static string ExtractXmlValue(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                return null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(reader.Value))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }

                        if (reader.NodeType == XmlNodeType.CDATA)
                        {
                            return reader.Value;
                        }
                        else if (reader.NodeType == XmlNodeType.Text)
                        {
                            return reader.Value;
                        }
                    }
                    return null;
                }
                else
                {
                    return reader.Value;
                }
            }
        }

        public static string ExtractXmlValueDecimalDome(XmlReader reader)
        {
            string value = null;
            if (reader.IsEmptyElement)
            {
                return value;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(reader.Value))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }

                        if (reader.NodeType == XmlNodeType.CDATA)
                        {
                            value = reader.Value;
                        }
                        else if (reader.NodeType == XmlNodeType.Text)
                        {
                            value = reader.Value;
                        }
                    }

                }
                else
                {
                    value = reader.Value;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Replace(".", ",");
                }
                return value;
            }
        }

        public static JsonSerializerOptions Configure()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new DateTimeOffsetConverterUsingDateTimeParse());
            return options;
        }


        //public static ErrorRS GetErrorRS(this HttpStatusCode data)
        //{
        //    var result = new ErrorRS()
        //    {
        //        Error = new Error()
        //        {
        //            Code = data.ToString(),
        //            Message = $"Bad HTTP status code:{data}"
        //        },
        //    };
        //    return result;
        //}

        //public static ErrorRS GetErrorRS(this ErrorRS data)
        //{
        //    var result = new ErrorRS()
        //    {
        //        Error = new Error()
        //        {
        //            Code = "500",
        //            Message = $"Empty reponse."
        //        },
        //    };
        //    return result;
        //}

        
        public static CancellationPolicy ProcessCancelPolice(List<Tuple<int, DateTime, decimal>> otherspolices , string currency, int roomCount)
        {
            var result = new CancellationPolicy()
            {
                PenaltyRules = new List<PenaltyRule>()
            };
            //procesamos las police que no son noshow las que este presentes en todas la rooms  
            var samepolice = otherspolices.Distinct().GroupBy(x => new { fecha = x.Item2.Date })
                          .Select(i => new { fecha = i.Key.fecha.Date, amount = i.Sum(y => y.Item3), cant = i.Count() })
                          .Where(y => y.cant == roomCount).ToList();

            if (samepolice != null)
            {
                samepolice.ForEach(i =>
                {
                    result.PenaltyRules.Add(new PenaltyRule()
                    {
                        Type= PolicyRule.AmountRule,
                        CurrencyCode = currency,
                        DateFrom = i.fecha,
                        Price = new Domain.Common.Price.Price()
                        {
                            Purchase = new Purchase()
                            {
                                Gross = i.amount,
                                Net = i.amount,
                                CostType = PurchaseCostType.Nett,
                                CurrencyCode = currency,
                                Cost = new Cost()
                                {
                                    Amount = i.amount,
                                    Price = i.amount,
                                    Quantity = 1
                                }
                            }
                        }
                    });
                });
            }
            if (!otherspolices.TrueForAll(j => samepolice.Exists(i => i.fecha == j.Item2.Date)))
            {
                //procesamos cronológicamente las demás si existen
                var activePolices = new List<decimal>(roomCount);
                for (int i = 0; i < roomCount; i++)
                {
                    activePolices.Add(0);
                }

                var orderedRules = otherspolices.OrderBy(i => i.Item2).ToList();

                // Formamos el Amount

                orderedRules.ForEach(i =>
                {
                    var amount = i.Item3;
                    //sumamos las activas menos la que estoy analizando
                    var totalamount = amount + (activePolices.Sum() - activePolices[i.Item1 - 1]);
                    //cargamos la nueva regla activa
                    activePolices[i.Item1 - 1] = amount;
                    if (!samepolice.Exists(j => j.fecha == i.Item2.Date))
                    {
                        result.PenaltyRules.Add(new PenaltyRule()
                        {
                            Type= PolicyRule.AmountRule,
                            CurrencyCode = currency,
                            DateFrom = i.Item2.Date,
                            Price = new Domain.Common.Price.Price()
                            {
                                Purchase = new Purchase()
                                {
                                    Gross = totalamount,
                                    Net = totalamount,
                                    CostType = PurchaseCostType.Nett,
                                    CurrencyCode = currency,
                                    Cost = new Cost()
                                    {
                                        Amount = totalamount,
                                        Price = totalamount,
                                        Quantity = 1
                                    }
                                }
                            }
                        });
                    }
                });
            }
            return result;
        }

        public static AdditionContainer<T> ToSum<T>(this T data, Func<T, T, T> _sumFunction) where T : class
        {
            return new AdditionContainer<T>(data, _sumFunction);
        }
    }

    public class DateTimeOffsetConverterUsingDateTimeParse : System.Text.Json.Serialization.JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTimeOffset));
            return DateTimeOffset.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }



    public class LongConverter : System.Text.Json.Serialization.JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(long));
            return long.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class AdditionContainer<T>
     where T : class
    {
        public T data;
        public Func<T, T, T> _sumFunction;
        public AdditionContainer(T pdata, Func<T, T, T> sumFunction)
        {
            data = pdata;
            _sumFunction = sumFunction;
        }

        public static T operator +(AdditionContainer<T> a, AdditionContainer<T> b)
        {
            return a._sumFunction(a.data, b.data);
        }
    }

}
