using System;
using System.Linq.Expressions;
using System.Reflection;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Tests.EndToEnd.Extensions
{
    public static class ValueObjectExtensions
    {
        internal static void SetProperty<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> prop, TProperty value) where TSource : ValueObject
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)prop.Body).Member;
            propertyInfo.SetValue(source, value);
        }
    }
}