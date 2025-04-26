using Microsoft.AspNetCore.Components;
using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;
using Microsoft.AspNetCore.Components.Forms;
using DBInteractionWebApp.Utilities;
using Microsoft.JSInterop;

namespace DBInteractionWebApp.Components.Pages.ExamplePages
{
    public partial class ProductCRUD
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                     // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();    // For storing error messages to be displayed on the razor page

        private Product currentProduct = new Product();             // For storing the current product information
        private List<Category> categories = new List<Category>();   // For storing the list of all available categories
        private List<Supplier> suppliers = new List<Supplier>();    // For storing the list of all available suppliers

        private EditContext editContext;                            // To support the EditForm on the razor page
        private ValidationMessageStore validationMessageStore;      // For storing the messages to be displayed with the controls


        [Parameter]
        public int? productID { get; set; }                         // This is how we get a data piece that was passed into the
                                                                    // page via the URL, specified in the @page directive.  Make 
                                                                    // sure the data types match.  In this case we want to be able
                                                                    // to access the page even if no information was available, 
                                                                    // so we used a nullable data type, indicated by the ? after 
                                                                    // the data type name.

        [Inject]
        private ProductServices ProductServices { get; set; }       // Grants access to the methods in the ProductServices class of
                                                                    // the system library.  Remember the [Inject].

        [Inject]
        private CategoryServices CategoryServices { get; set; }     // Grants access to the methods in the CategoryServices class of
                                                                    // the system library.  Remember the [Inject].

        [Inject]
        private SupplierServices SupplierServices { get; set; }     // Grants access to the methods in the SupplierServices class of
                                                                    // the system library.  Remember the [Inject].

        [Inject]
        private IJSRuntime JSRuntime { get; set; }                  // Making available a dialog that will wait for a user's response


        [Inject]
        private NavigationManager NavigationManager { get; set; }   // Allows us to jump to another page in the application


        /// <summary>
        /// Time to Initialize the Blazor page controls
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // We only want to retrieve product information if we were given a product ID.  If it was not provided, then
            // the productID Parameter we defined above will be null and HasValue will be false.
            // Note:  Remember that this step must be completed before the EditContext is crearted or your update
            //        will not function correctly because the original information will not be bound to the EditContext
            //        even though you can see the information in the controls.
            if (productID.HasValue)
                currentProduct = ProductServices.Product_GetByID(productID.Value);

            // The following are required to make validation of data on the razor page possible
            editContext = new EditContext(currentProduct);
            validationMessageStore = new ValidationMessageStore(editContext);

