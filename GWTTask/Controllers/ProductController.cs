using Application;
using Domain.Models.Products;
using Hangfire;
using Hangfire.Server;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace GWTTask.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService= productService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var products = await _productService.GetAllProducts();
            return View(products);
        }
     

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            var updateProduct = new UpdateProductDto
            {
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage
            };
            return View(updateProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, UpdateProductDto product, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                var isUpdated= await _productService.UpdateProduct(id, product, ProductImage);
                if (isUpdated)
                {
                    TempData["Success"] = "Product updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Can't update the product";
                    return View();
                }
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto model)
        {
            if (ModelState.IsValid)
            {
                var isAdded = await _productService.CreateNewProduct(model);
                if (isAdded)
                {
                    TempData["Success"] = "Product added successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Can't add the product";
                    return View();
                }
            }
            return View(model);
            // do something with the above data
            // to do : return something
        }
    }
}
