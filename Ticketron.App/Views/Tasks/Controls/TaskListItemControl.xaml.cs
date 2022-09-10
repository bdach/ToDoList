using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Ticketron.App.ViewModels;

namespace Ticketron.App.Views.Tasks.Controls
{
    public sealed partial class TaskListItemControl : UserControl
    {
        #region ViewModel

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TaskViewModel),
                typeof(TaskListItemControl),
                new PropertyMetadata(default(TaskViewModel)));

        public TaskViewModel ViewModel
        {
            get => (TaskViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        #endregion

        #region TaskDeleted

        public class TaskDeletedEventArgs : EventArgs
        {
            public TaskViewModel DeletedTask { get; }

            public TaskDeletedEventArgs(TaskViewModel deletedTask)
            {
                DeletedTask = deletedTask;
            }
        }

        public delegate void TaskDeletedEventHandler(object sender, TaskDeletedEventArgs e);

        public event TaskDeletedEventHandler? TaskDeleted;

        #endregion

        public TaskListItemControl()
        {
            this.InitializeComponent();
        }

        private void OnPointerEntered(object _, PointerRoutedEventArgs __) => ToggleActionBar(true);
        private void OnPointerExited(object _, PointerRoutedEventArgs __) => ToggleActionBar(false);

        private void ToggleActionBar(bool visible)
            => ActionCommandBar.Opacity = visible ? 1 : 0;

        private void TaskDeleteRequested(XamlUICommand _, ExecuteRequestedEventArgs __)
            => TaskDeleted?.Invoke(this, new TaskDeletedEventArgs(ViewModel));

        private Visibility DateTimeVisibility(DateTime? dateTime) => dateTime.HasValue ? Visibility.Visible : Visibility.Collapsed;
    }
}
