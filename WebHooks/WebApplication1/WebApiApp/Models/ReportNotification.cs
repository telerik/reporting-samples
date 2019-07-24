namespace WebApiApp.Models
{
    public class ReportNotification : Notification
    {
        public string Description { get; set; }
        public string Extension { get; set; }
        public bool IsDraft { get; set; }
        public string CategoryId { get; set; }
        public string CreatedByUserId { get; set; }
        public string LockedByUserId { get; set; }
        public string Revision { get; set; }
    }
}