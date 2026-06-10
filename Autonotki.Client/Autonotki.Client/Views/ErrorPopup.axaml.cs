using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Autonotki.Client.Views;

public partial class ErrorPopup : Window
{
    public ErrorPopup()
    {
        InitializeComponent();
    }

    public ErrorPopup(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
