using System;

namespace NLogCentral.Web.Models
{
    public class LogModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Level { get; set; }
        public string MachineName { get; set; }
        public string ProcessName { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
    }
}