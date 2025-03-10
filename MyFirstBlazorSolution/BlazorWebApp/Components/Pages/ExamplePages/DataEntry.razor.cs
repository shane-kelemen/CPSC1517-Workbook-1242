using EmploymentSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebApp.Components.Pages.ExamplePages
{
    // This must be a partial class to properly interact with the .razor page
    // The two together are considered to be the entire class definition
    public partial class DataEntry
    {
        private string employmentTitle = "";
        private DateTime startDate;
        decimal employedYears = 0;
        SupervisoryLevel employmentLevel;

        private string feedback = "";

        // Dictionaries are associative containers, which means a "value" can be accessed by
        // specifying the "Key".  Both the Key and the Value may be any data type.
        private Dictionary<string, string> errorMessages = new Dictionary<string, string>();


        // Inject blocks are needed when we wish to use services from other Assemblies in our code behind files
        // Make sure there is an appropriate using block placed at the top of the page if an error is indicated. 
        // Often no error will occur and/or the using statement will be included automatically if required.

        // IWebHostEnvironment is in Microsoft.AspNetCore.Hosting
        // Used to extract information about the current execution environment.  We are using it 
        // to extract the path of our current working directory.
        [Inject]
        private IWebHostEnvironment WebHostEnvironment { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }



        // Remember that you can look at this method sort of like a constructor, but the 
        // important thing is to remember that this method is used to set up the intitial 
        // state of the application on launch.
        protected override void OnInitialized()
        {
            base.OnInitialized();

            startDate = DateTime.Today;

            // The following are for manually testing that the Error block on the
            // Razor page is displaying correctly.
            //errorMessages.Add("One", "This is error message number one!");
            //errorMessages.Add("Two", "Another error message for the list!");
            //errorMessages.Add("Three", "Another error message for the list!");
            //errorMessages.Add("Four", "Another error message for the list!");
        }

        // Method for gathering the information from the page controls, validating the data, and storing it to the file 
        // if all data is valid.
        private void CollectEmploymentInfo()
        {
            // Standard clearing of user messaging fields
            feedback = "";
            errorMessages.Clear();

            #region Business Rules - Similar to the data validation in unit testing
            // Title must be present, must have at least one character
            // Start date must be today or in the past
            // Years of employment may not be less than zero
            if (string.IsNullOrWhiteSpace(employmentTitle))
            {
                errorMessages.Add("Title", "Employment title is required");
            }

            if (startDate > DateTime.Today)
            {
                errorMessages.Add("Start Date", "The start date must not be in the future");
            }

            if (employedYears < 0)
            {
                errorMessages.Add("Years", "Employed years may not be less than zero");
            }

            if (errorMessages.Count > 0)
            {
                // An example of a valid early return.  We have detected illegal data.
                // No reason to continue.
                return;
            }

            #endregion

            // If we get to this point, we have all legal data.  Show this in the feedback
            feedback = $"Entered data is {employmentTitle},{startDate.ToShortDateString()},{employedYears},{employmentLevel}";

            // We need to create and/or append to a file
            // Find out where the working directory is
            string appRootPath = WebHostEnvironment.ContentRootPath;
            // Add the rest of the pathing to where we want the 
            // file or where it already exists
            string csvFilePath = $@"{appRootPath}/Data/Employment.csv";

            try
            {
                // Because this instance is transient (temporarily needed), it is better to declare it in the method
                // and then let it be destroyed (go out of scope) when we are finished with it.
                Employment employment = new Employment(employmentTitle, employmentLevel, startDate, (double)employedYears);

                // This line arguably is not needed.  Could combine with the next line.
                // The ToString() may also be eliminated because it will be called automagically
                // by the runtime.
                string fileLine = employment.ToString() + "\n";
               
                File.AppendAllText(csvFilePath, fileLine);

                //File.AppendAllText(csvFilename, $"{employment}\n");  // A one-line version of the previous two lines of code
            }
            // The following catch blocks have been included so we may display different message dependent on the type 
            // of Exception encountered
            catch (FormatException ex)
            {
                errorMessages.Add($"Format Error : {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (ArgumentNullException ex)
            {
                errorMessages.Add($"Null Error : {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                errorMessages.Add($"Out Of Range Error : {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (ArgumentException ex)
            {
                errorMessages.Add($"Argument Error : {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (Exception ex)  // Always have one block that can catch all Exceptions
            {
                errorMessages.Add($"General Error : {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }

        }


        /// <summary>
        /// Method for getting to the root cause of an Exception
        /// </summary>
        /// <param name="ex">Provide the Highest Level exception to be mined for the root cause</param>
        /// <returns></returns>
        private Exception GetInnerException(Exception ex)
        {
            while ( ex.InnerException != null )
            {
                ex = ex.InnerException;
            }

            return ex;
        }

        private async Task Clear()
        {
            feedback = "";

            object[] messageLine = new object[]
                { "Clearing will lose all unsaved data.  Are you sure you want to clear the form?" };

            if (await JSRuntime.InvokeAsync<bool>("confirm", messageLine))
            {
                errorMessages.Clear();

                employmentTitle = "";
                startDate = DateTime.Today;
                employedYears = 0;
                employmentLevel = SupervisoryLevel.Entry;
            }
        }

        private void GoToReport()
        {
            NavigationManager.NavigateTo("report");
        }



















        // The following block was included in order to provide one small demonstration of how
        // a Dictionary can be used.  In this case it was used to categorize the words in a string
        // by their length in letters.
        Random rng = new Random();
        Dictionary<int, List<string>> categorizedWords = new Dictionary<int, List<string>>();
        string input = "The quick brown fox jumped over the lazy dog";

        private void ProcessString()
        {
            string[] words = input.Split(' ');  // In order to "step through" the code like we did in 
                                                // class, place a breakpoint in the far left column of 
                                                // the editor using a left-click.
            foreach (string word in words)
            {
                if (!categorizedWords.ContainsKey(word.Length))
                {
                    categorizedWords.Add(word.Length, new List<string>());
                }

                categorizedWords[word.Length].Add(word);
            }
        }
    }
}
