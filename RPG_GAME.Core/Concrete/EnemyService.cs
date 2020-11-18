using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RPG_GAME.Service.Concrete
{
    public class EnemyService : BaseService<Enemy>
    {
        public EnemyService()
        {
            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            string nameFile = "enemies";
            var filePath = path + "\\" + nameFile + ".xml";

            
            if (!File.Exists(filePath))
            {
                Initialize();
                SaveEnemiesToXml();
            }
            else
            {
                var listOfObjects = GetObjectsFromXml();
                if (listOfObjects == null)
                {
                    Console.WriteLine($"You should check xml file {nameFile}.xml");
                    Initialize();
                }
                else
                    Objects = listOfObjects;
            }

        }

        public List<Enemy> GetEnemiesByDiffLvl(int difLvl)
        {
            return Objects.Where(obj => obj.DiffLvl == difLvl).ToList();
        }

        public List<Enemy> FindEnemiesByCategory(List<Enemy> listOfEnemies, string Category)
        {
            return listOfEnemies.Where(enemies => enemies.Category == Category).ToList();
        }

        public void ResetEnemies()
        {
            Objects.Where(enemy => enemy.Health < enemy.MaxHealth).ToList().ForEach(enemy =>
            {
                enemy.SetHealth(enemy.MaxHealth);
            });
        }

        private void Initialize()
        {
            AddObject(new Archer(1, "Sworn Archer", 1, 5, "Archer", 1));
            AddObject(new Archer(2, "Sworn Archer", 5, 15, "Archer", 2, 1));
            AddObject(new Archer(3, "Sworn Archer", 15, 25, "Archer", 3, 2));
            AddObject(new Archer(4, "Sworn Archer", 15, 25, "Archer", 3, 2));
            AddObject(new Archer(5, "Beastly Archer", 2, 8, "Archer", 1));
            AddObject(new Archer(6, "Beastly Archer", 7, 17, "Archer", 2, 1));
            AddObject(new Archer(7, "Beastly Archer", 17, 28, "Archer", 3, 2));
            AddObject(new Archer(8, "Beastly Archer", 17, 28, "Archer", 3, 2));
            
            AddObject(new Knight(9, "Fearless Knight", 3, 12, "Knight", 1));
            AddObject(new Knight(10, "Fearless Knight", 17, 28, "Knight", 2, 2));
            AddObject(new Knight(11, "Fearless Knight", 22, 32, "Knight", 3, 3));
            AddObject(new Knight(12, "Fearless Knight", 22, 32, "Knight", 3, 3));
            AddObject(new Knight(13, "Invincible Knight", 5, 15, "Knight", 1));
            AddObject(new Knight(14, "Invincible Knight", 20, 30, "Knight", 2, 2));
            AddObject(new Knight(15,"Invincible Knight", 20, 30, "Knight", 2, 2));
            AddObject(new Knight(16, "Invincible Knight", 25, 35, "Knight", 3, 3));
            AddObject(new Knight(17, "Invincible Knight", 25, 35, "Knight", 3, 3));
            AddObject(new Knight(18, "Invincible Knight", 25, 35, "Knight", 3, 3));
            
            AddObject(new Dragon(19, "Blue Dragon", 7, 20, "Dragon", 1));
            AddObject(new Dragon(20, "Blue Dragon", 20, 35, "Dragon", 2, 4));
            AddObject(new Dragon(21, "Blue Dragon", 20, 35, "Dragon", 2, 4));
            AddObject(new Dragon(22, "Blue Dragon", 27, 40, "Dragon", 3, 5));
            AddObject(new Dragon(23, "Blue Dragon", 27, 40, "Dragon", 3, 5));
            AddObject(new Dragon(24, "Blue Dragon", 27, 40, "Dragon", 3, 5));
            AddObject(new Dragon(25, "Red Dragon", 8, 20, "Dragon", 1));
            AddObject(new Dragon(26, "Red Dragon", 22, 35, "Dragon", 2, 5));
            AddObject(new Dragon(27, "Red Dragon", 22, 35, "Dragon", 2, 5));
            AddObject(new Dragon(28, "Red Dragon", 32, 45, "Dragon", 3, 6));
            AddObject(new Dragon(29, "Red Dragon", 32, 45, "Dragon", 3, 6));
            AddObject(new Dragon(30, "Red Dragon", 32, 45, "Dragon", 3, 6));
            AddObject(new Dragon(31, "Black Dragon", 10, 25, "Dragon",1));
            AddObject(new Dragon(32, "Black Dragon", 25, 45, "Dragon",2, 7));
            AddObject(new Dragon(33, "Black Dragon", 25, 45, "Dragon",2, 7));
            AddObject(new Dragon(34,"Black Dragon", 35, 55, "Dragon",3, 10));
            AddObject(new Dragon(35, "Black Dragon", 35, 55, "Dragon",3, 10));
            AddObject(new Dragon(36, "Black Dragon", 35, 55, "Dragon",3, 10));
        }

        private void SaveEnemiesToXml()
        {
            TextWriter writer = null;
            try
            {
                string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
                string nameFile = "enemies";
                string filePath = path + "\\" + nameFile + ".xml";
                XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
                xmlRootAttribute.ElementName = typeof(Enemy).Name;
                xmlRootAttribute.IsNullable = true;
                var serializer = new XmlSerializer(typeof(List<Enemy>), xmlRootAttribute);
                writer = new StreamWriter(filePath, false);
                serializer.Serialize(writer, ConvertDiffEnemies(Objects));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private List<Enemy> GetObjectsFromXml()
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
            xmlRootAttribute.ElementName = typeof(Enemy).Name;
            xmlRootAttribute.IsNullable = true;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Enemy>), xmlRootAttribute);
            string xml;
            List<Enemy> objectsXml = new List<Enemy>();
            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            string fileName = "enemies";
            try
            {
                xml = File.ReadAllText(path + @"\" + fileName + ".xml");

                StringReader stringReader = new StringReader(xml);
                try
                {
                    objectsXml = (List<Enemy>)xmlSerializer.Deserialize(stringReader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    stringReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            objectsXml = ConvertEnemiesByCategory(objectsXml);

            return objectsXml;
        }

        public List<Enemy> ConvertDiffEnemies(List<Enemy> enemies)
        {
            var convertedEnemies = new List<Enemy>();
            foreach (var enemy in enemies)
            {
                if (enemy.GetType() != typeof(Enemy))
                {
                    var en = new Enemy(enemy.Id, enemy.Name, enemy.Attack, enemy.Health, enemy.Category, enemy.DiffLvl, enemy.HealLvl);
                    en.MaxHealth = enemy.MaxHealth;
                    convertedEnemies.Add(en);
                }
                else
                {
                    convertedEnemies.Add(enemy);
                }
            }
            return convertedEnemies;
        }

        private List<Enemy> ConvertEnemiesByCategory(List<Enemy> enemies)
        {
            var convertedEnemies = new List<Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemy.Category == "Archer")
                {
                    var archer = new Archer(enemy.Id, enemy.Name, enemy.Attack, enemy.Health, enemy.Category, enemy.DiffLvl, enemy.HealLvl);
                    convertedEnemies.Add(archer);
                }
                else if (enemy.Category == "Knight")
                {
                    var knight = new Knight(enemy.Id, enemy.Name, enemy.Attack, enemy.Health, enemy.Category, enemy.DiffLvl, enemy.HealLvl);
                    convertedEnemies.Add(knight);
                }
                else if (enemy.Category == "Dragon")
                {
                    var dragon = new Dragon(enemy.Id, enemy.Name, enemy.Attack, enemy.Health, enemy.Category, enemy.DiffLvl, enemy.HealLvl);
                    convertedEnemies.Add(dragon);
                }
            }

            return convertedEnemies;
        }

        public void UpgradeEnemies(int upgrade)
        {
            Objects.ForEach(enemy =>
            {
                enemy.UpgradeAttack(upgrade * 2);
                enemy.UpgradeMaxHealth(upgrade * 5);
                enemy.SetHealth(enemy.MaxHealth);
                enemy.UpgradeHeal(upgrade);
            });
        }

        public void UpgradeEnemiesByDiffLvl(int upgrade, int diffLvl)
        {
            Objects.Where(enemy => enemy.DiffLvl == diffLvl).ToList().ForEach(enemy =>
            {
                enemy.UpgradeAttack(upgrade * 2);
                enemy.UpgradeMaxHealth(upgrade * 5);
                enemy.SetHealth(enemy.MaxHealth);
                enemy.UpgradeHeal(upgrade);
            });
        }
    }
}
