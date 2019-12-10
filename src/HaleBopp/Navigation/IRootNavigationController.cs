using Comet;

namespace HaleBopp.Navigation
{
    public interface IRootNavigationController
    {
        View View { get; set; }
    }

    internal class EmptyRootNavigationController : IRootNavigationController
    {
        public View View { get; set; }
    }
}
