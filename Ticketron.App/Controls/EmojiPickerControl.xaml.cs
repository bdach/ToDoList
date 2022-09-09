using Windows.UI.ViewManagement.Core;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Ticketron.App.Utilities;


namespace Ticketron.App.Controls
{
    public sealed partial class EmojiPickerControl : UserControl
    {
        public static readonly DependencyProperty EmojiProperty =
            DependencyProperty.Register(
                nameof(Emoji),
                typeof(string),
                typeof(EmojiPickerControl),
                new PropertyMetadata(string.Empty, null));

        public string Emoji
        {
            get => (string)GetValue(EmojiProperty);
            set => SetValue(EmojiProperty, value);
        }

        public EmojiPickerControl()
        {
            this.InitializeComponent();
        }

        private void EditRequested(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Collapsed;
            EntryTextBox.Visibility = Visibility.Visible;

            EntryTextBox.Focus(FocusState.Programmatic);
        }

        private void EditResumed(object sender, RoutedEventArgs e)
        {
            CoreInputView.GetForCurrentView().TryShow(CoreInputViewKind.Emoji);
            EntryTextBox.Text = string.Empty;
        }

        private void EmojiChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryTextBox.Text))
                return;

            EntryTextBox.BorderThickness = IsEditingValidEmoji ? new Thickness(0) : new Thickness(1);
            EntryTextBox.BorderBrush = IsEditingValidEmoji
                ? new SolidColorBrush(Colors.Transparent)
                : new SolidColorBrush(Colors.Red);

            FinishEdit();
        }

        private void EditFinished(object sender, RoutedEventArgs e)
        {
            FinishEdit();
        }

        private void FinishEdit()
        {
            if (IsEditingValidEmoji)
                Emoji = EntryTextBox.Text;
            else
                EntryTextBox.Text = Emoji;

            EditButton.Visibility = Visibility.Visible;
            EntryTextBox.Visibility = Visibility.Collapsed;
        }

        private bool IsEditingValidEmoji => EmojiCollection.All.Contains(EntryTextBox.Text);
    }
}
