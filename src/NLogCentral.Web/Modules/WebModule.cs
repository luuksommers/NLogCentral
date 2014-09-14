using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using NLogCentral.Web.Indexes;
using NLogCentral.Web.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace NLogCentral.Web.Modules
{
    public class WebModule : NancyModule
    {
        private readonly IDocumentStore _store;


        public WebModule(IDocumentStore store)
        {
            _store = store;
            Get["/"] = Index;
            Get["/GraphData"] = GraphData;
            Get["/Applications"] = Applications;
            Get["/Applications/{application}"] = Application;

        }

        private dynamic Index(dynamic parameters)
        {

            // Saving changes using the session API
            using (IDocumentSession session = _store.OpenSession())
            {
                // Operations against session
                var vm = new IndexViewModel();

                vm.Logs = session.Query<LogModel>().OrderByDescending(a => a.Date).Take(10).ToList();
                vm.ProcessNames = session.Query<LogModel>().Select(a => a.ProcessName).Distinct().ToList();
                vm.Title = "Dashboard";

                return View["index.cshtml", vm];
            }
        }

        private dynamic GraphData(dynamic parameters)
        {
            var retval = new List<Statistic>();

            // Saving changes using the session API
            using (IDocumentSession session = _store.OpenSession())
            {
                // Operations against session
                var statistics = session.Query<LogModelCountByLevelPerHour.ReduceResult, LogModelCountByLevelPerHour>().ToList()
                    .GroupBy(x => x.Hour)
                    .Select(x => new Statistic()
                    {
                        DateTime = AsUtc(DateTime.MinValue.AddTicks(TimeSpan.TicksPerHour * x.Key)),
                        Error = x.Where(y => y.Level == "Error").Sum(y => y.Count),
                        Warn = x.Where(y => y.Level == "Warn").Sum(y => y.Count),
                        Fatal = x.Where(y => y.Level == "Fatal").Sum(y => y.Count),
                    })
                    .ToList();


                var dateLoop = Truncate(DateTime.UtcNow.AddHours(-24), TimeSpan.FromHours(1));
                while (dateLoop < DateTime.UtcNow.AddHours(1))
                {
                    var value = statistics.FirstOrDefault(a => a.DateTime == dateLoop);
                    if(value == null)
                        retval.Add(new Statistic(){DateTime = dateLoop});
                    else
                        retval.Add(value);

                    dateLoop = dateLoop.AddHours(1);
                }

                return retval;
            }
        }

        private dynamic Applications(dynamic parameters)
        {
            // Saving changes using the session API
            using (IDocumentSession session = _store.OpenSession())
            {
                // Operations against session
                var vm = new ApplicationsViewModel();

                vm.Applications = session.Query<LogModel>().Select(x=>x.ProcessName).Distinct().ToList();
                vm.Title = "Applications";
                return View["applications.cshtml", vm];
            }
        }

        private dynamic Application(dynamic parameters)
        {
            // Saving changes using the session API
            using (IDocumentSession session = _store.OpenSession())
            {
                var processName = (string)parameters.application;
                // Operations against session
                var vm = new IndexViewModel();

                vm.Logs = session.Query<LogModel>().Where(a => a.ProcessName == processName).OrderByDescending(a => a.Date).Take(100).ToList();
                vm.Title = processName;

                return View["application.cshtml", vm];
            }
        }

        public static DateTime Truncate(DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static DateTime AsUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}