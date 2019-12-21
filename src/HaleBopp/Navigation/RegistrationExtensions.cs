using System;
using System.Linq;
using Comet;
using DryIoc;
using HaleBopp.Common;
using HaleBopp.Views;

namespace HaleBopp
{
    public static class RegistrationExtensions
    {
        public static IContainer RegisterForNavigation<TView>(this IContainer container, string name = null)
            where TView : View =>
            container.RegisterForNavigation(typeof(TView), name);

        internal static IContainer RegisterForNavigation(this IContainer container, Type viewType, string name = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                name = viewType.Name;
            }

            container.RegisterDelegate<View>(r => ConstructView(r, viewType),
                                             Reuse.Transient,
                                             serviceKey: name);
            return container;
        }

        private static View ConstructView(IResolverContext context, Type viewType)
        {
            var scope = context.OpenScope();
            var svl = scope.Resolve<IScopedViewLocator>();
            var view = scope.Resolve(viewType) as View;
            svl.View = view;
            CometUtilities.SetMappings(view);

            var configurations = scope.ResolveMany<IViewConfiguration>();
            foreach(var config in configurations)
            {
                config.Configure(view);
            }

            var stateFieldNames = CometUtilities.CheckForStateAttributes(viewType).Select(x => x.Name).ToArray();
            scope.InjectPropertiesAndFields(view, stateFieldNames);
            return view;
        }
    }
}
