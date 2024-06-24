namespace FrenchyApps42.Tools.Api.Interfaces
{
    public class IApiRequest
    {
        public string URL { get; private set; }
        public HttpMethod Method { get; private set; }
        public Dictionary<string, string> RequestHeaders { get; private set; }
        public Dictionary<string, string> ContentHeaders { get; private set; }
        public Dictionary<string, string> QueryParams { get; private set; }
        public object Body { get; private set; }
    }
}
