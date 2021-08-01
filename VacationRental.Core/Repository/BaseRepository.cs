using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Api.Core.Repository
{
    public class BaseRepository<T> : Repository<T> where T : class
    {
        public BaseRepository()
        {
            this.context = new Dictionary<int, T>();
        }
        public override T Get(int id)
        {
            if (!context.ContainsKey(id))
                return null;
            return context[id];
        }

        public override ICollection<T> GetAll()
        {
            return context.Values;
        }

        public override int Insert(T entity)
        {
            entity.GetType().GetProperty("Id").SetValue(entity, context.Count + 1);
            context.Add(context.Count + 1, entity);
            return context.Count;
        }

        public override void Update(int id, T entity)
        {
            this.Delete(id);
            context.Add(id, entity);
        }

        public override void Delete(int id)
        {
            context.Remove(id);
        }
    }
}
