using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Ticketron.App.Controls
{
    public sealed partial class ClickToEditControl : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(ClickToEditControl),
                new PropertyMetadata(string.Empty, null));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ClickToEditControl()
        {
            this.InitializeComponent();
        }

        private void EditFinished(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
