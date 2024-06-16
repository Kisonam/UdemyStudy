using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _repository;
        public ProductRepository(StoreContext repository){
            _repository = repository;
        }
        public async Task<IReadOnlyList<Product>> GetProductAsynk()
        {
            

            return await _repository.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
           return await _repository.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsynk(int id)
        {
            return await _repository.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsynk()
        {
           return await _repository.ProductTypes.ToListAsync();
        }
    }
}