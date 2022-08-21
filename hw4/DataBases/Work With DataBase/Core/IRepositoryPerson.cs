using ContractControlCentre.DB.Entities;

namespace ContractControlCentre.DB.Core
{
    public interface IRepositoryPerson<T> where T : class
    {
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        IList<T> GetByName(string item);
        IList<T> GetByPagination(int skip, int take);
    }
}
