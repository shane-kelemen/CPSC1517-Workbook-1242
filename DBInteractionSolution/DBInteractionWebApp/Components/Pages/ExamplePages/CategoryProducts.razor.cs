using DBInteractionSystem.BLL;
using DBInteractionSystem.Entities;
using Microsoft.AspNetCore.Components;

namespace DBInteractionWebApp.Components.Pages.ExamplePages
{
    public partial class CategoryProducts
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                     // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();    // For storing error messages to be displayed on the razor page

        private int categoryID = 0;
        List<Category> categories = new List<Category>();

        List<Product> products = new List<Product>();
        private int productPage = 0;
        private int productsPerPage = 5;
        private int totalProductCountForCategory = 0;


        [Inject]
        public ProductServices ProductServices { get; set; }

        [Inject]
        public CategoryServices CategoryServices { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            try
            {
                categories = CategoryServices.Category_GetAll();
            }
            catch (Exception ex)
            {
                errorMessages.Add(GetInnerException(ex).Message);
            }
        }

        private void FetchReset()
        {
            productPage = 0;
            FetchProducts();
        }


        private void FetchProducts()
        {
            feedback = string.Empty; 
            errorMessages.Clear();
            products.Clear();

            if (categoryID <= 0)
            {
                feedback = "You must provide a valid category if you expect to receive product info!";
            }
            else
            {
                try
                {
                    totalProductCountForCategory = ProductServices.Product_GetCountForCategory(categoryID);

                    products = ProductServices.Product_GetProductsByCategory(categoryID, productPage, productsPerPage);
                }
                catch(Exception ex)
                {
                    errorMessages.Add(GetInnerException(ex).Message);
                }
            }
        }

        private void Previous()
        {
            if (productPage > 0)
            {
                --productPage;
                FetchProducts();
            }
        }

        private void Next()
        {
            if ((productPage + 1) * productsPerPage < totalProductCountForCategory)
            {
                ++productPage;
                FetchProducts();
            }
        }



        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}
