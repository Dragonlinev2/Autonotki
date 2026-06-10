using System.Runtime.InteropServices;
using Avalonia.Media;
using Avalonia;
using PopUp;

class Program
{
    static async Task Main()
    {
        var ready = new TaskCompletionSource();
        
        var appTask = Task.Run(() =>
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime([]);

            ready.SetResult();
        });
        await appTask;
    }
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
