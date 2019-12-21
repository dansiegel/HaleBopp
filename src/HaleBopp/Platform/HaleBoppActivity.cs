using System;
using Comet;
using Comet.Android;
using HaleBopp.Common;
using HaleBopp.Navigation;

namespace HaleBopp.Platform
{
    public class HaleBoppActivity : CometActivity, IRootNavigationController
    {
        View IRootNavigationController.View
        {
            get => Page;
            set => Page = value;
        }

        protected void Start<TApp>(string uri, IParameters parameters = null)
            where TApp : ICometApp =>
            Start<TApp>(UriParsingHelper.Parse(uri), parameters);

        protected void Start<TApp>(Uri uri, IParameters parameters = null)
            where TApp : ICometApp
        {
            CometHost.Start<TApp>(this, uri, parameters);
        }
    }
}
