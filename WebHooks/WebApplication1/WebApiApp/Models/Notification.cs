namespace WebApiApp.Models
{
    public abstract class Notification
    {
        public string Action { get; set; }
        public string UserAction { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}