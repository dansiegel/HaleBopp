using System;
using System.ComponentModel;
using Comet;
using DryIoc;
using HaleBopp.Common;
using HaleBopp.Logging;
using HaleBopp.Navigation;
using HaleBopp.Views;
using Container = DryIoc.Container;
using IContainer = DryIoc.IContainer;

namespace HaleBopp
{
    public static class CometHost
    {
        private static Lazy<IContainer> _lazyContainer = new Lazy<IContainer>(InitializeContainer);
        private static IContainer _container;
        public static IContainer Container => _container ?? (_container = _lazyContainer.Value);

        private static IContainer InitializeContainer()
        {
            var rules = Rules.Default
                .WithAutoConcreteTypeResolution()
                .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                .WithoutThrowOnRegisteringDisposableTransient()
                .WithFuncAndLazyWithoutRegistration()
#if __IOS__
                .WithoutFastExpressionCompiler()
#endif
                .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace);
            var container = new Container(rules);
            RegisterRequiredServices(container);
            return container;
        }

        private static void RegisterRequiredServices(IContainer container)
        {
            container.Register<IActiveViewLocator, ActiveViewLocator>(Reuse.Singleton);
            container.Register<IScopedViewLocator, ScopedViewLocator>(Reuse.Scoped);
            container.Register<INavigationService, NavigationService>(Reuse.Scoped);
            container.Register<IViewConfiguration, DefaultViewConfigurations>(Reuse.Singleton, serviceKey: "defaultConfiguration");
            container.Register<ILogger, NullLogger>();
        }

        public static void Start<TApp>(IRootNavigationController controller, string uri, IParameters parameters = null)
            where TApp : ICometApp =>
            Start<TApp>(controller, UriParsingHelper.Parse(uri), parameters);

        public static void Start<TApp>(IRootNavigationController controller, Uri uri, IParameters parameters = null)
            where TApp : ICometApp
        {
            Container.UseInstance<IRootNavigationController>(controller);
            var app = Container.Resolve<TApp>();
            app.RegisterServices(Container);
            var scope = Container.OpenScope();
            var navService = scope.Resolve<INavigationService>();
            navService.Navigate(uri, parameters, OnInitialNavigationResult);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Reset()
        {
            _container?.Dispose();
            _container = null;
        }

        private static void OnInitialNavigationResult(NavigationResult result)
        {
            if (result.Success) return;

            var logger = Container.Resolve<ILogger>();
            logger.Report(result.Exception, null);

            if(System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}
