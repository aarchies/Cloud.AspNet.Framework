namespace Glasssix.Contrib.Caller.DaprClient
{
    public class GlasssixDaprClient : GlasssixCallerClient
    {
        private string _appId = default!;

        public GlasssixDaprClient(string appid)
        {
            AppId = appid;
        }

        internal GlasssixDaprClient()
        {
        }

        public string AppId
        {
            get => _appId;
            set
            {
                //GlasssixArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(AppId));

                _appId = value;
            }
        }
    }
}