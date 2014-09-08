using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLogCentral.Web.Models
{
    public class Statistic
    {
        public DateTime DateTime { get; set; }
        public int Warn { get; set; }
        public int Error { get; set; }
        public int Fatal { get; set; }
    }
}