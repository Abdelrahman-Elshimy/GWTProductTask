using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Products
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Product name is required!")]
        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        [Required(ErrorMessage = "Product Description is required!")]
        public string ProductDescription { get; set; }
    }
}
