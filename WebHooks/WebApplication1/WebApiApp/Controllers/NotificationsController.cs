using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using WebApiApp.Models;

namespace WebApiApp.Controllers
{
    public class NotificationsController : ApiController
    {
        // GET api/notifications/report
        [HttpGet]
        [ActionName("report")]
        public IEnumerable<Notification> GetReportNotification()
        {
            string path = ResourceNames.ReportNotificationJsonDataFileName;

            return this.GetBaseNotification<ReportNotification>(path);
        }

        // GET api/notifications/category
        [HttpGet]
        [ActionName("category")]
        public IEnumerable<Notification> GetCategoryNotification()
        {
            string path = ResourceNames.CategoryNotificationJsonDataFileName;

            return this.GetBaseNotification<CategoryNotification>(path);
        }

        // GET api/notifications/connection
        [HttpGet]
        [ActionName("connection")]
        public IEnumerable<Notification> GetDataConnectionNotification()
        {
            string path = ResourceNames.DataConnectionNotificationJsonDataFileName;

            return this.GetBaseNotification<ConnectionNotification>(path);
        }

        // GET api/notifications/task
        [HttpGet]
        [ActionName("task")]
        public IEnumerable<Notification> GetScheduledTaskNotification()
        {
            string path = ResourceNames.ScheduledTaskNotificationJsonDataFileName;

            return this.GetBaseNotification<TaskNotification>(path);
        }

        // GET api/notifications/alert
        [HttpGet]
        [ActionName("alert")]
        public IEnumerable<Notification> GetDataAlertNotification()
        {
            string path = ResourceNames.DataAlertNotificationJsonDataFileName;

            return this.GetBaseNotification<TaskNotification>(path);
        }

        private IEnumerable<T> GetBaseNotification<T>(string path)
        {
            List<T> notification = new List<T>();

            ResourceNames.CreateFileIfDoesNotExist(path);
            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonData = streamReader.ReadToEnd();
                notification = JsonConvert.DeserializeObject<List<T>>(jsonData);
            }

            return notification;
        }
    }
}
