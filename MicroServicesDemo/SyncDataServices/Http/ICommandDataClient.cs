using MicroServicesDemo.DTOs;

namespace MicroServicesDemo.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
