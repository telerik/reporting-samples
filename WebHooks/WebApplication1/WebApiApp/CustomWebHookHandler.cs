using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApiApp.Models;

namespace WebApiApp
{
    public class CustomWebHookHandler : WebHookHandler
    {
        private readonly string ReportNotificationDataFileName;
        private readonly string CategoryNotificationDataFileName;
        private readonly string DataAlertNotificationDataFileName;
        private readonly string DataConnectionNotificationDataFileName;
        private readonly string ScheduledTaskNotificationDataFileName;

        public CustomWebHookHandler()
        {
            this.Receiver = CustomWebHookReceiver.ReceiverName;

            this.ReportNotificationDataFileName = ResourceNames.ReportNotificationJsonDataFileName;
            this.CategoryNotificationDataFileName = ResourceNames.CategoryNotificationJsonDataFileName;
            this.DataAlertNotificationDataFileName = ResourceNames.DataAlertNotificationJsonDataFileName;
            this.DataConnectionNotificationDataFileName = ResourceNames.DataConnectionNotificationJsonDataFileName;
            this.ScheduledTaskNotificationDataFileName = ResourceNames.ScheduledTaskNotificationJsonDataFileName;
        }

        public override Task ExecuteAsync(string generator, WebHookHandlerContext context)
        {
            string webhooksFilePath;

            // Get data from WebHook
            CustomNotifications data = context.GetDataOrDefault<CustomNotifications>();

            // Get data from each notification in this WebHook
            foreach (IDictionary<string, object> notification in data.Notifications)
            {
                string action = notification["Action"] as string;
                switch (action)
                {
                    case WebHookFilters.Reports:
                        webhooksFilePath = this.ReportNotificationDataFileName;
                        break;
                    case WebHookFilters.Categories:
                        webhooksFilePath = this.CategoryNotificationDataFileName;
                        break;
                    case WebHookFilters.DataAlerts:
                        webhooksFilePath = this.DataAlertNotificationDataFileName;
                        break;
                    case WebHookFilters.DataConnections:
                        webhooksFilePath = this.DataConnectionNotificationDataFileName;
                        break;
                    case WebHookFilters.ScheduledTasks:
                        webhooksFilePath = this.ScheduledTaskNotificationDataFileName;
                        break;
                    default:
                        throw new System.Exception($"Action \"{action}\" is not supported by this webhook handler.");
                }

                this.RecreateJsonDataFile(webhooksFilePath, notification);
            }

            return Task.FromResult(true);
        }

        private void RecreateJsonDataFile(string webhooksFilePath, IDictionary<string, object> notification)
        {
            StringBuilder sb;
            if (File.Exists(webhooksFilePath))
            {
                //replace the closing bracket with comma to allow adding a new element to the collection
                sb = new StringBuilder(File.ReadAllText(webhooksFilePath).Trim());
                if (sb.Length > 0)
                {
                    sb.Replace(']', ',', sb.Length - 1, 1);
                }
            }
            else
            {
                sb = new StringBuilder("[");
            }

            using (TextWriter tw = new StringWriter(sb))
            {
                JsonSerializer serializer = new JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                };

                serializer.Serialize(tw, notification);
            }

            sb.Append("]");

            File.WriteAllText(webhooksFilePath, sb.ToString());
        }
    }
}