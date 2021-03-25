using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using HealthyJuices.Application.Behaviours;
using HealthyJuices.Application.Mappers;

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

        public static IServiceCollection RegisterAutoMapper(this IServiceCollection @this)
        {
            @this.AddAutoMapper(Assembly.GetExecutingAssembly());
            return @this;
        }
    }
}