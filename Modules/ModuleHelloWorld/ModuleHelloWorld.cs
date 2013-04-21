using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

using Bionte.Views;

namespace Bionte.Modules
{
    public class ModuleHelloWorld : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        public ModuleHelloWorld(IRegionViewRegistry registry)
        {
            this.regionViewRegistry = registry;   
        }

        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(ViewHelloWorld));
        }
    }
}
