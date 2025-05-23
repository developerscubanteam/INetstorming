using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Connectivity.Queries.Base
{
    public class Pax
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Document { get; set; }
        public string? PostalCode { get; set; }
        public bool Holder { get; set; }
        public byte Age { get; set; }
    }
}
