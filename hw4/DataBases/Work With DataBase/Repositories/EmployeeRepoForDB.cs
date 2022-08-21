using ContractControlCentre.DB.Connections;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;

namespace ContractControlCentre.DB.Repositories
{
    public class EmployeeRepoForDB  : IEmployeeRepository
    {
        private readonly DbConnection _connection;
        private EmployeeModelEntity _person;


        public EmployeeRepoForDB(DbConnection connection)
        {
            _connection = connection;
        }



        public void Create(EmployeeModelEntity person)
        {
            _connection.Employees.Add(person);
            _connection.SaveChangesAsync();
        }


        public void Delete(int id)
        {
            var _person = _connection.Employees.Find(id);
            _person.IsDeleted = true;
            _connection.SaveChangesAsync();
        }


        public void Update(EmployeeModelEntity item)
        {
            EmployeeModelEntity updatingEmployee = _connection.Employees.Where(x => x.Id == item.Id).FirstOrDefault();

            if (item.FirstName != null)
                updatingEmployee.FirstName = item.FirstName;

            if (item.LastName != null)
                updatingEmployee.LastName = item.LastName;

            if (item.Age != null)
                updatingEmployee.Age = item.Age;

            if (item.Email != null)
                updatingEmployee.Email = item.Email;

            if (item.Company != null)
                updatingEmployee.Company = item.Company;


            _connection.SaveChangesAsync();
        }


        public EmployeeModelEntity GetById(int id)
        {
            return _connection.Employees.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}

