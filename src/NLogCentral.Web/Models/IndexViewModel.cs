using System.Collections.Generic;

namespace NLogCentral.Web.Models
{
    public class IndexViewModel : ViewModelBase
    {
        public List<LogModel> Logs { get; set; }
        public List<string> ProcessNames { get; set; }
    }
}