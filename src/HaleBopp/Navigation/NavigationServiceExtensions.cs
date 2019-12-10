using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaleBopp.Common;

namespace HaleBopp.Navigation
{
    public static class NavigationServiceExtensions
    {
        public static void Navigate(this INavigationService navigationService, Uri uri) =>
            navigationService.Navigate(uri, null, null);

        public static void Navigate(this INavigationService navigationService, Uri uri, IParameters parameters) =>
            navigationService.Navigate(uri, parameters, null);

        public static void Navigate(this INavigationService navigationService, Uri uri, params (string, object)[] parameters) =>
            navigationService.Navigate(uri, CreateParameters(parameters), null);

        public static Task<NavigationResult> NavigateAsync(this INavigationService navigationService, Uri uri) =>
            navigationService.NavigateAsync(uri, default(IParameters));

        public static Task<NavigationResult> NavigateAsync(this INavigationService navigationService, Uri uri, params (string, object)[] parameters) =>
            navigationService.NavigateAsync(uri, CreateParameters(parameters));

        public static Task<NavigationResult> NavigateAsync(this INavigationService navigationService, Uri uri, IParameters parameters)
        {
            var tcs = new TaskCompletionSource<NavigationResult>();
            navigationService.Navigate(uri, parameters, r =>
            {
                tcs.SetResult(r);
            });
            return tcs.Task;
        }

        public static void Navigate(this INavigationService navigationService, string uri) =>
            navigationService.Navigate(UriParsingHelper.Parse(uri), null, null);

        public static void Navigate(this INavigationService navigationService, string uri, IParameters parameters) =>
            navigationService.Navigate(UriParsingHelper.Parse(uri), parameters, null);

        public static void Navigate(this INavigationService navigationService, string uri, Action<NavigationResult> callback, params (string, object)[] parameters) =>
            navigationService.Navigate(uri, CreateParameters(parameters), callback);

        public static void Navigate(this INavigationService navigationService, string uri, IParameters parameters, Action<NavigationResult> callback) =>
            navigationService.Navigate(UriParsingHelper.Parse(uri), parameters, callback);

        public static Task<NavigationResult> NavigateAsync(this INavigationService navigationService, string uri) =>
            navigationService.NavigateAsync(uri, null);

        public static Task<NavigationResult> NavigateAsync(this INavigationService navigationService, string uri, IParameters parameters) =>
            navigationService.NavigateAsync(UriParsingHelper.Parse(uri), parameters);

        private static IParameters CreateParameters((string key, object value)[] tupleParams)
        {
            var parameters = new Parameters();
            parameters.AddRange(tupleParams.Select(x => new KeyValuePair<string, object>(x.key, x.value)));
            return parameters;
        }
    }
}
