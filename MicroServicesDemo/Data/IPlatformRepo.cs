using MicroServicesDemo.Models;

namespace MicroServicesDemo.Data
{
    public interface IPlatformRepo
    {
        bool Savechanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);

    }
}
