using Faraboom.Framework.Data;
using Faraboom.Framework.DataAnnotation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Faraboom.Framework.Core
{
    public static class Globals
    {
        private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static CultureInfo CurrentCulture => Thread.CurrentThread.CurrentUICulture;

        public static bool IsRtl => CurrentCulture.TextInfo.IsRightToLeft;

        public static ProviderType ProviderType { get; set; }

        public static string NormalizePersian(this string str)
        {
            return str?.Trim()
                .Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace("ھ", "ه");
        }

        public static string GetClientIpAddress(this HttpRequest httpRequest)
        {
            var xForwardedFor = httpRequest.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(xForwardedFor))
                return xForwardedFor.ToString().Split(',').Last();

            return httpRequest.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public static string GetHeaderParameter(this HttpRequest httpRequest, string key)
        {
            return httpRequest.Headers.TryGetValue(key, out StringValues stringValues) ? stringValues.FirstOrDefault() : null;
        }

        public static bool ValidateNationalCode(string nationalCode)
        {
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };

            if (string.IsNullOrWhiteSpace(nationalCode) || allDigitEqual.Contains(nationalCode) || nationalCode.Length != 10)
            {
                return false;
            }

            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
            var c = b % 11;

            return ((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a));
        }

        public static string GetLocalizedDisplayName(MemberInfo member)
        {
            if (member == null)
                return null;

            string name;
            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>(false);
            if (displayAttribute != null)
            {
                name = GetLocalizedValueInternal(displayAttribute, member.Name, Constants.ResourceKey.Name);
                return !string.IsNullOrWhiteSpace(name) ? name : member.Name;
            }

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            name = customAttribute?.GetName();
            return !string.IsNullOrWhiteSpace(name) ? name : member.Name;
        }

        public static string DisplayNameFor<T>(this Expression<Func<T, object>> expression)
            where T : class
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression?.Member.MemberType == MemberTypes.Property)
            {
                return GetLocalizedDisplayName(memberExpression.Member);
            }
            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression?.Member.MemberType == MemberTypes.Property)
                    return GetLocalizedDisplayName(memberExpression.Member);
            }

            return "";
        }

        public static string GetLocalizedShortName(MemberInfo member)
        {
            if (member == null)
                return null;

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay != null)
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.ShortName);

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetShortName();
        }

        internal static string GetLocalizedValueInternal(DisplayAttribute displayAttribute, string propertyName, Constants.ResourceKey resourceKey, ResourceManager cachedResourceManager = null)
        {
            var result = "";
            if (displayAttribute.ResourceType != null)
            {
                if (cachedResourceManager == null)
                    cachedResourceManager = new ResourceManager(displayAttribute.ResourceType);

                if (displayAttribute.EnumType == null)
                {
                    result = cachedResourceManager.GetString($"{propertyName}_{resourceKey}");
                }
                else
                {
                    result = cachedResourceManager.GetString($"{displayAttribute.EnumType.Name}_{propertyName}_{resourceKey}");
                    if (resourceKey == Constants.ResourceKey.Name && string.IsNullOrWhiteSpace(result))
                        result = cachedResourceManager.GetString($"{displayAttribute.EnumType.Name}_{propertyName}");
                }
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                switch (resourceKey)
                {
                    case Constants.ResourceKey.Name:
                        result = displayAttribute.Name;
                        break;
                    case Constants.ResourceKey.ShortName:
                        result = displayAttribute.ShortName;
                        break;
                    case Constants.ResourceKey.Description:
                        result = displayAttribute.Description;
                        break;
                    case Constants.ResourceKey.Prompt:
                        result = displayAttribute.Prompt;
                        break;
                    case Constants.ResourceKey.GroupName:
                        result = displayAttribute.GroupName;
                        break;
                }
            }

            return result;
        }

        public static string GetLocalizedDescription(MemberInfo member)
        {
            if (member == null)
                return null;

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay != null)
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.Description);

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetDescription();
        }

        public static string GetLocalizedPromt(MemberInfo member)
        {
            if (member == null)
                return null;

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay != null)
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.Prompt);

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetPrompt();
        }

        public static string GetLocalizedGroupName(MemberInfo member)
        {
            if (member == null)
                return null;

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay != null)
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.GroupName);

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetGroupName();
        }

        public static T ValueOf<T>(this Dictionary<string, string> dictionary, string key, T defaultValue = default)
        {
            dictionary.TryGetValue(key, out string tmp);
            return ValueOf(tmp, defaultValue);
        }

        public static T ValueOf<T>(this string value, T defaultValue = default)
        {
            return ValueOf(value, typeof(T), defaultValue);
        }

        public static dynamic ValueOf(this string value, Type type, dynamic defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                if (type.IsEnum)
                {
                    try
                    {
                        return Enum.Parse(type, value);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }

                var typeCode = Type.GetTypeCode(type);

                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        bool boolTmp;
                        return bool.TryParse(value, out boolTmp) ? boolTmp : defaultValue;

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                        byte byteTmp;
                        return byte.TryParse(value, out byteTmp) ? byteTmp : defaultValue;

                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                        short shortTmp;
                        return short.TryParse(value, out shortTmp) ? shortTmp : defaultValue;

                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        int intTmp;
                        return int.TryParse(value, out intTmp) ? intTmp : defaultValue;

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        long longTmp;
                        return long.TryParse(value, out longTmp) ? longTmp : defaultValue;

                    case TypeCode.Single:
                        float floatTmp;
                        return float.TryParse(value, out floatTmp) ? floatTmp : defaultValue;

                    case TypeCode.Double:
                        double doubleTmp;
                        return double.TryParse(value, out doubleTmp) ? doubleTmp : defaultValue;

                    case TypeCode.Decimal:
                        decimal decimalTmp;
                        return decimal.TryParse(value, out decimalTmp) ? decimalTmp : defaultValue;

                    case TypeCode.DateTime:
                        DateTime dateTimeTmp;
                        return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeTmp) ? dateTimeTmp : defaultValue;

                    case TypeCode.String:
                    case TypeCode.Char:
                        return value;

                    case TypeCode.Object:
                        if (type.Name == nameof(DateTimeOffset))
                        {
                            return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffsetTmp) ? dateTimeOffsetTmp : defaultValue;
                        }

                        if (type.Name == nameof(TimeSpan))
                        {
                            return TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out TimeSpan timeSpanTmp) ? timeSpanTmp : defaultValue;
                        }

                        if (type.Name == nameof(Guid))
                        {
                            return Guid.TryParse(value, out Guid guidTmp) ? guidTmp : defaultValue;
                        }

                        throw new ArgumentException(typeCode.ToString());

                    default:
                        throw new ArgumentException(typeCode.ToString());
                }
            }
            catch
            {
                throw new ArgumentException(nameof(value));
            }
        }

        public static int UserId(this HttpContext httpContext)
        {
            return UserId<int>(httpContext);
        }

        public static T UserId<T>(this HttpContext httpContext)
        {
            return UserId<T>(httpContext?.User);
        }

        public static int UserId(this ClaimsPrincipal claimsPrincipal)
        {
            return UserId<int>(claimsPrincipal);
        }

        public static T UserId<T>(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                return default;

            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier).ValueOf<T>();
        }

        public static IDictionary<string, object> ObjectToDictionary(object value)
        {
            var dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
                return new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);

            dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (value != null)
                dictionary = value.GetType().GetProperties().ToDictionary(t => t.Name, t => t.GetValue(value, null));

            return dictionary;
        }

        public static string GenerateRandomCode(int size = 32)
        {
            using var cryptoProvider = new RNGCryptoServiceProvider();
            var secretKeyByteArray = new byte[4 * size];
            cryptoProvider.GetBytes(secretKeyByteArray);
            var result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(secretKeyByteArray, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        public static int WeekOfYear(this DateTime date, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
                return new PersianCalendar().GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);

            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) date = date.AddDays(3);

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfMonth(this DateTime dt, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
                return new MyPersianCalendar().ToDateTime(int.Parse(dt.ToString("yyyy")), int.Parse(dt.ToString("MM")), 1, 0, 0, 0, 0);

            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime[] GetPriodOfYear(this DateTime dt, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                var cal = new MyPersianCalendar();
                return new[]
                {
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")), 1, 1, 0, 0, 0, 0),
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")) + 1, 1, 1, 0, 0, 0, 0).AddMilliseconds(-1)
                };
            }

            return new[]
                {
                    new DateTime(dt.Year, 1, 1),
                    new DateTime(dt.Year + 1, 1, 1).AddMilliseconds(-1)
                };
        }

        public static DateTime[] GetPriodOfMonth(this DateTime dt, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                var cal = new MyPersianCalendar();
                return new[]
                {
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")),int.Parse(dt.ToString("MM")),1,0,0,0,0),
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")),int.Parse(dt.ToString("MM")), 1, 0, 0, 0, 0).AddMonths(1).AddDays(-1)
                };
            }

            return new[]
                {
                    new DateTime(dt.Year, dt.Month, 1),
                    new DateTime(dt.Year, dt.Month, 1).AddMonths(1).AddDays(-1)
                };
        }

        public static CultureInfo GetCulture(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new CultureInfo(Constants.DefaultLanguageCode);

            if (!name.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
                return new CultureInfo(name);

            var persianCalture = new CultureInfo(name);
            var info = persianCalture.DateTimeFormat;
            var monthNames = new[] { "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن", "اسفند", "" };
            var shortestDayNames = new[] { "ى", "د", "س", "چ", "پ", "ج", "ش" };
            var dayNames = new[] { "يکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };

            info.MonthGenitiveNames = monthNames;
            info.AbbreviatedMonthGenitiveNames = monthNames;
            info.AbbreviatedMonthNames = monthNames;
            info.MonthNames = monthNames;

            info.ShortestDayNames = shortestDayNames;
            info.AbbreviatedDayNames = shortestDayNames;

            info.DayNames = dayNames;

            info.DateSeparator = "/";
            info.FullDateTimePattern = "dddd dd MMM yyyy HH:mm:ss";
            info.LongDatePattern = "dddd dd MMM yyyy";
            info.LongTimePattern = "HH:mm:ss";
            info.MonthDayPattern = "dd MMM";
            info.ShortTimePattern = "HH:mm";
            info.TimeSeparator = ":";
            info.YearMonthPattern = "MMM yyyy";
            info.AMDesignator = "ق.ظ";
            info.PMDesignator = "ب.ظ";
            info.ShortDatePattern = "yyyy/MM/dd";
            info.FirstDayOfWeek = DayOfWeek.Saturday;
            persianCalture.DateTimeFormat = info;

            persianCalture.NumberFormat.NumberDecimalDigits = 0;
            persianCalture.NumberFormat.CurrencyDecimalDigits = 0;
            persianCalture.NumberFormat.PercentDecimalDigits = 0;
            persianCalture.NumberFormat.CurrencyPositivePattern = 1;

            var persianCal = new MyPersianCalendar();

            var fieldInfo = typeof(DateTimeFormatInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            fieldInfo?.SetValue(info, persianCal);

            var field = typeof(CultureInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            field?.SetValue(persianCalture, persianCal);

            return persianCalture;
        }

        public static string Sentencise(this string str, bool titlecase = false)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            var retVal = new StringBuilder(32);

            retVal.Append(char.ToUpper(str[0]));
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsLower(str[i]))
                {
                    retVal.Append(str[i]);
                }
                else
                {
                    retVal.Append(" ");
                    if (titlecase)
                    {
                        retVal.Append(str[i]);
                    }
                    else
                    {
                        retVal.Append(char.ToLower(str[i]));
                    }
                }
            }

            return retVal.ToString();
        }

        public static IEnumerable<Type> GetAllTypesImplementingType(this Type mainType, IEnumerable<Type> scanTypes)
        {
            IEnumerable<Type> types;

            if (mainType.IsGenericType)
            {
                types = from t1 in scanTypes
                        from t2 in t1.GetInterfaces()
                        let baseType = t1.BaseType
                        where !t1.IsAbstract &&
                        ((baseType != null && baseType.IsGenericType && mainType.IsAssignableFrom(baseType.GetGenericTypeDefinition())) ||
                        (t2.IsGenericType && mainType.IsAssignableFrom(t2.GetGenericTypeDefinition())))
                        select t1;
            }
            else if (mainType.IsInterface)
            {
                types = scanTypes.Where(t => !t.IsAbstract && t.GetInterfaces().Contains(mainType));
            }
            else
            {
                types = scanTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(mainType));
            }

            if (!types?.Any() == true && mainType.IsClass && !mainType.IsAbstract)
                types = new[] { mainType };

            return types;
        }

        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
                return input.Substring(0, input.Length - suffixToRemove.Length);

            return input;
        }

        public static string Slugify(this string value)
        {
            return value == null ? null : Regex.Replace(value,
                                 "([a-z])([A-Z])",
                                 "$1-$2",
                                 RegexOptions.CultureInvariant,
                                 TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
        }

        internal static RouteValueDictionary PrepareValues(object routeValues, string area = null)
        {
            var rootValueDictionary = new RouteValueDictionary(routeValues);
            if (!rootValueDictionary.ContainsKey(Constants.LanguageIdentifier))
                rootValueDictionary.Add(Constants.LanguageIdentifier, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            else
                rootValueDictionary[Constants.LanguageIdentifier] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (!string.IsNullOrEmpty(area))
            {
                if (!rootValueDictionary.ContainsKey(Constants.AreaIdentifier))
                    rootValueDictionary.Add(Constants.AreaIdentifier, area);
                else
                    rootValueDictionary[Constants.AreaIdentifier] = area;
            }

            return rootValueDictionary;
        }

        public static async Task<string> ConvertImageToBase64Async(IFormFile file)
        {
            if (file == null)
                return null;

            using var target = new System.IO.MemoryStream();
            await file.CopyToAsync(target);
            return $"data:{file.ContentType};base64, {Convert.ToBase64String(target.ToArray())}";
        }

        #region Inner Classes

        private class MyPersianCalendar : PersianCalendar
        {
            public override int GetYear(DateTime time)
            {
                try
                {
                    return base.GetYear(time);
                }
                catch
                {
                    return time.Year;
                }
            }

            public override int GetMonth(DateTime time)
            {
                try
                {
                    return base.GetMonth(time);
                }
                catch
                {
                    return time.Month;
                }
            }

            public override int GetDayOfMonth(DateTime time)
            {
                try
                {
                    return base.GetDayOfMonth(time);
                }
                catch
                {
                    return time.Day;
                }
            }

            public override int GetDayOfYear(DateTime time)
            {
                try
                {
                    return base.GetDayOfYear(time);
                }
                catch
                {
                    return time.DayOfYear;
                }
            }

            public override DayOfWeek GetDayOfWeek(DateTime time)
            {
                try
                {
                    return base.GetDayOfWeek(time);
                }
                catch
                {
                    return time.DayOfWeek;
                }
            }
        }

        #endregion
    }
}
