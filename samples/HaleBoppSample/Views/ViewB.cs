using System;
using System.Collections.Generic;
using System.Text;
using Comet;
using HaleBopp.Navigation;

namespace HaleBoppSample.Views
{
    public class ViewB : ModalView
    {
        private readonly INavigationService _navService;

        [Body]
        View CreateBody() => new VStack
        {
            new Text("Hello ViewB"),

        };
    }

    public class ViewC : View
    {

    }
}
