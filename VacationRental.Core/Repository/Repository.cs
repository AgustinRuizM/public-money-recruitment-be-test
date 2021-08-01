using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VacationRental.Api.Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected IDictionary<int, T> context;
        public abstract T Get(int id);
        public abstract ICollection<T> GetAll();
        public abstract int Insert(T entity);
        public abstract void Update(int id, T entity);
        public abstract void Delete(int id);
    }
}
