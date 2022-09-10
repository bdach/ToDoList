using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Ticketron.App.ViewModels;
using Ticketron.App.Views.Settings.Controls;
using Ticketron.DB.Models;
using Ticketron.DB.Repositories;

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

        private void TaskGroupCreateRequested(object sender, TaskGroupEditControl.TaskGroupEditedEventArgs args)
        {
            TaskGroups.Add(new TaskGroupViewModel(args.EditResult));

            var editControl = (TaskGroupEditControl)sender;
            editControl.Reset();
        }

        private void TaskGroupDeleteRequested(object sender, TaskGroupListItem.TaskGroupDeletedEventArgs args)
        {
            var toDelete = TaskGroups.SingleOrDefault(group => group.Model.Id == args.DeletedGroup.Id);
            if (toDelete != null)
                TaskGroups.Remove(toDelete);
        }
    }
}
