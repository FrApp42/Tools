namespace FrenchyApps42.Web.ApiRequest.Structs
{
    public struct Result<T>
    {
        public int StatusCode;
        public T? Value;
        public string? Error;

        public Result(int statusCode, T value, string error = "")
        {
            this.StatusCode = statusCode;
            this.Value = value;
            this.Error = error;
        }
    }
}
