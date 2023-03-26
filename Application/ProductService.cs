using Domain.Models.Products;
using Infrastructure.Core;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ProductService : BaseService, IProductService
    {
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;


        [Obsolete]
        public ProductService(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IConfiguration configuration) : base(unitOfWork)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        [Obsolete]
        public async Task<bool> CreateNewProduct(CreateProductDto product)
        {

            try
            {
                var uniqueFileName = GetUniqueFileName(product.ProductImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.ContentRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                product.ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));

                var newProduct = new Product
                {
                    ProductDescription = product.ProductDescription,
                    ProductImage = filePath,
                    ProductName = product.ProductName,
                };
                _unitOfWork.GetRepository<Product>().Create(newProduct);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                var products = await _unitOfWork.GetRepository<Product>().GetAsync();
                return products.Reverse().ToList();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public async Task<Product> Find(long? id)
        {
            try
            {
                var product = await _unitOfWork.GetRepository<Product>().FindAsync(product => product.ProductId == id);
                return product;
            }
            catch
            {
                return null;
            }
        }

        [Obsolete]
        public async Task<bool> UpdateProduct(long id, UpdateProductDto product, IFormFile ProductImage)
        {
            try
            {
                var productExist = await Find(id);

                productExist.ProductDescription = product.ProductDescription;
                productExist.ProductName = product.ProductName;

                if (ProductImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(ProductImage.FileName);
                    var uploads = Path.Combine(hostingEnvironment.ContentRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
                    productExist.ProductImage = filePath;
                }
                _unitOfWork.GetRepository<Product>().Update(productExist);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
