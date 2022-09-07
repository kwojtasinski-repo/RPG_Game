using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPG_GAME.IntegrationTests.Common.DataInitializers
{
    internal sealed class DataInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var heroRepository = scope.ServiceProvider.GetRequiredService<IHeroRepository>();
            var heroes = SharedData.GetHeroes();

            foreach(var hero in heroes)
            {
                await heroRepository.AddAsync(hero);
            }

            var enemyRepository = scope.ServiceProvider.GetRequiredService<IEnemyRepository>();
            var enemies = SharedData.GetEnemies();
            foreach(var enemy in enemies)
            {
                await enemyRepository.AddAsync(enemy);
            }

            var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
            var players = SharedData.GetPlayers();
            foreach(var player in players)
            {
                await playerRepository.AddAsync(player);
            }

            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var users = SharedData.GetUsers();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
            foreach (var user in users)
            {
                user.ChangePassword(passwordHasher.HashPassword(default, user.Password));
                await userRepository.AddAsync(user);
            }

            var mapRepository = scope.ServiceProvider.GetRequiredService<IMapRepository>();
            var maps = SharedData.GetMaps();
            foreach(var map in maps)
            {
                await mapRepository.AddAsync(map);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
