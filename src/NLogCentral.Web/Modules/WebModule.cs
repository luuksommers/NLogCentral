using System;
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
            Get["/"] = Index;
            Get["/GraphData"] = GraphData;
        }

        private dynamic Index(dynamic parameters)
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
        }


        public class Statistic
        {
            public int Hour { get; set; }
            public int Info { get; set; }
            public int Warn { get; set; }
            public int Error { get; set; }
            public int Fatal { get; set; }
        }

        private dynamic GraphData(dynamic parameters)
        {
            // most probably the values will come from a database
            // this is just a sample to show you 
            // that you can return an IEnumerable object
            // and it will be serialized properly
            var stats = new List<Statistic>();
            var rand = new Random();
            for (int i = 0; i < 24; i++)
            {
                stats.Add(new Statistic()
                {
                    Hour = DateTime.UtcNow.Hour + i,
                    Info = rand.Next(30,100),
                    Warn = rand.Next(0, 30),
                    Error = rand.Next(0, 10),
                    Fatal = rand.Next(0, 5)
                });
            }

            return stats;
        }

    }
}