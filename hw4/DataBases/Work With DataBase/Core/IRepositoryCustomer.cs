namespace ContractControlCentre.DB.Core
{
    public interface IRepositoryCustomer<T> where T : class
    {
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}

