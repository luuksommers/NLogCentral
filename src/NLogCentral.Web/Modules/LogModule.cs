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
        public LogModule() : base("log")
        {
            Post["/"] = parameters =>
            {
                var log = this.Bind<LogModel>();
                var documentStore = new DocumentStore { Url = "http://192.168.110.128:8080/" };
                documentStore.Initialize();
                // Saving changes using the session API
                using (IDocumentSession session = documentStore.OpenSession())
                {
                    // Operations against session
                    session.Store(log);

                    // Flush those changes
                    session.SaveChanges();
                }

                return HttpStatusCode.OK;
            };
        }
    }
}
