using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Ticketron.App.ViewModels;
using Ticketron.App.Views.Settings;

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
            }

            // TODO: Later.
        }
    }
}
