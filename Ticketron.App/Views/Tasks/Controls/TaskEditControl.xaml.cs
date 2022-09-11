using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Ticketron.App.ViewModels;

namespace Ticketron.App.Views.Tasks.Controls
{
    public sealed partial class TaskEditControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TaskViewModel),
                typeof(TaskEditControl),
                new PropertyMetadata(default(TaskViewModel?)));

        public TaskViewModel? ViewModel
        {
            get => (TaskViewModel?)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public TaskEditControl()
        {
            this.InitializeComponent();
        }
    }
}
