using System;
using Nancy;
using Nancy.ModelBinding;
using NLogCentral.Web.Models;
using Raven.Client;
using Raven.Client.Document;

namespace NLogCentral.Web.Modules
{
    public class LogModule : NancyModule
    {
        private readonly IDocumentStore _store;

        public LogModule(IDocumentStore store) : base("log")
        {
            _store = store;
            Post["/"] = AddNewLog;
        }

        private dynamic AddNewLog(dynamic parameters)
        {
            var log = this.Bind<LogModel>();
            // Saving changes using the session API
            using (IDocumentSession session = _store.OpenSession())
            {
                // Operations against session
                session.Store(log);

                // Flush those changes
                session.SaveChanges();
            }

            return HttpStatusCode.OK;
        }
    }
}
