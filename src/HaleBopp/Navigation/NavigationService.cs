using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Comet;
using DryIoc;
using HaleBopp.Common;
using HaleBopp.Views;

namespace HaleBopp.Navigation
{
    public class NavigationService : INavigationService
    {
        private IContainer _container { get; }
        private IActiveViewLocator _activeViewLocator { get; }
        private IScopedViewLocator _scopedViewLocator { get; }
        private IRootNavigationController _rootNavigationController { get; }

        public NavigationService(IContainer container, IActiveViewLocator active, IScopedViewLocator scoped, IRootNavigationController root)
        {
            _container = container;
            _activeViewLocator = active;
            _scopedViewLocator = scoped;
            _rootNavigationController = root;
        }

        private View CreateViewFor(string name)
        {
            if (!_container.IsRegistered<View>(name))
                throw new NavigationException(NavigationException.ViewIsNotRegistered);

            return _container.Resolve<View>(name);
        }

        [SuppressMessage("Potential Code Quality Issues",
            "RECS0165:Asynchronous methods should return a Task instead of void")]
        public async void Navigate(Uri uri, IParameters parameters, Action<NavigationResult> callback)
        {
            try
            {
                await NavigateInternal(uri, parameters);
                callback(new NavigationResult());
            }
            catch (Exception ex)
            {
                callback(new NavigationResult(ex));
            }
        }

        private async Task NavigateInternal(Uri uri, IParameters parameters)
        {
            var navigationSegments = UriParsingHelper.GetUriSegments(uri);

            if (uri.IsAbsoluteUri || _rootNavigationController.View is null)
            {
                await ProcessNavigationForAbsoulteUri(navigationSegments, parameters);
            }
            else
            {
                await ProcessNavigation(_scopedViewLocator.View, navigationSegments, parameters);
            }
        }

        private async Task ProcessNavigationForAbsoulteUri(Queue<string> segments, IParameters parameters)
        {
            if (segments.Count == 0)
                return;

            (var view, var segmentParameters) = await ProcessSegmentAsync(null, segments, parameters).ConfigureAwait(false);
            _rootNavigationController.View = view;

            ViewUtilities.OnNavigatedTo(view, segmentParameters);
            await ProcessNavigation(view, segments, parameters);
        }

        private async Task ProcessNavigation(View currentView, Queue<string> segments, IParameters parameters)
        {
            if (segments.Count == 0)
                return;

            (var view, var segmentParameters) = await ProcessSegmentAsync(currentView, segments, parameters).ConfigureAwait(false);

            NavigationView.Navigate(currentView, view);
            ViewUtilities.OnNavigatedFrom(currentView, segmentParameters);
            ViewUtilities.OnNavigatedTo(view, segmentParameters);

            await ProcessNavigation(view, segments, parameters).ConfigureAwait(false);
        }

        private async Task<(View view, IParameters segmentParameters)> ProcessSegmentAsync(View currentView, Queue<string> segments, IParameters parameters)
        {
            var nextSegment = segments.Dequeue();

            var segmentParameters = UriParsingHelper.GetSegmentParameters(nextSegment, parameters);

            if (currentView != null && !await ViewUtilities.CanNavigateAsync(currentView, parameters))
            {
                throw new NavigationException(NavigationException.ViewCannotNavigate);
            }

            var view = CreateViewFor(UriParsingHelper.GetSegmentName(nextSegment));

            ViewUtilities.Initialize(view, segmentParameters);

            return (view, segmentParameters);
        }
    }
}
