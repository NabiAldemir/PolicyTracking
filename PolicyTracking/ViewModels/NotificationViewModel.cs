namespace PolicyTracking.ViewModels
{
    public class NotificationViewModel
    {
        public int PolicyId { get; set; }
        public string? CompanyName { get; set; }
        public string PolicyType { get; set; } 
        public string? CustomerName { get; set; }
        public string? CustomerSurname { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysLeft { get; set; }
    }
}
