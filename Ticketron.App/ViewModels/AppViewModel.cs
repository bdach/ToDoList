using System.Collections.ObjectModel;

namespace Ticketron.App.ViewModels;

public class AppViewModel
{
    /// <summary>
    /// Contains all available task groups.
    /// </summary>
    public ObservableCollection<TaskGroupViewModel> TaskGroups { get; } = new ObservableCollection<TaskGroupViewModel>();
}