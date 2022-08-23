using System;
using System.Data.SQLite;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Models;
using Dapper;

namespace ContractControlCentre.DB.Repositories
{
    public class PersonRepoForDB : IPersonRepository
    {
        private const string ConnectionString = "Data Source=CCC_DataBase.db;Version=3;Pooling=true;Max Pool Size=100;";


        public void Create(PersonModel item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO Person_DB(FirstName, LastName, Email, Company, Age) VALUES(@FirstName, @LastName, @Email, @Company, @Age)",
                 new
                 {
                     FirstName = item.FirstName,
                     LastName = item.LastName,
                     Email = item.Email,
                     Company = item.Company,
                     Age = item.Age
                 });
            }
        }


        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM Person_DB WHERE Id=@id",
                new
                {
                    id = id
                });
            }
        }


        public void Update(PersonModel item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE Person_DB SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Company = @Company, Age = @Age WHERE Id = @id",
                 new
                 {
                     FirstName = item.FirstName,
                     LastName = item.LastName,
                     Email = item.Email,
                     Company = item.Company,
                     Age = item.Age,
                     id = item.Id
                 });
            }
        }


        public IList<PersonModel> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<PersonModel>("SELECT Id, FirstName, LastName, Email, Company, Age FROM Person_DB").ToList();
            }
        }


        public PersonModel GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<PersonModel>("SELECT Id, FirstName, LastName, Email, Company, Age FROM Person_DB WHERE Id=@id",
                    new
                    {
                        id = id
                    });
            }
        }
    }
}

