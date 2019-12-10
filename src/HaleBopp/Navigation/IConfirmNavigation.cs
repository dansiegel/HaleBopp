using HaleBopp.Common;

namespace HaleBopp.Navigation
{
    public interface IConfirmNavigation
    {
        bool CanNavigate(IParameters parameters);
    }
}
