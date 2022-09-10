using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Ticketron.App.ViewModels;
using Ticketron.App.Views.Settings.Controls;

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

        private void TaskGroupCreated(object sender, TaskGroupEditControl.TaskGroupEditedEventArgs args)
        {
            TaskGroups.Add(new TaskGroupViewModel(args.EditResult));

            var editControl = (TaskGroupEditControl)sender;
            editControl.Reset();
        }

        private void TaskGroupDeleted(object sender, TaskGroupListItem.TaskGroupDeletedEventArgs args)
        {
            var toDelete = TaskGroups.SingleOrDefault(group => group.Model.Id == args.DeletedGroup.Id);
            if (toDelete != null)
                TaskGroups.Remove(toDelete);
        }
    }
}
