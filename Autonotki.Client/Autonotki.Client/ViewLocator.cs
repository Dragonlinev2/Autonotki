using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Autonotki.Client.ViewModels;

namespace Autonotki.Client;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null) return null;

        var name = param.GetType().FullName!
            .Replace("Autonotki.Client.ViewModels", "Autonotki.Client.Views")
            .Replace("ViewModel", "View");

        var type = Type.GetType(name);
        if (type != null)
            return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Widok nie znaleziony: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
