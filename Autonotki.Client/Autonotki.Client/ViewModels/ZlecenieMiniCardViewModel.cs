using Autonotki.Application.DTOs;

namespace Autonotki.Client.ViewModels;

public class ZlecenieMiniCardViewModel
{
    public string CarName { get; }
    public string? ServiceType { get; }
    public string? TimeDisplay { get; }
    public string? ClientName { get; }

    public ZlecenieMiniCardViewModel(ZlecenieDto dto)
    {
        CarName     = dto.CarName;
        ServiceType = dto.ServiceType;
        TimeDisplay = dto.DeadlineDisplay is not null ? $"godz. {dto.DeadlineDisplay}" : "";
        ClientName  = dto.ClientName;
    }
}
