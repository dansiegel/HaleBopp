using Foundation;
using UIKit;
using HaleBopp.Platform;

namespace HaleBoppSample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : HaleBoppApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
#if DEBUG
            Comet.Reload.Init();
#endif

            Start<CometApp>("ViewA");
            return true;
        }
    }
}


