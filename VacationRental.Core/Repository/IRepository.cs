using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Api.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        ICollection<T> GetAll();
        int Insert(T entity);
        void Update(int id, T entity);
        void Delete(int id);
    }
}
