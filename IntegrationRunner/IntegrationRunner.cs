using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Core;
using IntegrationServices.Interfaces;

namespace IntegrationRunner
{
    class IntegrationRunner
    {
        static void Main(string[] args)
        {
            LoadReferencedAssemblies();
            var container = DependencyBuilder.SetupDependencyInjection().Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var availableIntegrations = scope.Resolve<IEnumerable<IIntegration>>().ToList();
                Func<string, string, bool> compare = (a, b) => String.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);
                var integrationName = args.FirstOrDefault();
                if (!availableIntegrations.Any(i => compare(i.IntegrationName, integrationName)))
                {
                    DisplayIntegrationOptions(availableIntegrations);
                    return;
                }
                var logger = scope.Resolve<ILoggingService>();
                logger.SetLogger(integrationName);
                try
                {
                    availableIntegrations.First(i => compare(i.IntegrationName, integrationName)).Execute(args);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("{0} integration failed", integrationName), ex);
                }
            }
        }

        private static void DisplayIntegrationOptions(List<IIntegration> availableIntegrations)
        {
            Console.WriteLine("Please specify an integration as an argument. Available Integrations:");
            availableIntegrations.ForEach(a => Console.WriteLine("{0} : {1}", a.IntegrationName, a.Directions));
        }

        /// <summary>
        /// This loads all referenced assemblies into the current app domain to make sure that they gets picked up by the 
        /// dependency injection
        /// </summary>
        private static void LoadReferencedAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad =
                referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
        }
    }
}
