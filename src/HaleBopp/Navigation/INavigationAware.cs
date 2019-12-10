using HaleBopp.Common;

namespace HaleBopp.Navigation
{
    public interface INavigationAware
    {
        void OnNavigatedTo(IParameters parameters);

        void OnNavigatedFrom(IParameters parameters);
    }
}
