namespace HaleBopp.Mocks
{
    public static class TestHost
    {
        public static TestController Controller { get; private set; }

        public static void Start<TApp>(string uri)
            where TApp : ICometApp
        {
            Controller = new TestController();
            CometHost.Start<TApp>(Controller, uri);
        }
    }
}
