using System;
using Comet;

namespace HaleBopp.Views
{
    public class ActiveViewLocator : IActiveViewLocator
    {
        private WeakReference<View> _weakView;
        public View View
        {
            get => _weakView.TryGetTarget(out var target) ? target : null;
            set => _weakView = new WeakReference<View>(value);
        }
    }

}
