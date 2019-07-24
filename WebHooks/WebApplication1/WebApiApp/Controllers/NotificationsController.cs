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
            List<ReportNotification> reportNotification = new List<ReportNotification>();

            string path = ResourceNames.ReportNotificationJsonDataFileName;
            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonData = streamReader.ReadToEnd();
                reportNotification = JsonConvert.DeserializeObject<List<ReportNotification>>(jsonData);
            }

            return reportNotification;
        }

        // GET api/notifications/category
        [HttpGet]
        [ActionName("category")]
        public IEnumerable<Notification> GetCategoryNotification()
        {
            List<CategoryNotification> categoryNotification = new List<CategoryNotification>();

            string path = ResourceNames.CategoryNotificationJsonDataFileName;
            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonData = streamReader.ReadToEnd();
                categoryNotification = JsonConvert.DeserializeObject<List<CategoryNotification>>(jsonData);
            }

            return categoryNotification;
        }

        // GET api/notifications/connection
        [HttpGet]
        [ActionName("connection")]
        public IEnumerable<Notification> GetDataConnectionNotification()
        {
            List<ConnectionNotification> connectionNotification = new List<ConnectionNotification>();

            string path = ResourceNames.DataConnectionNotificationJsonDataFileName;
            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonData = streamReader.ReadToEnd();
                connectionNotification = JsonConvert.DeserializeObject<List<ConnectionNotification>>(jsonData);
            }

            return connectionNotification;
        }

        // GET api/notifications/task
        [HttpGet]
        [ActionName("task")]
        public IEnumerable<Notification> GetScheduledTaskNotification()
        {
            List<TaskNotification> taskNotification = new List<TaskNotification>();

            string path = ResourceNames.ScheduledTaskNotificationJsonDataFileName;
            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonData = streamReader.ReadToEnd();
                taskNotification = JsonConvert.DeserializeObject<List<TaskNotification>>(jsonData);
            }

            return taskNotification;
        }

        // GET api/notifications/alert
        [HttpGet]
        [ActionName("alert")]
        public IEnumerable<Notification> GetDataAlertNotification()
        {
            List<TaskNotification> alertNotification = new List<TaskNotification>();

            string path = ResourceNames.DataAlertNotificationJsonDataFileName;
            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonData = streamReader.ReadToEnd();
                alertNotification = JsonConvert.DeserializeObject<List<TaskNotification>>(jsonData);
            }

            return alertNotification;
        }
    }
}
