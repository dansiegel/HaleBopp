using DryIoc;
using HaleBopp.Mocks.Views;

namespace HaleBopp.Mocks
{
    public class MockApp : ICometApp
    {
        public void RegisterServices(IContainer container)
        {
            container.RegisterForNavigation<ViewA>();
            container.RegisterForNavigation<ViewB>();
            container.RegisterForNavigation<ViewC>();
        }
    }
}
