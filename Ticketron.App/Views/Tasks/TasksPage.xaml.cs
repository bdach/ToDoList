using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Ticketron.App.Persistence;
using Ticketron.App.ViewModels;

namespace Ticketron.App.Views.Tasks
{
    public sealed partial class TasksPage : Page
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TaskListViewModel),
                typeof(TasksPage),
                new PropertyMetadata(default(TaskListViewModel)));

        public TaskListViewModel ViewModel
        {
            get => (TaskListViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private readonly TaskListPersistenceManager _persistenceManager;

        public TasksPage()
        {
            this.InitializeComponent();

            _persistenceManager = App.Current.Services.GetRequiredService<TaskListPersistenceManager>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var taskGroupId = (int)e.Parameter;
            ViewModel = await _persistenceManager.LoadState(taskGroupId);

            TasksCollectionViewSource.Source = ViewModel.GroupedTasks;
        }
    }
}
