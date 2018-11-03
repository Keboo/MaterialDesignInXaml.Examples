using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DrawerHostRegions.MainModule
{
    class SampleModule : IModule
    {
        IRegionManager _manager;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _manager = containerProvider.Resolve<IRegionManager>();
            var regions = _manager.Regions;
            regions["MainRegion"].Add(containerProvider.Resolve<MainView>());

            regions["Test"].Add(containerProvider.Resolve<RightDrawerView>());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) {}
    }
}
