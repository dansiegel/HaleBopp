using System;

namespace HaleBopp.Navigation
{
    public class NavigationException : Exception
    {
        public const string ViewCannotNavigate = "The current view cannot be navigated away from";

        public NavigationException(string message)
            : base(message)
        {
        }
    }
}
