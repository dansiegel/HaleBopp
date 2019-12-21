using System;
using Comet;
using Comet.iOS;
using HaleBopp.Common;
using HaleBopp.Navigation;
using UIKit;

namespace HaleBopp.Platform
{
    public abstract class HaleBoppApplicationDelegate : UIApplicationDelegate
    {
        //public override UIWindow Window { get; set; }

        UIWindow window;

        protected void Start<TApp>(string uri, IParameters parameters = null)
            where TApp : ICometApp =>
            Start<TApp>(UriParsingHelper.Parse(uri), parameters);

        protected void Start<TApp>(Uri uri, IParameters parameters = null)
            where TApp : ICometApp
        {
            var controller = new HaleBoppViewController();
            CometHost.Start<TApp>(controller, uri, parameters);

            window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = controller
            };
            window.MakeKeyAndVisible();
        }
    }

    public class HaleBoppViewController : CometViewController, IRootNavigationController
    {
        View IRootNavigationController.View
        {
            get => CurrentView;
            set => CurrentView = value;
        }
    }
}
