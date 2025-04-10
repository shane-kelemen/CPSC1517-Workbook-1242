using DBInteractionSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractionSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public Product Product_GetByID(int productID)
        {
            return _westWindContext.Products
                                   .Where(product => product.ProductID == productID)
                                   .FirstOrDefault();
        }



        public int Product_Add(Product product)
        {
            List<Exception> errors = new List<Exception>();
            
            #region Data Validation

            if (product == null)
            {
                throw new ArgumentNullException("You must supply product information to be saved!");
            }

            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                errors.Add(new Exception("The product must be provided!"));
            }

            if (product.CategoryID <= 0)
            {
                errors.Add(new Exception("The category ID must be greater than zero!"));
            }

            if (product.SupplierID <= 0)
            {
                errors.Add(new Exception("The supplier ID must be greater than zero!"));
            }

            if (string.IsNullOrWhiteSpace(product.QuantityPerUnit))
            {
                errors.Add(new Exception("The quantity per unit must be provided!"));
            }

            if (product.UnitPrice < 0)
            {
                errors.Add(new Exception("The unit price must be greater than or equal to zero!"));
            }

            if (product.UnitsOnOrder < 0)
            {
                errors.Add(new Exception("The units on order must be greater than or equal to zero!"));
            }
            #endregion

            #region Business Rule Enforcement

            Supplier supplier = _westWindContext.Suppliers
                                                .Where(supplier => product.SupplierID == supplier.SupplierID)
                                                .FirstOrDefault();

            if (supplier == null)
            {
                errors.Add(new Exception("A supplier does not exist for the included Supplier ID!"));
            }

            Category category = _westWindContext.Categories
                                                .Where(category => category.CategoryID == product.CategoryID)
                                                .FirstOrDefault();

            if(category == null)
            {
                errors.Add(new Exception("A category does not exist for the included Category ID!"));
            }

            bool exists = false;

            exists = _westWindContext
                     .Products
                     .Include(product => product.Supplier)
                     .Any(currentProduct => currentProduct.SupplierID == product.SupplierID
                                            && currentProduct.ProductName == product.ProductName
                                            && currentProduct.QuantityPerUnit == product.QuantityPerUnit);

            if (exists)
            {
                errors.Add(new Exception($"Product {product.ProductName} from "
                                            + $"{product.Supplier.CompanyName} of size " 
                                            + $"{product.QuantityPerUnit} already exists!"));
            }
            #endregion

            if (errors.Count > 0)
            {
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There were data validation errors detected with the Add Product operation!",
                                                    errors);
            }

            try
            {
                _westWindContext.Products.Add(product);
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            if (errors.Count > 0)
            {
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There was a server error detected with the Add Product operation!",
                                                    errors);
            }
            else
            {
                try
                {
                    _westWindContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    _westWindContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

            return product.ProductID;
        }


        public int Product_Update(Product product)
        {
            List<Exception> errors = new List<Exception>();

            #region Data Validation

            if (product == null)
            {
                throw new ArgumentNullException("You must supply product information to be saved!");
            }

            bool exists = _westWindContext
                            .Products
                            .Any(currentProduct => currentProduct.ProductID == product.ProductID);

            if (!exists)
            {
                throw new ArgumentException($"Product {product.ProductName} from " +
                                            $"{product.Supplier.CompanyName} of size " +
                                            $"{product.QuantityPerUnit} does not exist!");
            }


            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                errors.Add(new Exception("The product must be provided!"));
            }

            if (product.CategoryID <= 0)
            {
                errors.Add(new Exception("The category ID must be greater than zero!"));
            }

            if (product.SupplierID <= 0)
            {
                errors.Add(new Exception("The supplier ID must be greater than zero!"));
            }

            if (string.IsNullOrWhiteSpace(product.QuantityPerUnit))
            {
                errors.Add(new Exception("The quantity per unit must be provided!"));
            }

            if (product.UnitPrice < 0)
            {
                errors.Add(new Exception("The unit price must be greater than or equal to zero!"));
            }

            if (product.UnitsOnOrder < 0)
            {
                errors.Add(new Exception("The units on order must be greater than or equal to zero!"));
            }
            #endregion

            #region Business Rule Enforcement

            Supplier supplier = _westWindContext.Suppliers
                                                .Where(supplier => product.SupplierID == supplier.SupplierID)
                                                .FirstOrDefault();

            if (supplier == null)
            {
                errors.Add(new Exception("A supplier does not exist for the included Supplier ID!"));
            }

            Category category = _westWindContext.Categories
                                                .Where(category => category.CategoryID == product.CategoryID)
                                                .FirstOrDefault();

            if (category == null)
            {
                errors.Add(new Exception("A category does not exist for the included Category ID!"));
            }


            exists = _westWindContext
                     .Products
                     .Include(product => product.Supplier)
                     .Any(currentProduct => currentProduct.SupplierID == product.SupplierID
                                            && currentProduct.ProductName == product.ProductName
                                            && currentProduct.QuantityPerUnit == product.QuantityPerUnit
                                            && currentProduct.ProductID != product.ProductID);

            if (exists)
            {
                errors.Add(new Exception($"Product {product.ProductName} from "
                                            + $"{product.Supplier.CompanyName} of size "
                                            + $"{product.QuantityPerUnit} already exists!"));
            }
            #endregion

            if (errors.Count > 0)
            {
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There were data validation errors detected with the Update Product operation!",
                                                    errors);
            }

            try
            {
                EntityEntry<Product> updating = _westWindContext.Entry(product);
                updating.State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            int rowsAffected = 0;

            if (errors.Count > 0)
            {
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There was a server error detected with the Update Product operation!",
                                                    errors);
            }
            else
            {            
                try
                {
                    rowsAffected = _westWindContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    _westWindContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

            return rowsAffected;  // placeholder return value
        }
    }
}
