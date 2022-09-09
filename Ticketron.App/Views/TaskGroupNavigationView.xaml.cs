using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Ticketron.App.ViewModels;
using Ticketron.DB.Repositories;

namespace Ticketron.App.Views
{
    public sealed partial class TaskGroupNavigationView : UserControl
    {
        private readonly ITaskGroupRepository _taskGroupRepository;

        public TaskGroupNavigationView()
        {
            this.InitializeComponent();

            _taskGroupRepository = App.Current.Services.GetRequiredService<ITaskGroupRepository>();
        }

        private async void OnNavigationViewLoaded(object _, RoutedEventArgs __)
        {
            var taskGroups = await _taskGroupRepository.GetAllAsync();

            foreach (var taskGroup in taskGroups)
                ViewModel.TaskGroups.Add(new TaskGroupViewModel(taskGroup));
        }
    }
}
