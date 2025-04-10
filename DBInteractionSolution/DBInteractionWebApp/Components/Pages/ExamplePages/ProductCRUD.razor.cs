using Microsoft.AspNetCore.Components;
using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;
using Microsoft.AspNetCore.Components.Forms;
using DBInteractionWebApp.Utilities;

namespace DBInteractionWebApp.Components.Pages.ExamplePages
{
    public partial class ProductCRUD
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                     // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();    // For storing error messages to be displayed on the razor page

        private Product currentProduct = new Product();
        private List<Category> categories = new List<Category>();
        private List<Supplier> suppliers = new List<Supplier>();

        private EditContext editContext;
        private ValidationMessageStore validationMessageStore;


        [Parameter]
        public int? productID { get; set; }

        [Inject]
        private ProductServices ProductServices { get; set; }

        [Inject]
        private CategoryServices CategoryServices { get; set; }

        [Inject]
        private SupplierServices SupplierServices { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            editContext = new EditContext(currentProduct);
            validationMessageStore = new ValidationMessageStore(editContext);

            categories = CategoryServices.Category_GetAll();
            suppliers = SupplierServices.Supplier_GetAll();

            if (productID.HasValue)
                currentProduct = ProductServices.Product_GetByID(productID.Value);
        }


        private void OnCreate()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            validationMessageStore.Clear();

            currentProduct.ProductID = 0;

            try
            {
                if (editContext.Validate())
                {
                    if (currentProduct.CategoryID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.CategoryID)),
                             "You must select a category");
                    }

                    if (currentProduct.SupplierID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.SupplierID)),
                             "You must select a supplier");
                    }

                    if (currentProduct.UnitPrice <= 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.UnitPrice)),
                             "The unit must be greater than 0");
                    }

                    if (currentProduct.UnitsOnOrder < 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.UnitsOnOrder)),
                             "The unit must be greater than or equal to 0");
                    }

                    if (editContext.GetValidationMessages().Any())
                    {
                        editContext.NotifyValidationStateChanged();
                    }
                    else
                    {
                        int newProductID = ProductServices.Product_Add(currentProduct);

                        feedback = $"Product : {currentProduct.ProductName} (ID : {newProductID} has been added!";
                    }     
                }
            }
            catch (ArgumentNullException ex)
            {
                errorMessages.Add(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception innerEx in ex.InnerExceptions)
                {
                    errorMessages.Add(innerEx.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessages.Add(ExceptionHelper.GetInnerException(ex).Message);
            }
        }

        private void OnUpdate()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                if (editContext.Validate())
                {
                    if (currentProduct.CategoryID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.CategoryID)),
                             "You must select a category");
                    }

                    if (currentProduct.SupplierID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.SupplierID)),
                             "You must select a supplier");
                    }

                    if (currentProduct.UnitPrice <= 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.UnitPrice)),
                             "The unit must be greater than 0");
                    }

                    if (currentProduct.UnitsOnOrder < 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.UnitsOnOrder)),
                             "The unit must be greater than or equal to 0");
                    }

                    if (editContext.GetValidationMessages().Any())
                    {
                        editContext.NotifyValidationStateChanged();
                    }
                    else
                    {
                        int rowsAffected = ProductServices.Product_Update(currentProduct);

                        if (rowsAffected != 0)
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID} has been updated!";
                        }
                        else
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID} has NOT been updated! " +
                                            $"Check that the product is still on file!";
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                errorMessages.Add(ex.Message);
            }
            catch (ArgumentException ex)
            {
                errorMessages.Add(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception innerEx in ex.InnerExceptions)
                {
                    errorMessages.Add(innerEx.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessages.Add(ExceptionHelper.GetInnerException(ex).Message);
            }
        }
    }
    






    // CRUD -   CREATE  RETRIEVE    UPDATE  DELETE
    // SQL -    INSERT  SELECT      UPDATE  DELETE
    // HTTP -   POST    GET         PUT     DELETE

    // RESTful Web Design Philosophy
    // Works off the HTTP protocol

    // Format of HTTP Request 

    // GET thor.cnt.sast.ca/forProcessing.php HTTP/1.1
    // headers:
    //
    // Payload - name/value pairs of information to the server


    // Format of HTTP Response

    // HTTP/1.1 200 OK
    // headers:
    //
    // Payload - whatever the resource was that was requested
}
