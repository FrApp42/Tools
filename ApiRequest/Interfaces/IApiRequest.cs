namespace FrenchyApps42.Web.ApiRequest.Interfaces
{
    public interface IApiRequest
    {
        string URL { get; }
        HttpMethod Method { get; }

        Dictionary<string, string> RequestHeaders { get; }
        Dictionary<string, string> ContentHeaders { get; }
        Dictionary<string, string> QueryParams { get; }

        object Body { get; }
        byte[] DocumentBody { get; }
        string DocumentFileName { get; }
        string ContentType { get; }
    }
}
