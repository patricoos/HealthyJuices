using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyJuices.Application.Utils
{
    public static class StartupApplicationExtensions
    {
        public static IServiceCollection RegisterMediatR(this IServiceCollection @this)
        {
            @this.AddMediatR(Assembly.GetExecutingAssembly());
            return @this;
        }
    }
}