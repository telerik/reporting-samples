using Newtonsoft.Json;
using System.Collections;
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
            return Deserialize<List<ReportNotification>>(ResourceNames.ReportNotificationJsonDataFileName);
        }

        // GET api/notifications/category
        [HttpGet]
        [ActionName("category")]
        public IEnumerable<Notification> GetCategoryNotification()
        {
            return Deserialize<List<CategoryNotification>>(ResourceNames.CategoryNotificationJsonDataFileName);
        }

        // GET api/notifications/connection
        [HttpGet]
        [ActionName("connection")]
        public IEnumerable<Notification> GetDataConnectionNotification()
        {
            return Deserialize<List<ConnectionNotification>>(ResourceNames.DataConnectionNotificationJsonDataFileName);
        }

        // GET api/notifications/task
        [HttpGet]
        [ActionName("task")]
        public IEnumerable<Notification> GetScheduledTaskNotification()
        {
            return Deserialize<List<TaskNotification>>(ResourceNames.ScheduledTaskNotificationJsonDataFileName);
        }

        // GET api/notifications/alert
        [HttpGet]
        [ActionName("alert")]
        public IEnumerable<Notification> GetDataAlertNotification()
        {
            return Deserialize<List<TaskNotification>>(ResourceNames.DataAlertNotificationJsonDataFileName);
        }

        static T Deserialize<T>(string fileName) where T: ICollection
        {
            return File.Exists(fileName) 
                ? JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName)) 
                : default(T);
        }
    }
}
