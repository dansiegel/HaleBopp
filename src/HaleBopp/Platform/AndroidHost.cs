#if MONOANDROID
using System;
using System.Collections.Generic;
using System.Text;
using Comet;
using Comet.Android;
using DryIoc;
using HaleBopp.Common;
using HaleBopp.Navigation;

namespace HaleBopp.Platform
{
    public static class AndroidHost
    {
        public static void Start<TApp>(string uri, IParameters parameters = null)
            where TApp : ICometApp =>
            Start<TApp>(UriParsingHelper.Parse(uri), parameters);

        public static void Start<TApp>(Uri uri, IParameters parameters = null)
            where TApp : ICometApp
        {
            var controller = new AndroidRootViewController();
            CometHost.Start<TApp>(controller, uri, parameters);
        }
    }

    internal class AndroidRootViewController : IRootNavigationController
    {
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
            var cometActivity = AndroidContext.CurrentContext as CometActivity;
            cometActivity.Page = View;
        }
    }
}
#endif
