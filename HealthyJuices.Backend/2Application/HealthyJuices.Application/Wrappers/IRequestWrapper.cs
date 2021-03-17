using HealthyJuices.Common.Utils;
using MediatR;

namespace HealthyJuices.Application.Wrappers
{
    public interface IRequestWrapper : IRequest<Response> { }
    public interface IRequestWrapper<T> : IRequest<Response<T>> { }

    public interface IHandlerWrapper<TIn> : IRequestHandler<TIn, Response> where TIn : IRequestWrapper { }
    public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Response<TOut>> where TIn : IRequestWrapper<TOut> { }
}