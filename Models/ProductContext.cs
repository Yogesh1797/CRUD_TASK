using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CRUD_TASK.Models
{
    public class ProductContext : DbContext
    {
      
        public DbSet<Product> Products { get; set; }
    }
}