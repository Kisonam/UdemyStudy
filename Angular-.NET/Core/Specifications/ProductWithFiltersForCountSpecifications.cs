using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecifications : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecifications(ProductsSpecParams productsSpecParams)
        : base(x => 
        (string.IsNullOrEmpty(productsSpecParams.Search) || x.Name.ToLower().Contains
        (productsSpecParams.Search)) &&
        (productsSpecParams.BrandId == null || x.ProductBrandId == productsSpecParams.BrandId) 
        && (productsSpecParams.TypeId == null || x.ProductTypeId == productsSpecParams.TypeId))
        {
        }
    }
}