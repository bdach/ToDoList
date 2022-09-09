using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Ticketron.App.ViewModels;

namespace Ticketron.App.Views.Settings
{

    public sealed partial class TaskGroupsSection : UserControl
    {
        public ObservableCollection<TaskGroupViewModel> TaskGroups { get; }

        public TaskGroupsSection()
        {
            TaskGroups = App.Current.State.TaskGroups;

            this.InitializeComponent();
        }
    }
}
