using Comet;
using HaleBoppSample.Models;

namespace HaleBoppSample.Views
{
    public class ViewA : View
    {
        // Will not be handled by DI
        private readonly State<int> count = new State<int>();

        // Will be handled by DI
        [State]
        private readonly Person person;

        [Body]
        View CreateBody() => new VStack
        {
            new Text("Hello World")
        }.FillVertical();
    }
}
