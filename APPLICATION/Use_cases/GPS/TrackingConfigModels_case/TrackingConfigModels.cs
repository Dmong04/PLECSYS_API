namespace APPLICATION.Use_cases.GPS.TrackingConfigModels_case
{
    public class TrackingConfigModels
    {
        public class GetTrackingConfigRequest
        {
            public string SellerId { get; set; } = string.Empty;
        }

        public class UpdateTrackingConfigRequest
        {
            public string SellerId { get; set; } = string.Empty;
            public int IntervalMinutes { get; set; }
        }

        public class TrackingConfigResponse
        {
            public string SellerId { get; set; } = string.Empty;
            public int IntervalMinutes { get; set; }
        }
    }
}
