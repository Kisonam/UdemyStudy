using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<Villa> Update(Villa villa)
        {
            villa.UpdateData = DateTime.Now;
            _dbContext.Villas.Update(villa);
            await _dbContext.SaveChangesAsync();
            return villa;
        }
    }
}