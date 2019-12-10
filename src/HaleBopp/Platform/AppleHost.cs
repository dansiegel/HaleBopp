#if __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using Comet;
using Comet.iOS;
using DryIoc;
using HaleBopp.Common;
using HaleBopp.Navigation;
using UIKit;

namespace HaleBopp.Platform
{
    public static class AppleHost
    {
        public static UIKit.UIWindow Start<TApp>(string uri, IParameters parameters = null)
            where TApp : ICometApp =>
            Start<TApp>(UriParsingHelper.Parse(uri), parameters);

        public static UIKit.UIWindow Start<TApp>(Uri uri, IParameters parameters = null)
            where TApp : ICometApp
        {
            var controller = new AppleRootViewController();
            CometHost.Start<TApp>(controller, uri, parameters);
            return controller.Window;
        }
    }

    internal class AppleRootViewController : IRootNavigationController
    {
        public AppleRootViewController()
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
        }

        public UIWindow Window { get; }

        private View _view;
        public View View
        {
            get => _view;
            set
            {
                if (EqualityComparer<View>.Default.Equals(_view, value))
                    return;

                _view = value;
                OnViewChanged();
            }
        }

        private void OnViewChanged()
        {
            if(View is null)
            {
                Window.RootViewController = null;
            }
            else
            {
                Window.RootViewController = View.ToViewController();

                if (!Window.IsKeyWindow)
                {
                    Window.MakeKeyAndVisible();
                }
            }
        }
    }
}
#endif
