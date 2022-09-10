using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Ticketron.App.ViewModels;
using Ticketron.App.Views.Settings;
using Ticketron.App.Views.Tasks;

namespace Ticketron.App.Views
{
    public sealed partial class TaskGroupNavigationView : UserControl
    {
        public ObservableCollection<TaskGroupViewModel> TaskGroups { get; }

        public TaskGroupNavigationView()
        {
            TaskGroups = App.Current.State.TaskGroups;

            this.InitializeComponent();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                sender.Header = "Settings";
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            var selectedTaskGroup = (TaskGroupViewModel)sender.SelectedItem;
            sender.Header = $"{selectedTaskGroup.Icon} {selectedTaskGroup.Name}";
            ContentFrame.Navigate(typeof(TasksPage), selectedTaskGroup.Model.Id);
        }
    }
}
