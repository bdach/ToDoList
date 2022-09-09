using System.Collections.ObjectModel;

namespace Ticketron.App.ViewModels;

public class TaskGroupNavigationViewModel
{
    public ObservableCollection<TaskGroupViewModel> TaskGroups { get; } = new ObservableCollection<TaskGroupViewModel>();
}