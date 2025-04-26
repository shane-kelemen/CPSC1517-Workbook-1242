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
        // This set up is the same in every system service file in your BLL folder
        // The only thing that will change in different solutions is the name of the
        // context being used.
        private readonly WestWindContext _westWindContext;

        internal ProductServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }


        /// <summary>
        /// This method was created to support the paginator on the CategoryProducts page.  It returns
        /// the number of total products in the supplied category.
        /// </summary>
        /// <param name="categoryID">The category ID for which a count is required.</param>
        /// <returns>The count of the products in the supplied category</returns>
        public int Product_GetCountForCategory(int categoryID)
        {
            return _westWindContext.Products
                                   .Where(product => product.CategoryID == categoryID)
                                   .Count();
        }


        /// <summary>
        /// This method will return products from the supplied Category.  They will be limited 
        /// in number to the value of productsPerPage, and offset by the productPage value. 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="productPage">The zero-index based page offset of results to return</param>
        /// <param name="productsPerPage">The maximum number of results to return</param>
        /// <returns>A List of Product entity instances for the supplied category and paging values</returns>
        public List<Product> Product_GetProductsByCategory(int categoryID, int productPage, int productsPerPage)
        {
            // UPDATE!  The .AsNoTracking() method was used below to ensure that entities
            //          for all the products are not created in the 
            //          Entity Framework's Tracking subsystem.  This will help avoid the duplicate
            //          entity tracking error that was being caused in an edge case. 
            return _westWindContext.Products
                                   .AsNoTracking()
                                   .Include(product => product.Supplier)
                                   .Where(product => product.CategoryID == categoryID)
                                   .OrderByDescending(product => product.Supplier.CompanyName)
                                   .ThenBy(product => product.ProductName)
                                   .Skip(productPage * productsPerPage)  // Skip() jumps past the numerical number of results
                                                                         // If on the "first" page, product page will be zero,
                                                                         // so the results will begin at the start of the set.
                                                                         // The second page will begin after jumping past
                                                                         // productPerPage of results.

                                   .Take(productsPerPage)       // Only keep productsPerPage number of results beginning after
                                                                // the skipped results.
                                   .ToList();
        }


        /// <summary>
        /// This method will return the details of a single product matching the provided productID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns>A Product entity instance if it exists</returns>
        public Product Product_GetByID(int productID)
        {
            // UPDATE!  The .AsNoTracking() method was used below to ensure that entities
            //          for the specific category and supplier are not created in the 
            //          Entity Framework's Tracking subsystem.  This will help avoid the duplicate
            //          entity tracking error that was being caused in an edge case. 
            return _westWindContext.Products
                                   .AsNoTracking()
                                   .Where(product => product.ProductID == productID)
                                   .FirstOrDefault();
        }


        /// <summary>
        /// This method will attempt to add a new product into the Products table of the Westwind database,
        /// or more accurately, the database that is being accessed through the DbContext (WestWindContext) 
        /// provided to the constructor of the ProductServices class above from the associated Transient in
        /// the WestWindExtensions class.
        /// </summary>
        /// <param name="product">An instance of the Product entity.  Definition found in the Entities 
        ///                             folder of the system library.</param>
        /// <returns>The ProductID automatically assigned to the product by the database.  Identity column.</returns>
        /// <exception cref="ArgumentNullException">Thrown if a Product instance is not provided.</exception>
        /// <exception cref="AggregateException">Throws a collection of Exceptions encountered during data validation</exception>
        /// 
        public int Product_Add(Product product)
        {
            List<Exception> errors = new List<Exception>();

            #region Data Validation

            // If no information is provided, then we may as well exit the whole operation with a throw
            // to let the user know what they did incorrectly.
            if (product == null)
            {
                throw new ArgumentNullException("You must supply product information to be saved!");
            }


            // UPDATE!  This check was added to immediately abandon the Insert operation if a ProductID
            //          was provided.  Only a product ID of 0 will be allowed past this point.
            if (product.ProductID != 0)
            {
                throw new ArgumentException("Providing a ProductID is not permitted!  It will be automatically assigned to the identity column in the database.");
            }

            // All data should be checked to ensure it is legal.  An example check for the required ProductName
            // is shown here.  Appropriate messages should be added to the Exception collection as shown in the
            // ProductName check.
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

            // Other data checks may include valid ranges for values such as >= 0 for UnitPrice.
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
            // In addition to simple data validation, we may have a more complex business rule.  In this 
            // case, we will allow a supplier to give us only one product with the same name and description.
            // For example:  Supplier 25, may provide Milk in a QuantityPerUnit of 4L, but they may not
            //                have a second product with the same two features.  However, Supplier 30 may 
            //                also supply Milk in a quantity of 4L, or Supplier 25 may also provide Milk in a
            //                QuantityPerUnit of 2L.

            Supplier supplier = _westWindContext.Suppliers
                                                .Where(supplier => product.SupplierID == supplier.SupplierID)
                                                .FirstOrDefault();

            if (supplier == null)
            {
                errors.Add(new Exception("A supplier does not exist for the included Supplier ID!"));
            }
            // UPDATE!  Because we used the .AsNoTracking() method when retrieving our individual
            //          product information in the Product_GetByID() method, the product entity
            //          passed into this method will have a null for its internal product.Supplier
            //          entity.  Trying to use it will cause a NullReferenceException to be thrown,
            //          so assigning the supplier we found above to the product.Supplier entity will
            //          fix the problem.
            else
            {
                product.Supplier = supplier;
            }

            Category category = _westWindContext.Categories
                                                .Where(category => category.CategoryID == product.CategoryID)
                                                .FirstOrDefault();

            if (category == null)
            {
                errors.Add(new Exception("A category does not exist for the included Category ID!"));
            }
            // UPDATE!  Because we used the .AsNoTracking() method when retrieving our individual
            //          product information in the Product_GetByID() method, the product entity
            //          passed into this method will have a null for its internal product.Category
            //          entity.  Trying to use it will cause a NullReferenceException to be thrown,
            //          so assigning the category we found above to the product.Category entity will
            //          fix the problem.
            else
            {
                product.Category = category;
            }


            bool exists = false;

            // Check to see if the product with the provided features already exists 
            exists = _westWindContext
                     .Products
                     // UPDATE!  As we have already found and assigned the category and supplier
                     //          entities, this .Include() should not be necessary.
                     //.Include(product => product.Supplier)
                     .Any(currentProduct => currentProduct.SupplierID == product.SupplierID
                                            && currentProduct.ProductName == product.ProductName
                                            && currentProduct.QuantityPerUnit == product.QuantityPerUnit);

            // If the product with the provided features does exist, an an Exception with an appropriate and descriptive
            // message to the Exception collection.
            if (exists)
            {
                errors.Add(new Exception($"Product {product.ProductName} from "
                                            + $"{product.Supplier.CompanyName} of size "
                                            + $"{product.QuantityPerUnit} already exists!"));
            }
            #endregion

            // If any data validation or business rule violation is detected, then we should inform the user
            // of the problems and exit without performing any database manipulation.
            if (errors.Count > 0)
            {
                // This Clear() of the ChangeTracker is not required unless a database data change has occurred,
                // but it does not hurt anything to be double sure.
                _westWindContext.ChangeTracker.Clear();

                // This throws the Exception collection back to the calling scope so that the contained errors
                // may be used.
                throw new AggregateException("There were data validation errors detected with the Add Product operation!",
                                                    errors);
            }

            // Once all data validation and business rules adherence has been cleared without problems,
            // then we may proceed with database operations.

            // Accessing a database may always possibly result in unexpected errors.  For instance, a
            // communication breakdown with the database engine, so it is advisable to use a try/catch
            // to perform the operation.  Add any errors to the Exception collection.
            try
            {
                // This is where the insert operation is being attempted, but the operation is only being tested in memory,
                // not saved to the database in SQL Server yet.  This is similar to how a transaction works in SQL.  Above,
                // when we Clear() the ChangeTracker, we are essentially performing a rollback on the transaction.  Later, we
                // will attempt to perform a commit when we use the SaveChanges() method of the context.
                _westWindContext.Products.Add(product);
            }
            catch (Exception ex)
            {
                // Add any error that may be caught into our collection of errors to be
                // thrown back to the caller if a database interaction problem occurs
                errors.Add(ex);
            }

            // If any errors whatsoever are encountered, we must clear any changes that may have occurred
            // successfully before throwing the exception that ends the method.
            if (errors.Count > 0)
            {

                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There was a server error detected with the Add Product operation!",
                                                    errors);
            }
            // If no errors are encountered, then we may attempt to save all changes to the database.
            else
            {
                // Once again, a try/catch should be used here, as the action of pushing the data changes to the
                // database may encounter problems that cannot be or have not been predicted, such as a communications
                // error, or the truncation of data.
                try
                {
                    _westWindContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Once again, be sure to Clear() the database changes before throwing the received Exception back
                    // to the user.  The error collection is not required here as there can only be one Exception thrown
                    // if you have gotten to this point.
                    _westWindContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

            // If the SaveChanges() method is successful, a ProductID will have been assigned to the product that we tried
            // to save.  We have decided to send it back to the user.
            return product.ProductID;
        }


        /// <summary>
        /// This method will attempt to update an existing product in the Products table of the Westwind database,
        /// or more accurately, the database that is being accessed through the DbContext (WestWindContext) 
        /// provided to the constructor of the ProductServices class above from the associated Transient in
        /// the WestWindExtensions class.
        /// </summary>
        /// <param name="product">An instance of the Product entity.  Definition found in the Entities 
        ///                             folder of the system library.</param>
        /// <returns>Thenumber of rows affected in the database.  Should be 1 in this case if update successful</returns>
        /// <exception cref="ArgumentNullException">Thrown if a Product instance is not provided.</exception>
        /// <exception cref="ArgumentException">Thrown if a provided Product instance is incorrect in some way</exception>
        /// <exception cref="AggregateException">Throws a collection of Exceptions encountered during data validation</exception>
        /// 
        public int Product_Update(Product product)
        {
            List<Exception> errors = new List<Exception>();

            #region Data Validation

            if (product == null)
            {
                throw new ArgumentNullException("You must supply product information to be saved!");
            }

            // UPDATE!  If the product ID is invalid for the database, there is no point
            // in continuing with an update operation that will not affect anything.
            if (product.ProductID <= 0)
            {
                throw new ArgumentException("You must supply a valid ProductID, greater than 0!");
            }


            // Make sure we have the product that the user wants to update.  If it is not present,
            // we can go no further and so should throw an immediate exception.
            bool exists = _westWindContext
                            .Products
                            .Any(currentProduct => currentProduct.ProductID == product.ProductID);

            if (!exists)
            {
                throw new ArgumentException($"Product {product.ProductName} from " +
                                            $"{product.Supplier.CompanyName} of size " +
                                            $"{product.QuantityPerUnit} does not exist!");
            }

            // Assuming the submitted product does exist...
            // All data should be checked to ensure it is legal.  An example check for the required ProductName
            // is shown here.  Other data checks may include valid ranges for values such as >= 0 for UnitPrice.
            // Appropriate messages should be added to the Exception collection as shown in the ProductName check.
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

            // Though we are allowed to add products to the database table without linking them to a supplier
            // or category, for our purposes we do not want to allow this, so we must check that they exist
            // before proceeding.
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


            // Check to see if the product with the provided features already exist from the same supplier.
            exists = _westWindContext
                     .Products
                     .Include(product => product.Supplier)
                     .Any(currentProduct => currentProduct.SupplierID == product.SupplierID
                                            && currentProduct.ProductName == product.ProductName
                                            && currentProduct.QuantityPerUnit == product.QuantityPerUnit
                                            && currentProduct.ProductID != product.ProductID);

            // If the product with the provided features does exist, add an Exception with an appropriate and descriptive
            // message to the Exception collection.
            if (exists)
            {
                errors.Add(new Exception($"Product {product.ProductName} from "
                                            + $"{product.Supplier.CompanyName} of size "
                                            + $"{product.QuantityPerUnit} already exists!"));
            }
            #endregion

            // If any errors whatsoever are encountered, we must clear any changes that may have occurred
            // successfully before throwing the exception that ends the method.
            if (errors.Count > 0)
            {
                // The ChangeTracker stores all changes to be made to the database when a SaveChanges() call
                // is used.  This would include any changes left over in the ChangeTracker during a failed 
                // database operation.  Thus, we must Clear() the ChangeTracker on error detection.
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There were data validation errors detected with the Update Product operation!",
                                                    errors);
            }

            // When performing either temporary or permanent actions, it is best to try/catch the operations
            // to make sure we mitigate any unexpected problems.
            try
            {
                // This line tells the Entity Framework that you wish use the data in the inputProduct to 
                // perform an action.
                EntityEntry<Product> updating = _westWindContext.Entry(product);
                updating.State = EntityState.Modified;  // This line sets the action as an update to the database
            }
            catch (Exception ex)
            {
                errors.Add(ex);     // Add any errors encountered to the error collection, same as with data validation
                                    // and business rules violations.
            }


            // The following block is where the actions against the database are actually performed.  In particular,
            // SaveChanges().  It is possible an action we are trying to perform is illegal in the database, so we
            // put this block in a try/catch, and if an error occurs, then we clear the ChangeTracker and then throw
            // the Exception we caught back to the calling method, because this is the last action we will be perfoming.
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

            return rowsAffected;    // If the operation was successful, return the number of rows affected.
        }


        /// <summary>
        /// Note that a Logical delete is the same as an update where the only thing changed is setting the 
        /// Discontinued flag to true, and thus we may ignore most of the rest of the data submitted.  This 
        /// reduces the amount of data validation and business rules enforement required.
        /// </summary>
        /// <param name="product">The Product to be Discontinued</param>
        /// <returns>The number of rows affected in the database.  Should be 1 in this case if Discontinue successful</returns>
        /// <exception cref="ArgumentNullException">Thrown if a Product instance is not provided.</exception>
        /// <exception cref="ArgumentException">Thrown if a provided Product instance is incorrect in some way</exception>
        ///
        public int Product_Discontinue(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("You must supply a product for discontinuation!");
            }

            // UPDATE!  If the product ID is invalid for the database, there is no point
            // in continuing with a discontinue operation that will not affect anything.
            if (product.ProductID <= 0)
            {
                throw new ArgumentException("You must supply a valid ProductID, greater than 0!");
            }

            Product exists = _westWindContext
                            .Products
                            .FirstOrDefault(currentProduct => currentProduct.ProductID == product.ProductID);

            if (exists == null)
            {
                throw new ArgumentException($"Product {product.ProductName} from " +
                                    $"{product.Supplier.CompanyName} of size {product.QuantityPerUnit} " +
                                    $" does not exist onfile!");
            }

            exists.Discontinued = true;     // Mark the product as discontinued (soft/logical delete)

            EntityEntry<Product> updating = _westWindContext.Entry(exists);
            updating.State = EntityState.Modified;

            int rowsAffected = 0;
            try
            {
                rowsAffected = _westWindContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _westWindContext.ChangeTracker.Clear();
                throw ex;
            }

            return rowsAffected;
        }

        /// <summary>
        /// A Product may need to be reactivated for any of a number of different reasons.
        /// Effectively the method is the same as Discontinue, aside from setting the Discontinue 
        /// flag to false.
        /// </summary>
        /// <param name="product">The Product to be Reactivated</param>
        /// <returns>The number of rows affected in the database.  Should be 1 in this case if Activation successful</returns>
        /// <exception cref="ArgumentNullException">Thrown if a Product instance is not provided.</exception>
        /// <exception cref="ArgumentException">Thrown if a provided Product instance is incorrect in some way</exception>
        ///
        public int Product_Activate(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("You must supply a product for reactivation!");
            }

            // UPDATE!  If the product ID is invalid for the database, there is no point
            // in continuing with an activate operation that will not affect anything.
            if (product.ProductID <= 0)
            {
                throw new ArgumentException("You must supply a valid ProductID, greater than 0!");
            }

            Product exists = _westWindContext
                            .Products
                            .FirstOrDefault(currentProduct => currentProduct.ProductID == product.ProductID);

            if (exists == null)
            {
                throw new ArgumentException($"Product {product.ProductName} from " +
                                    $"{product.Supplier.CompanyName} of size {product.QuantityPerUnit} " +
                                    $" does not exist onfile!");
            }

            exists.Discontinued = false;

            EntityEntry<Product> updating = _westWindContext.Entry(exists);
            updating.State = EntityState.Modified;

            int rowsAffected = 0;
            try
            {
                rowsAffected = _westWindContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _westWindContext.ChangeTracker.Clear();
                throw ex;
            }

            return rowsAffected;
        }


        /// <summary>
        /// This method will permanently delete a record from the database.  This should not be done in 
        /// regular business practices unless the data has been archived.
        /// </summary>
        /// <param name="product">The Product to be Reactivated</param>
        /// <returns>The number of rows affected in the database.  Should be 1 in this case if Delete successful</returns>
        /// <exception cref="ArgumentNullException">Thrown if a Product instance is not provided.</exception>
        /// <exception cref="ArgumentException">Thrown if a provided Product instance is incorrect in some way</exception>
        /// <exception cref="AggregateException">Throws a collection of Exceptions encountered during checks for child records</exception>
        /// 
        public int Product_Delete(Product product)
        {
            List<Exception> errors = new List<Exception>();

            if (product == null)
            {
                throw new ArgumentNullException("You must supply a product for reactivation!");
            }

            // UPDATE!  If the product ID is invalid for the database, there is no point
            // in continuing with a delete operation that will not affect anything.
            if (product.ProductID <= 0)
            {
                throw new ArgumentException("You must supply a valid ProductID, greater than 0!");
            }

            // The Any() method returns true if at least one item in the collection satisfies the condition
            // In this case, is there a product that has the same ProductID as the input Product?
            bool exists = _westWindContext.Products
                                          .Any(currentProduct => currentProduct.ProductID == product.ProductID);

            if (!exists)
            {
                throw new ArgumentException($"Product {product.ProductName} from " +
                                   $"{product.Supplier.CompanyName} of size {product.QuantityPerUnit} " +
                                   $" does not exist onfile!");
            }


            // Is there a product that matches the input productID that is being referred to by a record in
            // the ManifestItems table?
            // Note:  This style is looking directly at the ManifestItems table and looking for a matching
            //        product.
            exists = _westWindContext.ManifestItems
                                     .Any(manifestItem => manifestItem.ProductID == product.ProductID);

            if(exists)
            {
                errors.Add(new ArgumentException($"Product {product.ProductName} from" +
                           $" {product.Supplier.CompanyName} of size {product.QuantityPerUnit} has " +
                           $" associated manifest records on file. Unable to remove the product."));
            }


            // Is there a product that matches the input productID that is being referred to by a record in
            // the OrderDetails table?
            // Note:  This style is looking at the Order Details collection of the specific product if there
            //        is a matching Product to the input Product.
            exists = _westWindContext.Products.Any(currentProduct => currentProduct.OrderDetails.Count > 0
                                                    && currentProduct.ProductID == product.ProductID);

            if (exists)
            {
                errors.Add(new ArgumentException($"Product {product.ProductName} from" +
                           $" {product.Supplier.CompanyName} of size {product.QuantityPerUnit} has " +
                           $" associated order detail records on file. Unable to remove the product."));
            }


            if (errors.Count > 0)
            {
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("There was a problem with the delete operation: ", errors);
            }


            // This time we are indicating that a delete operation is to be performed, but otherwise the format
            // is the same as for updating.
            EntityEntry<Product> deleting = _westWindContext.Entry(product);
            deleting.State = EntityState.Deleted;       // Indicate a delete...


            int rowsAffected = 0;
            try
            {
                rowsAffected = _westWindContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _westWindContext.ChangeTracker.Clear();
                throw ex;
            }

            return rowsAffected;
        }
    }
}
