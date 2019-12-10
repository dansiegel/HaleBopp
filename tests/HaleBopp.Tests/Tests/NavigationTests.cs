using System;
using HaleBopp.Mocks;
using Xunit;

namespace HaleBopp.Tests
{
    public class NavigationTests
    {
        public NavigationTests()
        {
            TestHost.Start<MockApp>("ViewA");
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
