using Avalonia.Controls;
using Avalonia.Interactivity;

namespace J113D.Avalonia.MessageBox
{
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxWindow() { }

        public MessageBoxWindow(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            InitializeComponent();
            Classes.Add(buttons.ToString());
            Classes.Add(icon.ToString());
            Title = title;
            MessageText.Text = message;
        }

        private void OnOk(object? sender, RoutedEventArgs e)
        {
            Close(MessageBoxResult.Ok);
        }

        private void OnYes(object? sender, RoutedEventArgs e)
        {
            Close(MessageBoxResult.Yes);
        }

        private void OnNo(object? sender, RoutedEventArgs e)
        {
            Close(MessageBoxResult.No);
        }

        private void OnCancel(object? sender, RoutedEventArgs e)
        {
            Close(MessageBoxResult.Cancel);
        }

        private void OnAbort(object? sender, RoutedEventArgs e)
        {
            Close(MessageBoxResult.Abort);
        }
    }
}