using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using ToDoList.App.ViewModels;
using ToDoList.App.Views.Settings.Controls;
using ToDoList.DB.Models;
using ToDoList.DB.Repositories;

namespace ToDoList.App.Views.Settings
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
