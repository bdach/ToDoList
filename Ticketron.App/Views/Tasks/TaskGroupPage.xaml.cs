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
    public sealed partial class TaskGroupPage : Page
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TaskGroupPageViewModel),
                typeof(TaskGroupPage),
                new PropertyMetadata(default(TaskGroupPageViewModel)));

        public TaskGroupPageViewModel ViewModel
        {
            get => (TaskGroupPageViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public TaskGroupPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var taskGroupId = (int)e.Parameter;
            var persistenceManager = App.Current.Services.GetRequiredService<TaskGroupPagePersistenceManager>();
            ViewModel = await persistenceManager.LoadState(taskGroupId);

            TasksCollectionViewSource.Source = ViewModel.GroupedTasks;
        }

        private async void OnTaskEntryKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || string.IsNullOrEmpty(TaskEntryTextBox.Text))
                return;

            var newTask = new TaskViewModel(ViewModel.Group) { Title = TaskEntryTextBox.Text };
            await ViewModel.AddTask(newTask);

            TaskEntryTextBox.Text = string.Empty;
        }

        private async void TaskDeleteRequested(object sender, TaskListItemControl.TaskDeletedEventArgs e)
            => await ViewModel.DeleteTask(e.DeletedTask);

        private void SelectedTaskChanged(object sender, SelectionChangedEventArgs e)
            => SplitView.IsPaneOpen = TaskListView.SelectedItem is TaskViewModel;
    }
}
