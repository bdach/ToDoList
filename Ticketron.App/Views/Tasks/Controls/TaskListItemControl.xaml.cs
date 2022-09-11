using System;
using System.Collections.Generic;
using System.ComponentModel;
using Humanizer;
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
                new PropertyMetadata(default(TaskViewModel?), ViewModelChanged));

        public TaskViewModel? ViewModel
        {
            get => (TaskViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        #endregion

        #region ShowTaskGroup

        public static readonly DependencyProperty ShowTaskGroupProperty =
            DependencyProperty.Register(
                nameof(ShowTaskGroup),
                typeof(bool),
                typeof(TaskListItemControl),
                new PropertyMetadata(default(bool), ShowTaskGroupChanged));

        public bool ShowTaskGroup
        {
            get => (bool)GetValue(ShowTaskGroupProperty);
            set => SetValue(ShowTaskGroupProperty, value);
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
        {
            if (ViewModel != null)
                TaskDeleted?.Invoke(this, new TaskDeletedEventArgs(ViewModel));
        }

        private static void ViewModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = (TaskListItemControl)sender;

            var oldValue = (TaskViewModel?)e.OldValue;
            var newValue = (TaskViewModel?)e.NewValue;

            if (oldValue == newValue) return;

            if (oldValue != null)
                oldValue.PropertyChanged -= ViewModelPropertyChanged;

            if (newValue != null)
                newValue.PropertyChanged += ViewModelPropertyChanged;

            itemControl.UpdateDescriptionText();

            void ViewModelPropertyChanged(object? _, PropertyChangedEventArgs __) => itemControl.UpdateDescriptionText();
        }

        private static void ShowTaskGroupChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = (TaskListItemControl)sender;

            if (itemControl.ViewModel != null)
                itemControl.UpdateDescriptionText();
        }

        private void UpdateDescriptionText()
        {
            var textFragments = new List<string>();

            if (ViewModel != null)
            {
                if (ShowTaskGroup)
                    textFragments.Add($"{ViewModel?.TaskGroup.Icon} {ViewModel?.TaskGroup.Name}");

                if (ViewModel?.ScheduledFor != null)
                    textFragments.Add(ViewModel.ScheduledFor.Value.Date == DateTime.Today ? "today" : ViewModel.ScheduledFor.Value.Humanize(dateToCompareAgainst: DateTime.UtcNow.Date));
            }

            var descriptionText = string.Join("  ●  ", textFragments);
            DetailsTextBlock.Text = descriptionText;
            DetailsTextBlock.Visibility =
                string.IsNullOrEmpty(descriptionText) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
