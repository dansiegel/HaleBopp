using System;
using DryIoc;
using HaleBopp;
using HaleBoppSample.Views;

namespace HaleBoppSample
{
    public class CometApp : ICometApp
    {
        public void RegisterServices(IContainer container)
        {
            container.RegisterForNavigation<ViewA>();
            container.RegisterForNavigation<ViewB>();
        }
    }
}
