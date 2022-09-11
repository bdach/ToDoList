using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Ticketron.App.Persistence;
using Ticketron.App.ViewModels;

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

            //TasksForTodayCollectionViewSource.Source = ViewModel.GroupedTasks;
        }
    }
}
