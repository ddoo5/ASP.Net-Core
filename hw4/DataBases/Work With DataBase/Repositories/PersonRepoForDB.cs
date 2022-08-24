using System;
using ContractControlCentre.DB.Connections;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ContractControlCentre.DB.Repositories
{
    public class PersonRepoForDB : IPersonRepository
    {
        private readonly DbConnection _connection;

        public PersonRepoForDB(DbConnection connection)
        {
            _connection = connection;
        }



        public void Create(PersonModelEntity person)
        {
            _connection.Persons.Add(person);
            _connection.SaveChangesAsync();
        }


        public void Delete(int id)
        {
            var _person = _connection.Persons.Find(id);
            _person.IsDeleted = true;
            _connection.SaveChangesAsync();
        }


        public void Update(PersonModelEntity item)
        {
            PersonModelEntity updatingPerson = _connection.Persons.Where(x => x.Id == item.Id).FirstOrDefault();

            if(item.FirstName != null)
                updatingPerson.FirstName = item.FirstName;

            if(item.LastName != null)
                updatingPerson.LastName = item.LastName;

            if(item.Age != null)
                updatingPerson.Age = item.Age;

            if(item.Email != null)
                updatingPerson.Email = item.Email;

            if(item.Company != null)
                updatingPerson.Company = item.Company;


            _connection.SaveChangesAsync();
        }


        public IList<PersonModelEntity> GetByName(string Name)
        {
            return _connection.Persons.Where(person => person.FirstName == Name && person.IsDeleted == false).ToList();
        }


        public IList<PersonModelEntity> GetByPagination(int skip, int take)
        {
            return _connection.Persons.Skip(skip)
                .Take(take)
                .Where(user => user.IsDeleted == false)
                .ToList();
        }


        public PersonModelEntity GetById(int id)
        {
            return _connection.Persons.Where(persons => persons.Id == id).FirstOrDefault();
        }
    }
}

