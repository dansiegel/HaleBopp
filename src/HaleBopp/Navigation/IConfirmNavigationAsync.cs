using System.Threading.Tasks;
using HaleBopp.Common;

namespace HaleBopp.Navigation
{
    public interface IConfirmNavigationAsync
    {
        Task<bool> CanNavigateAsync(IParameters parameters);
    }
}
