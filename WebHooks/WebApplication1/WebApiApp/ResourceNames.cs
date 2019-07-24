using System.IO;
using System.Web;

namespace WebApiApp
{
    internal static class ResourceNames
    {
        private static readonly string appPath = HttpContext.Current.Server.MapPath("~/");
        internal static string AppPath
        {
            get
            {
                return appPath;
            }
        }

        private static readonly string reportsPath = Path.Combine(AppPath, "Reports");
        internal static string ReportsPath
        {
            get
            {
                return reportsPath;
            }
        }

        private static readonly string jsonPath = Path.Combine(ReportsPath, "JsonData");
        internal static string JsonPath
        {
            get
            {
                return jsonPath;
            }
        }

        private static readonly string reportNotificationJsonDataFileName = Path.Combine(JsonPath, "ReportNotification.json");
        internal static string ReportNotificationJsonDataFileName
        {
            get
            {
                return reportNotificationJsonDataFileName;
            }
        }

        private static readonly string categoryNotificationJsonDataFileName = Path.Combine(JsonPath, "CategoryNotification.json");
        public static string CategoryNotificationJsonDataFileName
        {
            get
            {
                return categoryNotificationJsonDataFileName;
            }
        }

        private static readonly string dataAlertNotificationJsonDataFileName = Path.Combine(JsonPath, "DataAlertNotification.json");
        public static string DataAlertNotificationJsonDataFileName
        {
            get
            {
                return dataAlertNotificationJsonDataFileName;
            }
        }

        private static readonly string dataConnectionNotificationJsonDataFileName = Path.Combine(JsonPath, "DataConnectionNotification.json");
        public static string DataConnectionNotificationJsonDataFileName
        {
            get
            {
                return dataConnectionNotificationJsonDataFileName;
            }
        }

        private static readonly string scheduledTaskNotificationJsonDataFileName = Path.Combine(JsonPath, "ScheduledTaskNotification.json");
        public static string ScheduledTaskNotificationJsonDataFileName
        {
            get
            {
                return scheduledTaskNotificationJsonDataFileName;
            }
        }
    }
}