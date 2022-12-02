using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToDoList.App.ViewModels;
using ToDoList.DB.Models;

namespace ToDoList.App.Views.Settings.Controls
{
    public sealed partial class TaskGroupEditControl : UserControl
    {
        #region ViewModel

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TaskGroupViewModel),
                typeof(TaskGroupEditControl),
                new PropertyMetadata(new TaskGroupViewModel()));

        public TaskGroupViewModel ViewModel
        {
            get => (TaskGroupViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        #endregion

        #region Mode

        public enum TaskGroupEditMode
        {
            Add,
            Edit,
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                nameof(Mode),
                typeof(TaskGroupEditMode),
                typeof(TaskGroupEditControl),
                new PropertyMetadata(default(TaskGroupEditMode), ModeChanged));

        public TaskGroupEditMode Mode
        {
            get => (TaskGroupEditMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        #endregion

        #region TaskGroupEditCompleted

        public class TaskGroupEditedEventArgs : EventArgs
        {
            public TaskGroup EditResult { get; }

            public TaskGroupEditedEventArgs(TaskGroup editResult)
            {
                EditResult = editResult;
            }
        }

        public delegate void TaskGroupEditCompletedEventHandler(object sender, TaskGroupEditedEventArgs args);

        public event TaskGroupEditCompletedEventHandler? TaskGroupEditCompleted;

        #endregion

        public TaskGroupEditControl()
        {
            this.InitializeComponent();
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var taskGroupEditControl = (TaskGroupEditControl)d;

            switch (e.NewValue)
            {
                case TaskGroupEditMode.Add:
                    taskGroupEditControl.ConfirmButton.Icon = new SymbolIcon(Symbol.Add);
                    taskGroupEditControl.ConfirmButton.Label = "Add";
                    break;

                case TaskGroupEditMode.Edit:
                    taskGroupEditControl.ConfirmButton.Icon = new SymbolIcon(Symbol.Accept);
                    taskGroupEditControl.ConfirmButton.Label = "Done";
                    break;
            }
        }

        private void ConfirmButtonClicked(object _, RoutedEventArgs __)
        {
            TaskGroupEditCompleted?.Invoke(this, new TaskGroupEditedEventArgs(ViewModel.Model));
        }

        public void Reset()
        {
            ViewModel = new TaskGroupViewModel();
        }
    }
}
