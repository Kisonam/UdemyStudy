using API.Dtos;
using API.Errors;
using API.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   public class ProductsController : BaseApiController
   {
      private readonly IGenericRepository<ProductBrand> _brandRepository;
      private readonly IGenericRepository<Product> _productRepository;
      private readonly IGenericRepository<ProductType> _productTypeRepo;
      private readonly IMapper _mapper;

      public ProductsController(IGenericRepository<Product> productRepository,
      IGenericRepository<ProductBrand> brandRepository,
      IGenericRepository<ProductType> productTypeRepo,
      IMapper mapper)
      {
         this._productTypeRepo = productTypeRepo;
         this._mapper = mapper;
         this._productRepository = productRepository;
         this._brandRepository = brandRepository;

      }
      [HttpGet]
      public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
      [FromQuery]ProductsSpecParams productsSpecParams)
      {
         var spec = new ProductsWithTypesAndBrandsSpecification(productsSpecParams);

         var countSpec = new ProductWithFiltersForCountSpecifications(productsSpecParams);

         var totalItems = await _productRepository.GetCountAsync(countSpec);

         var products = await _productRepository.ListAsync(spec);
      
         var data = _mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

         return Ok(new Pagination<ProductToReturnDto>(productsSpecParams.PageIndex, productsSpecParams.PageSize, totalItems, data));
      }
      [HttpGet("{id}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] 
      public async Task<ActionResult<ProductToReturnDto>> GetProducts(int id)
      {
         var spec = new ProductsWithTypesAndBrandsSpecification(id);

         var product = await _productRepository.GetEntityWithSpec(spec);

         if (product == null) return NotFound(new ApiResponse(404));

         return _mapper.Map<Product, ProductToReturnDto>(product);
      }
      [HttpGet("brands")]
      public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
      {
         var products = await _brandRepository.ListAllAsync();
         return Ok(products);
      }
      [HttpGet("types")]
      public async Task<ActionResult<List<ProductType>>> GetProductType()
      {
         var products = await _productTypeRepo.ListAllAsync();
         return Ok(products);
      }
      
   }
}