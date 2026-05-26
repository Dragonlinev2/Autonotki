using CommunityToolkit.Mvvm.ComponentModel;

namespace Autonotki.Client.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
	// Expose self as `FormVM` so views that expect a nested FormVM work
	public ViewModelBase FormVM => this;
}
