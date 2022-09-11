using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Ticketron.App.ViewModels;

namespace Ticketron.App.Views.Tasks.Controls
{
    [ContentProperty(Name = nameof(MasterContent))]
    public sealed partial class TaskMasterDetailView : UserControl
    {
        #region SelectedTask

        public static readonly DependencyProperty SelectedTaskProperty
            = DependencyProperty.Register(
                nameof(SelectedTask),
                typeof(TaskViewModel),
                typeof(TaskMasterDetailView),
                new PropertyMetadata(default(TaskViewModel?)));

        public TaskViewModel? SelectedTask
        {
            get => (TaskViewModel?)GetValue(SelectedTaskProperty);
            set => SetValue(SelectedTaskProperty, value);
        }

        #endregion

        #region MasterContent

        public static readonly DependencyProperty MasterContentProperty = DependencyProperty.Register(
            nameof(MasterContent), typeof(object), typeof(TaskMasterDetailView), new PropertyMetadata(default));

        public object? MasterContent
        {
            get => GetValue(MasterContentProperty);
            set => SetValue(MasterContentProperty, value);
        }

        #endregion

        public TaskMasterDetailView()
        {
            this.InitializeComponent();
        }

        private bool ShouldPaneBeOpen(TaskViewModel? selectedTask) => selectedTask != null;
    }
}
