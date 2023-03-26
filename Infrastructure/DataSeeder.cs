using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DataSeeder
    {
       
        public static void Seed(ApplicationDBContext context, IHostEnvironment hostEnvironment)
        {
            
            if (!context.Products.Any())
            {
                string directoryPath = Path.Combine(hostEnvironment.ContentRootPath, "ProductsFile");
                string filePath = Path.Combine(directoryPath, "products.json");
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    List<Product> items = JsonConvert.DeserializeObject<List<Product>>(json);
                    context.AddRange(items);
                    context.SaveChanges();
                }
                
            }
        }
    }
}
