using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Comet;
using HaleBopp.Navigation;

namespace HaleBopp.Common
{
    public static class ViewUtilities
    {
        public static void InvokeActionOnViewAndStateObjects<T>(View view, Action<T> action)
            where T : class
        {
            InvokeActionOnViewAndStateObjectsInternal(view, action);

            var states = CometUtilities.GetViewStateObjects(view);
            foreach(var state in states)
            {
                InvokeActionOnViewAndStateObjectsInternal(state, action);
            }
        }

        private static void InvokeActionOnViewAndStateObjectsInternal<T>(object item, Action<T> action)
            where T : class
        {
            if(item is T itemAsT)
            {
                action(itemAsT);
            }
        }

        public static async Task InvokeActionOnViewAndStateObjectsAsync<T>(View view, Func<T, Task> action)
            where T : class
        {
            await InvokeActionOnViewAndStateObjectsInternalAsync(view, action);

            var states = CometUtilities.GetViewStateObjects(view);
            foreach (var state in states)
            {
                await InvokeActionOnViewAndStateObjectsInternalAsync(state, action);
            }
        }

        private static async Task InvokeActionOnViewAndStateObjectsInternalAsync<T>(object item, Func<T, Task> action)
            where T : class
        {
            if (item is T itemAsT)
            {
                await action(itemAsT);
            }
        }

        public static async Task<bool> CanNavigateAsync(View view, IParameters parameters)
        {
            List<bool> canNavigate = new List<bool>();
            InvokeActionOnViewAndStateObjects<IConfirmNavigation>(view, v => canNavigate.Add(v.CanNavigate(parameters)));

            if(!canNavigate.Any(x => x == false))
            {
                await InvokeActionOnViewAndStateObjectsAsync<IConfirmNavigationAsync>(view, async v => canNavigate.Add(await v.CanNavigateAsync(parameters)));
            }

            return !canNavigate.Any(x => x == false);
        }

        public static void OnNavigatedFrom(View view, IParameters parameters)
        {
            InvokeActionOnViewAndStateObjects<INavigationAware>(view, x => x.OnNavigatedFrom(parameters));
        }

        public static void OnNavigatedTo(View view, IParameters parameters)
        {
            InvokeActionOnViewAndStateObjects<INavigationAware>(view, x => x.OnNavigatedTo(parameters));
        }

        public static void Initialize(View view, IParameters parameters)
        {
            InvokeActionOnViewAndStateObjects<IInitialize>(view, x => x.Initialize(parameters));
            InvokeActionOnViewAndStateObjects<IAbracadabra>(view, x => Abracadabra(x, parameters));
        }

        internal static void Abracadabra(object item, IParameters parameters)
        {
            var props = item.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(x => x.CanWrite);

            foreach (var prop in props)
            {
                (var name, var isRequired) = prop.GetAutoInitializeProperty();

                if (!parameters.HasKey(name, out var key))
                {
                    if (isRequired)
                        throw new ArgumentNullException(name);
                    continue;
                }

                prop.SetValue(item, parameters.GetValue(key, prop.PropertyType));
            }
        }

        private static bool HasKey(this IParameters parameters, string name, out string key)
        {
            key = parameters.Select(x => x.Key).FirstOrDefault(k => k.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return !string.IsNullOrEmpty(key);
        }

        private static (string Name, bool IsRequired) GetAutoInitializeProperty(this PropertyInfo pi)
        {
            var attr = pi.GetCustomAttribute<AutoInitializeAttribute>();
            if (attr is null)
            {
                return (pi.Name, false);
            }

            return (string.IsNullOrEmpty(attr.Name) ? pi.Name : attr.Name, attr.IsRequired);
        }
    }
}
