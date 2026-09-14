using System.Diagnostics.CodeAnalysis;

namespace ZFlex.Extensions
{
    public static class ArgsExtensions
    {
        public static T? ExtractParamValue<T>(this string[] args, string paramName, IFormatProvider? formatProvider = null) where T : IParsable<T>
        {
            var paramIndex = args.IndexOf($"-{paramName}");
            if (paramIndex != -1 && args.Length - 1 >= paramIndex + 1) 
            {
                var paramStringValue = args[paramIndex + 1];

                if (T.TryParse(paramStringValue, formatProvider, out var result))
                    return result;
            }

            return default;
        }

        public static bool ExtractParamValue<T>(this string[] args, string paramName, [MaybeNullWhen(returnValue: false)] out T value, IFormatProvider? formatProvider = null) where T : IParsable<T>
        {
            var parsedValue = args.ExtractParamValue<T>(paramName, formatProvider);
            var hasResult = parsedValue != null;

            value = hasResult ? parsedValue : default;

            return hasResult;
        }

        public static DateOnly GetReservationDate(this string[] args, int defaultReservationDayUntilNow = 1)
            => args switch
            {
                var argsArr when argsArr.ExtractParamValue("rd", out int parsedDayNum) => DateOnly.FromDateTime(DateTime.Now.AddDays(parsedDayNum)),
                var argsArr when argsArr.ExtractParamValue("d", out DateOnly parsedDate) => parsedDate,
                _ => DateOnly.FromDateTime(DateTime.Now.AddDays(defaultReservationDayUntilNow))
            };

        public static bool HasParam(this string[] args, string paramName)
            => args.Contains($"-{paramName}");
    }
}
