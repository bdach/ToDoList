using System;
using Microsoft.UI.Xaml.Data;

namespace Ticketron.App.Utilities;

public class DateTimeToDateTimeOffsetConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value == null && targetType == typeof(DateTimeOffset?))
            return null;

        if (value is DateTime sourceDate && (targetType == typeof(DateTimeOffset) || targetType == typeof(DateTimeOffset?)))
            return new DateTimeOffset(sourceDate);

        throw new InvalidOperationException(
            $"{nameof(DateTimeToDateTimeOffsetConverter)} can only be used to convert between {nameof(DateTime)} and {nameof(DateTimeOffset)}");
    }

    public object? ConvertBack(object? value, Type targetType, object parameter, string language)
    {
        if (value == null && targetType == typeof(DateTime?))
            return null;

        if (value is DateTimeOffset sourceDateTimeOffset && (targetType == typeof(DateTime) || targetType == typeof(DateTime?)))
            return sourceDateTimeOffset.UtcDateTime;

        throw new InvalidOperationException(
            $"{nameof(DateTimeToDateTimeOffsetConverter)} can only be used to convert between {nameof(DateTime)} and {nameof(DateTimeOffset)}");
    }
}