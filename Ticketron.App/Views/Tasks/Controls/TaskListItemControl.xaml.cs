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
                new PropertyMetadata(default(bool), VisualDependencyPropertyChanged));

        public bool ShowTaskGroup
        {
            get => (bool)GetValue(ShowTaskGroupProperty);
            set => SetValue(ShowTaskGroupProperty, value);
        }

        #endregion

        #region CanStart

        public static readonly DependencyProperty CanStartProperty =
            DependencyProperty.Register(
                nameof(CanStart),
                typeof(bool),
                typeof(TaskListItemControl),
                new PropertyMetadata(default(bool)));

        public bool CanStart
        {
            get => (bool)GetValue(CanStartProperty);
            private set => SetValue(CanStartProperty, value);
        }

        #endregion

        #region CanStop

        public static readonly DependencyProperty CanStopProperty =
            DependencyProperty.Register(
                nameof(CanStop),
                typeof(bool),
                typeof(TaskListItemControl),
                new PropertyMetadata(default(bool), VisualDependencyPropertyChanged));

        public bool CanStop
        {
            get => (bool)GetValue(CanStopProperty);
            private set => SetValue(CanStopProperty, value);
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
            itemControl.UpdateTaskTrackingState(App.Current.State);

            void ViewModelPropertyChanged(object? _, PropertyChangedEventArgs __) => itemControl.UpdateDescriptionText();
        }

        #region Command bar visual state

        private void OnPointerEntered(object _, PointerRoutedEventArgs __) => ToggleActionBar(true);
        private void OnPointerExited(object _, PointerRoutedEventArgs __) => ToggleActionBar(false);

        private void ToggleActionBar(bool visible)
            => ActionCommandBar.Opacity = visible ? 1 : 0;

        #endregion

        #region Command bar task deletion

        private void TaskDeleteRequested(XamlUICommand _, ExecuteRequestedEventArgs __)
        {
            if (ViewModel != null)
                TaskDeleted?.Invoke(this, new TaskDeletedEventArgs(ViewModel));
        }

        #endregion

        #region Updating description text

        private static void VisualDependencyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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
                    textFragments.Add(ViewModel.ScheduledFor.Value.Date == DateTime.Today ? "today" : ViewModel.ScheduledFor.Value.Humanize(dateToCompareAgainst: DateTime.UtcNow.Date).Titleize());

                if (CanStop)
                    textFragments.Add("Currently in progress");
            }

            var descriptionText = string.Join("  ●  ", textFragments);
            DetailsTextBlock.Text = descriptionText;
            DetailsTextBlock.Visibility =
                string.IsNullOrEmpty(descriptionText) ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion

        #region Time tracking

        private void ControlLoaded(object _, RoutedEventArgs __)
            => App.Current.State.PropertyChanged += OnAppStateChanged;

        private void ControlUnloaded(object _, RoutedEventArgs __)
            => App.Current.State.PropertyChanged -= OnAppStateChanged;

        private void OnAppStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not AppViewModel appState || e.PropertyName != nameof(AppViewModel.CurrentLogEntry))
                return;

            UpdateTaskTrackingState(appState);
        }

        private void UpdateTaskTrackingState(AppViewModel appState)
        {
            CanStart = appState.CurrentLogEntry == null;
            CanStop = appState.CurrentLogEntry?.Task.Model.Id == ViewModel?.Model.Id;
        }

        private async void TaskStartRequested(XamlUICommand _, ExecuteRequestedEventArgs __)
        {
            if (ViewModel == null) return;
            await App.Current.State.StartWorkingOnAsync(ViewModel);
        }

        private async void TaskStopRequested(XamlUICommand _, ExecuteRequestedEventArgs __)
        {
            if (ViewModel == null) return;
            if (ViewModel != App.Current.State.CurrentLogEntry?.Task)
                throw new InvalidOperationException(
                    "Catastrophic failure: attempted to stop task which is not in progress.");

            await App.Current.State.EndWorkingOnCurrentTaskAsync();
        }

        #endregion
    }
}
