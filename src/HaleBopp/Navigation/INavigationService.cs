using System;
using HaleBopp.Common;

namespace HaleBopp.Navigation
{
    public interface INavigationService
    {
        void Navigate(Uri uri, IParameters parameters, Action<NavigationResult> callback);
    }
}
