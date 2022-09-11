using System;
using Windows.System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Ticketron.App.Persistence;
using Ticketron.App.ViewModels;
using Ticketron.App.Views.Tasks.Controls;

namespace Ticketron.App.Views.Tasks
{
    public sealed partial class TodayPage : Page
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TasksForTodayViewModel),
                typeof(TodayPage),
                new PropertyMetadata(default(TasksForTodayViewModel)));

        public TasksForTodayViewModel ViewModel
        {
            get => (TasksForTodayViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public TodayPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var persistenceManager = App.Current.Services.GetRequiredService<TasksForTodayPersistenceManager>();
            ViewModel = await persistenceManager.LoadState();
        }

        private async void OnTaskEntryKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || string.IsNullOrEmpty(TaskEntryTextBox.Text))
                return;

            var newTask = new TaskViewModel(TaskGroupSelector.SelectedTaskGroup)
            {
                Title = TaskEntryTextBox.Text,
                ScheduledFor = DateTime.Today
            };
            await ViewModel.AddTask(newTask);

            TaskEntryTextBox.Text = string.Empty;
        }

        private async void TaskDeleteRequested(object sender, TaskListItemControl.TaskDeletedEventArgs e)
            => await ViewModel.DeleteTask(e.DeletedTask);
    }
}
