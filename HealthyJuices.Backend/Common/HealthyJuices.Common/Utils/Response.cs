namespace HealthyJuices.Common.Utils
{

    public class Response
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }

        public bool Failed => !Succeed;

        public static Response Fail(string message) => new Response(message, true);
        public static Response Success(string message = null) => new Response(message, false);

        protected Response(string msg, bool error)
        {
            Message = msg;
            Succeed = !error;
        }
    }

    public class Response<TResult>
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }

        public bool Failed => !Succeed;
        public TResult Value { get; set; }

        public static Response<T> Fail<T>(string message) => new Response<T>(default, message, true);
        public static Response<T> Success<T>(T data, string message = null) => new Response<T>(data, message, false);

        protected internal Response(TResult value, string msg, bool error) 
        {
            Value = value;
            Message = msg;
            Succeed = !error;
        }
    }
}