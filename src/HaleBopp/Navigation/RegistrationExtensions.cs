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
            where TView : View
        {
            if(string.IsNullOrEmpty(name))
            {
                name = typeof(TView).Name;
            }

            var viewType = typeof(TView);
            var stateFields = CometUtilities.GetStatePropertiesAndFields(container, viewType).ToArray();

            if(stateFields.Length == 0)
            {
                throw new InvalidOperationException($"Your view '{viewType.Name}' must declare at least one property with the State Attribute.");
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
            scope.Dispose();
            scope = null;
            return view;
        }
    }
}
