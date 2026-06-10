using System;
using Avalonia.Controls;
using Avalonia.Media;

namespace PopUp;

public partial class MainWindow : Window
{
    private string _icon;
    private string _message;
    private IBrush _background;
    private string _button;
    
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(string message = "Uwaga", string icon = "⚠", string button = "OK")
    {
        InitializeComponent();
    }

    public void SetComponent()
    {
        SetIcon(_icon);
        SetMessage(_message);
        SetStyle(_background);
        Title = "Uwaga!";
    }

    public void SetMessage(string text)
    {
        _message = text;
        MessageText.Text = _message;
    }

    public void SetIcon(string icon)
    {
        _icon = icon;
        IconText.Text = _icon;
    }

    public void SetStyle(IBrush background)
    {
        _background = background;
        Root.Background = _background;
    }

    public void AddButton(string text)
    {
        _button = text;
        var btn = new Button
        {
            Content = _button,
            Margin = new Avalonia.Thickness(10)
        };

        btn.Click += (_, _) => Close();

        ButtonsPanel.Children.Add(btn);
    }
}