using System.Reflection;
using HealthyJuices.Application.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyJuices.Application.Utils
{
    public static class StartupExtensions
    {
        public static IServiceCollection RegisterMediatR(this IServiceCollection @this)
        {
            @this.AddMediatR(Assembly.GetExecutingAssembly());
            return @this;
        }

        //public static IServiceCollection AddValidators(this IServiceCollection @this)
        //{
        //    @this.Scan(scan => scan
        //        .FromAssemblyOf<IValidationHandler>()
        //        .AddClasses(classes => classes.AssignableTo<IValidationHandler>())
        //        .AsImplementedInterfaces()
        //        .WithTransientLifetime());
        //    return @this;
        //}
    }
}