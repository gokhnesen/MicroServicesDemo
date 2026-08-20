using CommandsService.Models;

namespace SyncDataServices.Grpc;

    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