            // Retrieve a List of all the Categories and Suppliers to poulate the select controls
            categories = CategoryServices.Category_GetAll();
            suppliers = SupplierServices.Supplier_GetAll();      
        }


        /// <summary>
        /// This method will collect the information from the razor page and send it to the ProductService
        /// class so that a database addition may be attempted.
        /// </summary>
        private void OnCreate()
        {
            // Reset all of the fields for feedback, error messaging, previous displayed validator errors,
            // and the ProductID in case a previous submit was already successful which would cause the 
            // ProductID to be something other than zero.
            feedback = string.Empty;
            errorMessages.Clear();
            validationMessageStore.Clear();

            // UPDATE!  The following line has been commented out as one part of fixing the
            //          tracking issue people have been experiencing.  Not setting the
            //          ProductID = 0 will allow us to check in the insert if the product information 
            //          should be used to attempt an insert.  If it is zero, then yes.  If it is not 
            //          zero then no.  If no, the service method should throw an exception.  This will
            //          also allow us to update or discontinue the product without issues.
            //currentProduct.ProductID = 0;       // This is what we check to indicate we have an insert operation

            try
            {
                // When a submit is attempted
                if (editContext.Validate())
                {
                    // Check for illegal or missing data in any of the fields that you wish to check on the
                    // client side.  Checking on the client side helps increase user experience by perventing
                    // excessive round trips to the server, each of which is more noticable for time lag.
                    if (currentProduct.CategoryID == 0)
                    {
                        // For any item where the data is unacceptable, add an error message to the message store
                        // using the following pattern.  Notice it is consistent for all three checks included here
                        // aside from the item being accessed in the CurrentProduct and the message text.
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

                    // If there are any messages for the editContext
                    if (editContext.GetValidationMessages().Any())
                    {
                        // Have those messages displayed
                        editContext.NotifyValidationStateChanged();
                    }
                    // If there are no messages, proceed with a call to the ProductServices method for adding
                    // a new Product to the database.  Display success feedback if no Exceptions are thrown.
                    else
                    {
                        int newProductID = ProductServices.Product_Add(currentProduct);

                        feedback = $"Product : {currentProduct.ProductName} (ID : {newProductID}) has been added!";
                    }     
                }
            }
            catch (ArgumentNullException ex)        // Triggered if no Product instance is provided to the service method
            {
                errorMessages.Add(ex.Message);
            }
            catch (AggregateException ex)           // Triggered if data validation, business rules, or temporary data modifications encountered
            {
                foreach (Exception innerEx in ex.InnerExceptions)
                {
                    errorMessages.Add(innerEx.Message);
                }
            }
            catch (Exception ex)                    // Triggered if the final SaveChanges() operation throws an unexpected exception.
            {
                errorMessages.Add(ExceptionHelper.GetInnerException(ex).Message);
            }
        }


        /// <summary>
        /// The following method will collect data from the razor page and send the values to the service
        /// for the purpose of the updating the database table record.        
        /// </summary>
        /// 
        //  NOTE:  The internal workings are virtually the same as for OnCreate()
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
                                            $"(ID : {currentProduct.ProductID}) has been updated!";
                        }
                        else
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has NOT been updated! " +
                                            $"Check that the product is still on file!";
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                errorMessages.Add(ex.Message);
            }
            catch (ArgumentException ex)            // Triggered if the supplied product does not exist in the database
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

        /// <summary>
        /// This method will call the Discontinue method in the service method in order to perform a 
        /// "SOFT/LOGICAL DELETE" of the database record by updating the Discontinued flag of the 
        /// product to true.
        /// </summary>
        private void OnDiscontinue()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                if (editContext.Validate())
                {
                    if (currentProduct.SupplierID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.SupplierID)),
                             "You must select a supplier");
                    }

                    if (editContext.GetValidationMessages().Any())
                    {
                        editContext.NotifyValidationStateChanged();
                    }
                    else
                    {
                        int rowsAffected = ProductServices.Product_Discontinue(currentProduct);

                        if (rowsAffected != 0)
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has been discontinued!!";
                        }
                        else
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has NOT been discontinued! " +
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


        /// <summary>
        /// This method will call the Activate method in the service method in order to perform a 
        /// "SOFT/LOGICAL UNDELETE" or "REACTIVATION" of the database record by updating the Discontinued 
        /// flag of the product to false.
        /// </summary>
        private void OnActivate()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                if (editContext.Validate())
                {
                    if (currentProduct.SupplierID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.SupplierID)),
                             "You must select a supplier");
                    }

                    if (editContext.GetValidationMessages().Any())
                    {
                        editContext.NotifyValidationStateChanged();
                    }
                    else
                    {
                        int rowsAffected = ProductServices.Product_Activate(currentProduct);

                        if (rowsAffected != 0)
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has been reactivated!!";
                        }
                        else
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has NOT been reactivated! " +
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


        /// <summary>
        /// This method will perform a PHYSICAL DELETE of a record in the database.  This should not be taken
        /// lightly, and generally is only performed after the data to be deleted has been archived.
        /// </summary>
        private void OnDelete()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                if (editContext.Validate())
                {
                    if (currentProduct.SupplierID == 0)
                    {
                        validationMessageStore
                            .Add(editContext.Field(nameof(currentProduct.SupplierID)),
                             "You must select a supplier");
                    }

                    if (editContext.GetValidationMessages().Any())
                    {
                        editContext.NotifyValidationStateChanged();
                    }
                    else
                    {
                        int rowsAffected = ProductServices.Product_Delete(currentProduct);

                        if (rowsAffected != 0)
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has been deleted!!";
                        }
                        else
                        {
                            feedback = $"Product : {currentProduct.ProductName} " +
                                            $"(ID : {currentProduct.ProductID}) has NOT been deleted! " +
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

        /// <summary>
        /// This method will pop up a dialog that will seek input from the user.  In this case, 
        /// we are seeking verification that the user wishes to clear the data from the form.
        /// </summary>
        public async Task OnClear()
        {
            // Set up the message to be displayed to the user.
            object[] messageLine = new object[] {"Clearing will cause all unsaved data to be lost. " +
                "Are you sure you want to clear the form?"};

            // Cause the dialog to appear.  If the user presses OK, then "confirm" has taken place and the 
            // if statement will run.
            if (await JSRuntime.InvokeAsync<bool>("confirm", messageLine))
            {
                // Reset all of the important values for the page.
                feedback = string.Empty;
                errorMessages.Clear();
                validationMessageStore.Clear();
                currentProduct = new Product();

                editContext = new EditContext(currentProduct);
            }
        }

        /// <summary>
        /// This method will pop up a dialog that will seek input from the user.  In this case, 
        /// we are seeking verification that the user wishes to leave the current page.
        /// </summary>
        public async Task OnGoTo()
        {
            // Set up the message to be displayed to the user.
            object[] messageLine = new object[] {"Leaving will cause all unsaved data to be lost. " +
                "Are you sure you want to leave the page?"};

            // Cause the dialog to appear.  If the user presses OK, then "confirm" has taken place and the 
            // if statement will run.
            if (await JSRuntime.InvokeAsync<bool>("confirm", messageLine))
            {
                // Jump to the Category Products page
                // (same as the @page directive for the page we want to go to)
                NavigationManager.NavigateTo("categoryProducts");
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
