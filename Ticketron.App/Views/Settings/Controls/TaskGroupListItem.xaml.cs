using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Ticketron.App.ViewModels;
using Ticketron.DB.Models;

namespace Ticketron.App.Views.Settings.Controls
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
    }
}
