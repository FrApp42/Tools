namespace FrenchyApps42.Web.ApiRequest.Interfaces
{
    public class IApiRequest
    {
        public string URL { get; private set; }
        public HttpMethod Method { get; private set; }
        public Dictionary<string, string> RequestHeaders { get; private set; }
        public Dictionary<string, string> ContentHeaders { get; private set; }
        public Dictionary<string, string> QueryParams { get; private set; }
        public object Body { get; private set; }
        public byte[] DocumentBody { get; private set; }
        public string DocumentFileName { get; private set; }
        public string ContentType { get; private set; }
    }
}
