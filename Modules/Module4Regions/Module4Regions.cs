using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

using Bionte.Views;

namespace Bionte.Modules
{
    class Module4Regions : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        public Module4Regions(IRegionViewRegistry registry)
        {
            this.regionViewRegistry = registry;   
        }

        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(View4Regions));
        }
    }
}
