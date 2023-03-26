using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Products
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required!")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Image is required!")]
        public IFormFile ProductImage { get; set; }
        [Required(ErrorMessage = "Product Message is required!")]
        public string ProductDescription { get; set; }
            }
}
