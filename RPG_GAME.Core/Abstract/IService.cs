using System.Collections.Generic;

namespace RPG_GAME.Service.Abstract
{
    public interface IService<T>
    {
        List<T> Objects { get; set; }

        List<T> GetAllObjects();
        T GetObjectById(int id);
        int AddObject(T obj);
        void RemoveObject(T obj);
        int GetLastId();
    }
}
