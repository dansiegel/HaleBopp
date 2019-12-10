using Foundation;
using UIKit;
using HaleBopp.Platform;

namespace HaleBoppSample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
#if DEBUG
            Comet.Reload.Init();
#endif

            Window = AppleHost.Start<CometApp>("ViewA");
            return true;
        }
    }
}


