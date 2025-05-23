using Domain.Error;

namespace PTGWJuniper.Models
{
    public class ExceptionResponse
    {
        public ExceptionResponse()
        {
            TrackId = Guid.NewGuid().ToString();
            Errors = new List<Error>();
        }
        public string TrackId { get; set; }
        public List<Error> Errors { get; set; }
    }
}
