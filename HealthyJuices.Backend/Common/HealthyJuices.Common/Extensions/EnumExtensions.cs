using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace HealthyJuices.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns collection of all enum type items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEnum"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllValues<T>(this T theEnum) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!(theEnum is Enum))
                throw new ArgumentException($"{theEnum} is not an enum");

            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool HasValue<T>(this T @this) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!(@this is Enum))
                throw new ArgumentException($"{@this} is not an enum");

            var firstValue = @this.GetAllValues().FirstOrDefault().Value();
            return @this.Value() >= firstValue;
        }

        public static T Parse<T>(this string str) where T : struct
        {
            try
            {
                var res = (T)Enum.Parse(typeof(T), str);
                return res;
            }
            catch
            {
                return default(T);
            }
        }

        public static int Value<T>(this T @enum) where T : struct, IComparable, IFormattable, IConvertible
        {
            var val = Convert.ToInt32(@enum);
            return val;
        }

        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
        }

        public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
        {
            return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
        }

        public static bool IsFlagSet<T>(this T value, T flag) where T : struct
        {
            if (!(value is Enum))
                throw new ArgumentException($"{value} is not an enum");
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (!(e is Enum))
                return string.Empty;

            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture))
                    continue;

                var memInfo = type.GetMember(type.GetEnumName(val));

                if (memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    return descriptionAttribute.Description;
                }
            }

            return string.Empty;
        }


        public static string ToFormattedText(this Enum value)
        {
            var newStr = Regex.Replace(value.ToString(), "(?<=[A-Za-z])(?=[^A-Za-z])", " ");

            return newStr;
        }

        #region - Private -
        private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
        {
            ulong bits = Convert.ToUInt64(value);
            List<Enum> results = new List<Enum>();
            for (int i = values.Length - 1; i >= 0; i--)
            {
                ulong mask = Convert.ToUInt64(values[i]);
                if (i == 0 && mask == 0L)
                    break;
                if ((bits & mask) == mask)
                {
                    results.Add(values[i]);
                    bits -= mask;
                }
            }
            if (bits != 0L)
                return Enumerable.Empty<Enum>();
            if (Convert.ToUInt64(value) != 0L)
                return results.Reverse<Enum>();
            if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
                return values.Take(1);
            return Enumerable.Empty<Enum>();
        }

        private static IEnumerable<Enum> GetFlagValues(Type enumType)
        {
            ulong flag = 0x1;
            foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                if (bits == 0L)
                    //yield return value;
                    continue; // skip the zero value
                while (flag < bits) flag <<= 1;
                if (flag == bits)
                    yield return value;
            }
        }
        #endregion
    }
}
