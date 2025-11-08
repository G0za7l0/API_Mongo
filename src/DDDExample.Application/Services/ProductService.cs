using AutoMapper;
using DDDExample.Application.DTOs;
using DDDExample.Application.Interfaces;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;

namespace DDDExample.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync() =>
            _mapper.Map<IEnumerable<ProductDto>>(await _repo.GetAllAsync());

        public async Task<ProductDto?> GetByIdAsync(string id) =>
            _mapper.Map<ProductDto>(await _repo.GetByIdAsync(id));

        public async Task CreateAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _repo.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _repo.UpdateAsync(product);
        }

        public async Task DeleteAsync(string id) =>
            await _repo.DeleteAsync(id);
    }
}
