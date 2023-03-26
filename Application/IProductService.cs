using Domain.Models.Products;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<bool> CreateNewProduct(CreateProductDto product);
        Task<bool> UpdateProduct(long id, UpdateProductDto product, IFormFile ProductImage);
        Task<Product> Find(long? id);
    }
}
