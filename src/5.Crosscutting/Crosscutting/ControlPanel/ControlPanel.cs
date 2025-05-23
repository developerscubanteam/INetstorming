namespace Crosscutting.ControlPanel
{
    public enum HealthStatus
    {
        Ok = 1,
        Down = 2
    }

    public static class ControlPanel
    {
        public static HealthStatus Status { get; set; }
        public static string? Reason { get; set; }
        public static int AccommodationMappingsCount { get; set; }
        public static int MealPlanMappingsCount { get; set; }
        public static int RoomTypeMappingsCount { get; set; }        
        public static int AvailabilityConsecutiveErrors { get; set; }
        public static int BookingConsecutiveErrors { get; set; }
        public static bool DatabaseOk { get; set; }
        public static int QueueLength { get; set; }
        public static HealthStatus CheckStatus()
        {
            if (AvailabilityConsecutiveErrors >= 1000)
            {
                Status = HealthStatus.Down;
                Reason = "Max consecutive errors in availability";             
            }

            if (BookingConsecutiveErrors >= 5)
            {
                Status = HealthStatus.Down;
                Reason = "Max booking errors in availability";                
            }

            if (DatabaseOk == false)             
                Status = HealthStatus.Down;

            return Status;
        }
    }


}
