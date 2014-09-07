using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Raven.Client.Document;

namespace NLogCentral.Web
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            
            var documentStore = new DocumentStore { Url = "http://192.168.110.128:8080/" };
            documentStore.Initialize();
            //documentStore.DatabaseCommands.PutIndex("Logs/ByProcessName",
            //                                                    new IndexDefinitionBuilder<LogModel>
            //                                                    {
            //                                                        Map = logs => from log in logs
            //                                                                      select new { log.ProcessName }
            //                                                    });
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("js", @"Content/js")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("css", @"Content/css")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("fonts", @"Content/fonts")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("font-awesome-4.1.0", @"Content/font-awesome-4.1.0")
            );
        }
    }
}
