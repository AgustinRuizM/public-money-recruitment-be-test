using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Api.Core.Repository;

namespace VacationRental.Api.Core.Base.Services
{
    public interface IService<T> where T : class
    {
        IRepository<T> BaseRepository { get; }
        T Get(int id);
        ICollection<T> GetAll();
        int Create(T entity);
        void Update(int id, T entity);
        void Delete(int id);
    }
}
