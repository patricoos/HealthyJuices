using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Common.Utils
{

    public class Response
    {
        public string Message => Errors == null ? default : string.Join("\n,", Errors.ToArray());
        public ResponseStatus Status { get; init; }
        public IEnumerable<string> Errors { get; init; }


        public bool Succeed => Status == ResponseStatus.Success;
        public bool Failed => !Succeed;


        public static Response Fail(ResponseStatus statusCode, params string[] messages) => new Response(statusCode, messages);
        public static Response Success() => new Response(ResponseStatus.Success);

        public static Response<T> Fail<T>(ResponseStatus statusCode, params string[] messages) => new Response<T>(default, statusCode, messages);
        public static Response<T> Success<T>(T data) => new Response<T>(data, ResponseStatus.Success);


        protected Response(ResponseStatus status, params string[] messages)
        {
            Errors = messages;
            Status = status;
        }
    }

    public class Response<TResult> : Response
    {
        public TResult Value { get; init; }


        public static Response<T> Fail<T>(ResponseStatus statusCode, params string[] messages) => new Response<T>(default, statusCode, messages);
        public static Response<T> Success<T>(T data) => new Response<T>(data, ResponseStatus.Success);


        private Response(TResult value, ResponseStatus status, params string[] messages) : base(status, messages)
        {
            Value = value;
        }
    }
}