using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using ToDoList.App.ViewModels;
using ToDoList.DB.Models;

namespace ToDoList.App.Views.Settings.Controls
{
    public sealed partial class TaskGroupListItem : UserControl
    {
        #region ViewModel

        public static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TaskGroupViewModel),
                typeof(TaskGroupListItem),
                new PropertyMetadata(new TaskGroupViewModel()));

        public TaskGroupViewModel ViewModel
        {
            get => (TaskGroupViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        #endregion

        #region TaskGroupDeleted

        public class TaskGroupDeletedEventArgs : EventArgs
        {
            public TaskGroup DeletedGroup { get; }

            public TaskGroupDeletedEventArgs(TaskGroup deleted)
            {
                DeletedGroup = deleted;
            }
        }

        public delegate void TaskGroupDeletedEventHandler(object sender, TaskGroupDeletedEventArgs args);

        public event TaskGroupDeletedEventHandler? TaskGroupDeleted;

        #endregion

        public TaskGroupListItem()
        {
            this.InitializeComponent();
        }

        private void TaskGroupEditRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            ViewControl.Visibility = Visibility.Collapsed;
            EditControl.Visibility = Visibility.Visible;
        }

        private void TaskGroupEditCompleted(object sender, TaskGroupEditControl.TaskGroupEditedEventArgs args)
        {
            ViewControl.Visibility = Visibility.Visible;
            EditControl.Visibility = Visibility.Collapsed;
        }

        private void TaskGroupDeleteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            TaskGroupDeleted?.Invoke(this, new TaskGroupDeletedEventArgs(ViewModel.Model));
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e) => ToggleActionBar(true);
        private void OnPointerExited(object sender, PointerRoutedEventArgs e) => ToggleActionBar(false);

        private void ToggleActionBar(bool visible)
            => ActionCommandBar.Opacity = visible ? 1 : 0;
    }
}
