using CommandsService.Models;
using SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrebDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                if (grpcClient == null)
                {
                    Console.WriteLine("--> Could not resolve IPlatformDataClient");
                    return;
                }

                var platforms = grpcClient.ReturnAllPlatforms();
                var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
                if (repo == null)
                {
                    Console.WriteLine("--> Could not resolve ICommandRepo");
                    return;
                }

                SeedData(repo, platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform>? platforms)
        {
            if (platforms == null || !platforms.Any())
            {
                Console.WriteLine("--> No platforms returned from gRPC service, skipping seed");
                return;
            }

            Console.WriteLine("--> Seeding new platforms");

            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
            }

            repo.SaveChanges();
        }
    }
}
