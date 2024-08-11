using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_API.Models;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
         Task<List<T>> GetVillas(Expression<Func<T, bool>> filter = null);
        Task<T> GetVilla(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Create(T villa);
        Task Remove(T villa);
        Task Save();
    }
}