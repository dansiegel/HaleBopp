using System;
using System.Collections.Generic;
using System.Text;
using Comet;
using HaleBopp.Mocks.Models;

namespace HaleBopp.Mocks.Views
{
    public class ViewA : View
    {
        [State]
        private readonly ModelA Model;
    }
}
