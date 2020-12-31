using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Application.Controllers
{
    public class ProductsController
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var entities = await _productRepository.Query()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<List<ProductDto>> GetAllActiveAsync()
        {
            var entities = await _productRepository.Query()
                .IsActive()
                .IsNotRemoved()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<ProductDto> GetByIdAsync(long id)
        {
            var entity = await _productRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            var result = entity?.ToDto();

            return result;
        }

        public async Task<long> CreateAsync(ProductDto dto)
        {
            var product = new Product(dto.Name, dto.Description, dto.Unit, dto.QuantityPerUnit, dto.DefaultPricePerUnit);

            _productRepository.Insert(product);
            await _productRepository.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var product = await _productRepository.Query()
                .ById(dto.Id)
                .FirstOrDefaultAsync();

            if (product == null)
                throw new BadRequestException($"Not found product with id: {dto.Id}");

            product.Update(dto.Name, dto.Description, dto.Unit, dto.QuantityPerUnit, dto.DefaultPricePerUnit);

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(long id)
        {
            var product = await _productRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            if (product == null)
                throw new BadRequestException($"Not found product with id: {id}");

            product.Remove();

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}