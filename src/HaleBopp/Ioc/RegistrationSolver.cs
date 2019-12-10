using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comet;

namespace HaleBopp.Ioc
{
    public class RegistrationSolver
    {
        public static RegistrationSolver Instance { get; }

        static RegistrationSolver()
        {
            Instance = new RegistrationSolver();
        }

        public RegistrationSolver InAppAssembly<TApp>()
            where TApp : ICometApp
        {
            var appAssembly = typeof(TApp).Assembly;
            AssemblyPredicates.Add(a => a == appAssembly);
            return this;
        }

        public RegistrationSolver InAssembly(params Type[] assemblyTypes) =>
            InAssembly(assemblyTypes.Select(x => x.Assembly).ToArray());

        public RegistrationSolver InAssembly(params Assembly[] assemblies)
        {
            AssemblyPredicates.Add(a => assemblies.Any(x => x == a));
            return this;
        }

        public RegistrationSolver InViewsNamespace()
        {
            TypePredicates.Add(t => t.Namespace.EndsWith(".Views"));
            return this;
        }

        public RegistrationSolver Where(Func<Type, bool> predicate)
        {
            TypePredicates.Add(predicate);
            return this;
        }

        private readonly List<Func<Type, bool>> TypePredicates;
        private readonly List<Func<Assembly, bool>> AssemblyPredicates;

        private RegistrationSolver()
        {
            TypePredicates = new List<Func<Type, bool>>();
            AssemblyPredicates = new List<Func<Assembly, bool>>();
        }

        internal static IEnumerable<Type> GetViews()
        {
            var viewTypes = new List<Type>();
            foreach(var assembly in Instance.GetAssemblies())
            {
                viewTypes.AddRange(Instance.GetViews(assembly));
            }

            return viewTypes;
        }

        private IEnumerable<Type> GetViews(Assembly assembly)
        {
            var types = assembly.ExportedTypes.Where(t => !t.IsAbstract && typeof(View).IsAssignableFrom(t));
            foreach(var predicate in TypePredicates)
            {
                types = types.Where(predicate);
            }

            return types;
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.FullName.StartsWith("Microsoft.") &&
                !x.FullName.StartsWith("System.") &&
                x != typeof(Comet.View).Assembly && x != GetType().Assembly);

            foreach(var predicate in AssemblyPredicates)
            {
                assemblies = assemblies.Where(predicate);
            }

            return assemblies;
        }
    }
}
