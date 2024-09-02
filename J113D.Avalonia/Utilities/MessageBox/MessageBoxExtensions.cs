using Avalonia.Controls;
using System.Threading.Tasks;

namespace J113D.Avalonia.MessageBox
{
    public static class MessageBoxExtensions
    {
        public static Task<MessageBoxResult?> MessageBoxDialog(this Window window, string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return new MessageBoxWindow(title, message, buttons, icon)
                .ShowDialog<MessageBoxResult?>(window);
        }
    }
}
