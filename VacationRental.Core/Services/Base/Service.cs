using System.Collections.Generic;
using VacationRental.Api.Core.Repository;

namespace VacationRental.Api.Core.Base.Services
{
    public class Service<T> : IService<T> where T : class
    {
        public IRepository<T> BaseRepository { get; }

        public Service(IRepository<T> repository)
        {
            this.BaseRepository = repository;
        }

        public T Get(int id)
        {
            return BaseRepository.Get(id);
        }

        public ICollection<T> GetAll()
        {
            return BaseRepository.GetAll();
        }

        public int Create(T entity)
        {
            return BaseRepository.Insert(entity);
        }

        public void Update(int id, T entity)
        {
            BaseRepository.Update(id, entity);
        }

        public void Delete(int id)
        {
            BaseRepository.Delete(id);
        }
    }
}
