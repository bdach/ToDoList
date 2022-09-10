using System;
using Humanizer;
using Microsoft.UI.Xaml.Data;

namespace Ticketron.App.Utilities;

public class DateToHumanisedStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, string language)
    {
        if (targetType != typeof(string))
            throw new InvalidOperationException(
                $"{nameof(DateToHumanisedStringConverter)} can only be used to convert from {nameof(DateTime)} to strings.");

        if (value is not DateTime dateTime)
            return string.Empty;

        if (dateTime.Date == DateTime.UtcNow.Date)
            return "today";

        return dateTime.Date.Humanize(dateToCompareAgainst: DateTime.UtcNow.Date);
    }

    public object? ConvertBack(object? value, Type targetType, object parameter, string language)
        => throw new NotSupportedException($"{nameof(DateToHumanisedStringConverter)} is a lossy conversion.");
}