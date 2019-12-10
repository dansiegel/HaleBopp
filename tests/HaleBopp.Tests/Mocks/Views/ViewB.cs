using Comet;
using HaleBopp.Mocks.Models;

namespace HaleBopp.Mocks.Views
{
    public class ViewB : View
    {
        [State]
        private ModelA ModelA;

        [State]
        private readonly ModelB ModelB;
    }
}
