using RPG_GAME.Service.Abstract;
using RPG_GAME.Core.Common;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Service.Common
{
    public class BaseService<T> : IService<T> where T : BaseEntity
    {
        public List<T> Objects { get; set; }

        public BaseService()
        {
            Objects = new List<T>();
        }

        public int AddObject(T obj)
        {
            Objects.Add(obj);
            return obj.Id;
        }

        public List<T> GetAllObjects() => Objects;

        public void RemoveObject(T obj)
        {
            Objects.Remove(obj);
        }

        public T GetObjectById(int id)
        {
            return Objects.FirstOrDefault(o => o.Id == id);
        }

        public int GetLastId()
        {
            int id;

            if (Objects.Any())
            {
                id = Objects.OrderBy(o => o.Id).LastOrDefault().Id;
            }
            else
            {
                id = 0;
            }

            return id;
        }
    }
}
