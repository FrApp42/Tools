namespace FrApp42.Web.API
{
    public struct Result<T>
    {
        public int StatusCode;
        public T? Value;
        public string? Error;

        public Result(int statusCode, T value, string error = "")
        {
            StatusCode = statusCode;
            Value = value;
            Error = error;
        }
    }
}
