using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System.Reflection;

namespace RPG_Game.Infrastructure.Mappings
{
    internal class MongoDbClassMap
    {
        private readonly IList<Assembly> _assemblies = new List<Assembly>();

        protected MongoDbClassMap(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException($"Assembly '{nameof(assembly)}' is null");
            }

            _assemblies.Add(assembly);
        }

        protected MongoDbClassMap(IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
            {
                throw new ArgumentNullException($"Assemblies are null");
            }

            _assemblies = assemblies.ToList();
        }

        private void RegisterMappings()
        {
            var configs = FindAllConfigurations();

            foreach (var config in configs)
            {
                var interfaces = config.GetInterfaces();
                var entityMapConfig = interfaces.Where(i => i.IsGenericType && i.IsInterface && 
                        i.GetGenericTypeDefinition() == typeof(IEntityMapConfiguration<>)).SingleOrDefault();
                var entityType = entityMapConfig.GetGenericArguments()[0];
                var typeOfBsonClassMap = typeof(BsonClassMap<>).MakeGenericType(entityType);
                var bsonClassMap = Activator.CreateInstance(typeOfBsonClassMap);

                if (bsonClassMap is null)
                {
                    throw new InvalidOperationException($"Invalid type of class '{entityType.FullName}' please check if everything is correctly implemented");
                }

                var methodRegister = typeOfBsonClassMap.GetMethod("RegisterClassMap", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                var instance = Activator.CreateInstance(config);

                if (instance is null)
                {
                    throw new InvalidOperationException($"Cannot create instance of object '{config.FullName}'");
                }

                var methodMap = instance.GetType().GetMethod("Map");
                methodMap?.Invoke(instance, new object[] { bsonClassMap });
                methodRegister?.Invoke(null, new object[] { bsonClassMap });
            }
        }

        private IList<Type> FindAllConfigurations()
        {
            var types = _assemblies.SelectMany(t => t.GetTypes()).ToList();
            var configs = types.Where(c => c.IsClass &&
                    DoesTypeSupportInterface(c, typeof(IEntityMapConfiguration<>)))
                .ToList();

            return configs;
        }

        private static bool DoesTypeSupportInterface(Type type, Type inter)
        {
            if (inter.IsAssignableFrom(type))
                return true;
            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == inter))
                return true;
            return false;
        }

        private void SetGlobalConvention()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camel case", pack, t => true);
        }

        private void Map()
        {
            RegisterMappings();
            SetGlobalConvention();
        }

        public static void RegisterAllMappings(IEnumerable<Assembly> assemblies)
        {
            var mappings = new MongoDbClassMap(assemblies);
            mappings.Map();
        }

        internal static void RegisterAllMappings(Assembly assembly)
        {
            var mappings = new MongoDbClassMap(assembly);
            mappings.Map();
        }
    }
}
