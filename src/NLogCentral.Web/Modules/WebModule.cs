using System.Collections.Generic;
using System.Linq;
using Nancy;
using NLogCentral.Web.Models;
using Raven.Client;
using Raven.Client.Document;

namespace NLogCentral.Web.Modules
{
    public class WebModule : NancyModule
    {
        public class ViewModelBase
        {
            public string Title { get; set; }
        }

        public class IndexViewModel : ViewModelBase
        {
            public List<LogModel> Logs { get; set; }
            public List<string> ProcessNames { get; set; }
        }

        public WebModule()
        {
            Get["/"] = _ =>
            {
                var documentStore = new DocumentStore { Url = "http://192.168.110.128:8080/" };
                documentStore.Initialize();
                // Saving changes using the session API
                using (IDocumentSession session = documentStore.OpenSession())
                {
                    // Operations against session
                    var vm = new IndexViewModel();
                    vm.Logs = session.Query<LogModel>().Take(10).ToList();
                    vm.ProcessNames = session.Query<LogModel>().Select(a => a.ProcessName).Distinct().ToList();
                    vm.Title = "Dashboard";

                    return View["index.cshtml", vm];
                }
            };
        }
    }
}