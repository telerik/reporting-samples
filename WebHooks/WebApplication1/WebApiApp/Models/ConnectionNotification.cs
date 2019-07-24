namespace WebApiApp.Models
{
    public class ConnectionNotification : Notification
    {
        public string Description { get; set; }
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
    }
}