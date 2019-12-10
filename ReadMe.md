# HaleBopp

HaleBopp is a lightweight framework for building apps with Clancey.Comet. It is inspired by Prism but in no way prescriptive of an MVVM pattern. HaleBopp provides an easy way to ensure you can register services, and Views for navigation, as well as an easy to use NavigationService that is entirely URI based and allows you to pass both URI and NavigationParameters.

```cs
public class CometApp : ICometApp
{
    public void RegisterServices(IContainer container)
    {
        container.RegisterForNavigation<ViewA>();
    }
}

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
```