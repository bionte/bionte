using System.Windows;

using Microsoft.Practices.Unity;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Logging;

using Bionte.Modules;

namespace YAPOS
{
    class Bootstrapper : UnityBootstrapper
    {
        private readonly LoggerFacade _logger = new LoggerFacade();

        protected override ILoggerFacade CreateLogger()
        {
            return _logger;
        }
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        //protected override void ConfigureModuleCatalog()
        //{
        //    base.ConfigureModuleCatalog();

        //    ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
        //    moduleCatalog.AddModule(typeof(Bionte.Modules.ModuelHelloWorld));
        //}

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}
