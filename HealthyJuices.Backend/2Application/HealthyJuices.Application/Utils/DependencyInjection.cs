using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using HealthyJuices.Application.Behaviours;

namespace HealthyJuices.Application.Utils
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterMediatR(this IServiceCollection @this)
        {
            @this.AddMediatR(Assembly.GetExecutingAssembly());

            return @this;
        }

        public static IServiceCollection RegisterValidators(this IServiceCollection @this)
        {
            @this.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            @this.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return @this;
        }
    }
}