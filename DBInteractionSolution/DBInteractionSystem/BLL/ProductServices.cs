using DBInteractionSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractionSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBInteractionSystem.BLL
{
    public class ProductServices
    {
        private readonly WestWindContext _westWindContext;

        internal ProductServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }

        public int Product_GetCountForCategory(int categoryID)
        {
            return _westWindContext.Products
                                   .Where(product => product.CategoryID == categoryID)
                                   .Count();
        }


        public List<Product> Product_GetProductsByCategory(int categoryID, int productPage, int productsPerPage)
        {
            return _westWindContext.Products
                                   .Include(product => product.Supplier)
                                   .Where(product => product.CategoryID == categoryID)
                                   .OrderByDescending(product => product.Supplier.CompanyName)
                                   .ThenBy(product => product.ProductName)
                                   .Skip(productPage * productsPerPage)
                                   .Take(productsPerPage)
                                   .ToList();
        }
    }
}
