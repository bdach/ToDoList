using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Ticketron.App.ViewModels;

namespace Ticketron.App.Views.Tasks.Controls
{
    public sealed partial class TaskGroupDropdown : UserControl
    {
        public ReadOnlyObservableCollection<TaskGroupViewModel> TaskGroups { get; }

        public static readonly DependencyProperty SelectedTaskGroupProperty =
            DependencyProperty.Register(
                nameof(SelectedTaskGroup),
                typeof(TaskGroupViewModel),
                typeof(TaskGroupDropdown),
                new PropertyMetadata(default(TaskGroupViewModel)));

        public TaskGroupViewModel SelectedTaskGroup
        {
            get => (TaskGroupViewModel)GetValue(SelectedTaskGroupProperty);
            set => SetValue(SelectedTaskGroupProperty, value);
        }

        public static readonly DependencyProperty CompactProperty =
            DependencyProperty.Register(
                nameof(Compact),
                typeof(bool),
                typeof(TaskGroupDropdown),
                new PropertyMetadata(default(bool)));

        public bool Compact
        {
            get => (bool)GetValue(CompactProperty);
            set => SetValue(CompactProperty, value);
        }

        public TaskGroupDropdown()
        {
            this.InitializeComponent();

            TaskGroups = new ReadOnlyObservableCollection<TaskGroupViewModel>(App.Current.State.TaskGroups);
            SelectedTaskGroup = TaskGroups.First();
        }

        public string SelectDisplayMember(bool compact)
            => compact ? nameof(TaskGroupViewModel.Icon) : nameof(TaskGroupViewModel.Name);
    }
}
