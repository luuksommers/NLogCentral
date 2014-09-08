using Nancy;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace NLogCentral.Web
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var documentStore = new DocumentStore { ConnectionStringName = "RavenDb" };
            documentStore.Initialize();

            IndexCreation.CreateIndexes(typeof(CustomBootstrapper).Assembly, documentStore);

            container.Register<IDocumentStore>(documentStore);
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
