using System;
using System.Collections.Generic;

namespace WebApiApp.Models
{
    public class TaskNotification : Notification
    {
        public List<TaskReport> Reports { get; set; }
        public DateTime StartDate { get; set; }
        public string RecurrenceRule { get; set; }
        public bool Enabled { get; set; }
        public string CreatedByUserId { get; set; }
        public bool ExecuteImmediately { get; set; }
        public DateTime DateCreated { get; set; }
    }
}