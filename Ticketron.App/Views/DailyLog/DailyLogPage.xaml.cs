using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Ticketron.DB.Models;
using Ticketron.DB.Repositories;

namespace Ticketron.App.Views.DailyLog
{
    public sealed partial class DailyLogPage : Page
    {
        private readonly ITaskLogRepository _taskLogRepository;

        public DailyLogPage()
        {
            this.InitializeComponent();

            _taskLogRepository = App.Current.Services.GetRequiredService<ITaskLogRepository>();
        }

        private async System.Threading.Tasks.Task FetchLogAsync(DateTime date)
        {
            var dailyLog = await _taskLogRepository.GetDailyLogAsync(date);
            LogTreeView.ItemsSource = dailyLog.TaskProgress;
        }

        private async void LogDateChanged(object? _, DatePickerValueChangedEventArgs e)
            => await FetchLogAsync(e.NewDate.Date);

        private async void OnLoaded(object _, RoutedEventArgs __)
            => await FetchLogAsync(LogDatePicker.Date.Date);
    }

    internal class DailyLogItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TaskTemplate { get; set; }
        public DataTemplate? LogEntryTemplate { get; set; }

        protected override DataTemplate? SelectTemplateCore(object item) => item switch
        {
            DailyTaskProgress => TaskTemplate,
            TaskLogEntry => LogEntryTemplate,
            _ => throw new ArgumentException($"Unsupported item type {item.GetType()}", nameof(item))
        };
    }
}
