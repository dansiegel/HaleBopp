using System;

namespace HaleBopp.Navigation
{
    public struct NavigationResult
    {
        public NavigationResult(Exception ex)
        {
            Exception = ex;
        }

        public bool Success => Exception is null;

        public Exception Exception { get; }
    }
}
