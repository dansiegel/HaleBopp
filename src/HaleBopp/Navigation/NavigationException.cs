using System;

namespace HaleBopp.Navigation
{
    public class NavigationException : Exception
    {
        public const string ViewCannotNavigate = "The current view cannot be navigated away from";
        public const string ViewIsNotRegistered = "The requested view has not been registered for Navigation";

        public NavigationException(string message)
            : base(message)
        {
        }
    }
}
