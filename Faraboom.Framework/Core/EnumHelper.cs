﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faraboom.Framework.Core
{
    public static class EnumHelper
    {
        public static string LocalizeEnum(object value)
        {
            if (value == null)
                return string.Empty;

            var result = value.ToString();
            return Globals.GetLocalizedDisplayName(value.GetType().GetField(result)) ?? result;
        }

        public static string LocalizeEnum<T>(object value) where T : struct
        {
            if (value == null)
                return string.Empty;

            var result = value.ToString();

            return Globals.GetLocalizedDisplayName(typeof(T).GetField(result)) ?? result;
        }

        public static string LocalizedShortName<T>(T value) where T : struct
        {
            var result = value.ToString();

            return Globals.GetLocalizedShortName(typeof(T).GetField(result)) ?? result;
        }

        public static string LocalizedDescription<T>(T value) where T : struct
        {
            var result = value.ToString();

            return Globals.GetLocalizedDescription(typeof(T).GetField(result)) ?? result;
        }

        public static bool TryParse<T>(this string name, out T t) where T : struct
        {
            t = default;

            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (Enum.TryParse(name, true, out t))
                return true;

            Type type = typeof(T);
            var fields = type.GetFields();
            foreach (var info in fields)
            {
                if (name == info.Name
                    || name == Globals.GetLocalizedDisplayName(info)
                    || name == Globals.GetLocalizedShortName(info))
                {
                    t = (T)info.GetValue(info);
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> FlagsEnumToList<T>(this T value) where T : struct
        {
            if (!typeof(T).IsSubclassOf(typeof(Enum)))
                throw new ArgumentException();

            return value.ToString().Split(',').Select(flag => (T)Enum.Parse(typeof(T), flag));
        }

        public static T ListToFlagsEnum<T>(this IEnumerable<T> value) where T : struct, IConvertible
        {
            var intlist = value.Select(t => t.ToInt32(new NumberFormatInfo()));
            var aggregatedint = intlist.Aggregate((prev, next) => prev | next);

            return (T)Enum.ToObject(typeof(T), aggregatedint);
        }
    }
}