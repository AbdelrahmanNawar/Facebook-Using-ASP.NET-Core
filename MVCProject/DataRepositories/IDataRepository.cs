using System.Collections.Generic;

namespace MVCProject.DataRepositories
{
    public interface IDataRepository<T,K>
    {
        List<T> SelectAll();
        T SelectById(K id);
        void Insert(T t);
        void Update(K id, T t);
        void Delete(K id);
        bool IsExist(K id);
    }
}
