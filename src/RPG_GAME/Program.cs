using System;
using RPG_GAME.Helpers;
using RPG_GAME.Core.Entity;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using RPG_GAME.Service.Concrete;
using RPG_GAME.Service.Managers;
using System.Threading.Tasks;
using Grpc.Net.Client;
using RPG_GAME.Protos;

namespace RPG_GAME
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var baseUri = "http://localhost:50000";
            using var channel = GrpcChannel.ForAddress(baseUri);
            var client = new Battle.BattleClient(channel);
            var mapId = "2c70e345-024f-4b12-b1ff-11f33ff40fdb";
            var userId = "68945a59-bb5f-48d0-a855-4a8b679c8e74";

            var response = await client.PrepareBattleAsync(new BattleRequest { MapId = mapId, UserId = userId });

            Console.WriteLine(response.ToString());
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
    
    class ProgramOld
    {
        static void OldMain(string[] args)
        {
            MenuActionService menuActionService = new MenuActionService();
            HeroService heroService = new HeroService();
            HeroManager heroManager = new HeroManager(menuActionService, heroService);
            EnemyService enemyService = new EnemyService();
            StoryManager storyManager;
            int tryHard = 0;
            bool choseLvlOne = false;
            bool choseLvlTwo = false;
            bool choseLvlThree = false;
            int lastDiffLvl = 0;
            List<Hero> heroes = null;

            Console.WriteLine("Welcome to RPG Game app!");
            while (true)
            {
                int exit = 0;
                Console.WriteLine("Please let me know what you want to do:");
                var mainMenu = menuActionService.GetMenuActionsByMenuName("Main");
                for (int i = 0; i < mainMenu.Count; i++)
                {
                    Console.WriteLine($"{mainMenu[i].Id}. {mainMenu[i].Name}");
                }

                var operation = Console.ReadKey();

                switch (operation.KeyChar)
                {
                    case '1': 
                        var newId = heroManager.AddNewHero();
                        break;
                    case '2':
                        heroManager.RemoveHero();
                        break;
                    case '3': 
                        heroManager.HeroDetails();
                        break;
                    case '4':
                        var toShow = heroManager.ShowHeroes();
                        Console.WriteLine(toShow.ToStringTable(new[] { "Id", "Name", "Health", "Max Health",
                            "Attack", "Heal Level", "Experience", "Level", "Required Experience",
                            "Profession", "Typ Id" }, a => a.Id, a => a.Name, a => a.Health, a => a.MaxHealth,
                            a => a.Attack, a => a.HealLvl, a => a.Exp, a => a.Level, a => a.RequiredExp,
                            a => a.Profession, a => a.TypeId));
                        break;
                    case '5':
                        heroes = heroManager.GetAllHeroes();
                        if (heroes.Count > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine(heroes.ToStringTable(new[] { "Id", "Name", "Health", "Max Health",
                            "Attack", "Heal Level", "Experience", "Level", "Required Experience",
                            "Profession", "Typ Id" }, a => a.Id, a => a.Name, a => a.Health, a => a.MaxHealth,
                            a => a.Attack, a => a.HealLvl, a => a.Exp, a => a.Level, a => a.RequiredExp,
                            a => a.Profession, a => a.TypeId));
                            int chosenHero = -1;

                            Hero hero = null;
                            while (hero == null)
                            {
                                chosenHero = heroManager.SelectCharacter();
                                
                                if (chosenHero == -1)
                                    break;

                                hero = heroManager.GetHeroById(chosenHero);
                            }

                            if (chosenHero != -1)
                            {
                                Console.WriteLine($"You chose Hero {hero.Name}, level:{hero.Level}, profession:{hero.Profession}");
                                storyManager = new StoryManager(menuActionService, hero, enemyService);
                                storyManager.SetLastDiffLvl(lastDiffLvl);
                                storyManager.Start();
                                
                                if (storyManager.DiffLvl == lastDiffLvl) 
                                {
                                    tryHard++;
                                    enemyService.UpgradeEnemiesByDiffLvl(tryHard, storyManager.DiffLvl);
                                }
                                else
                                {
                                    tryHard = 0;
                                }

                                lastDiffLvl = storyManager.DiffLvl; 
                                hero.Reset();
                                
                                switch(storyManager.DiffLvl)
                                {
                                    case 1:
                                        choseLvlOne = true;
                                        break;
                                    case 2:
                                        choseLvlTwo = true;
                                        break;
                                    case 3:
                                        choseLvlThree = true;
                                        break;
                                }

                                if (choseLvlOne && choseLvlTwo && choseLvlThree)
                                {
                                    enemyService.UpgradeEnemies(1);
                                    choseLvlOne = false;
                                    choseLvlTwo = false;
                                    choseLvlThree = false;
                                }
                            }
                        }
                        else
                            Console.WriteLine("\nThere is no heroes to choose");
                        break;
                    case '6':
                        exit = 1;
                        if(heroes != null)
                        {
                            bool IsFileCreated = CreateXmlFileWithAllHeroes(heroes);
                            if (IsFileCreated)
                                Console.WriteLine("\nSaved all Heroes in file listOfAllHeroes.xml");
                        }
                        Console.WriteLine("Press Enter to continue");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Action you entered does not exist");
                        break;
                }

                if (exit == 1)
                    break;
                
            }
        }

        static XmlRootAttribute SetNameOfNode(string nameOfNode)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
            xmlRootAttribute.ElementName = nameOfNode;
            xmlRootAttribute.IsNullable = true;
            return xmlRootAttribute;
        }

        static bool CreateXmlFileWithAllHeroes(List<Hero> heroes)
        {
            TextWriter writer = null;
            try
            {
                string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
                string fileName = "listOfAllHeroes";
                string filePath = path + "\\" + fileName + ".xml";
                var serializer = new XmlSerializer(typeof(List<Hero>), SetNameOfNode("ListOfAllHeroes"));
                writer = new StreamWriter(filePath, false);
                serializer.Serialize(writer, heroes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return true;
        }
    }
}
