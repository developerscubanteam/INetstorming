using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RequestLogs
{
    public enum AuditType
    {
        CreateBooking = 1,
        CancelBooking = 2,
        GetBooking = 3,
        Valuation = 4
    }

    public enum LogType
    {
        Information = 1,
        Error = 2
    }

    public class RequestLog
    {
        public required string TrackId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public AuditType Operation { get; set; }
        public string? Customer { get; set; }
        public string? ExternalSupplier { get; set; }
        public LogType Status { get; set; }
        public string? RQ { get; set; }
        public string? RS { get; set; }
        public string? FileName { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
