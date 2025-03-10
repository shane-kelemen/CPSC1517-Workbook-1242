using EmploymentSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorWebApp.Components.Pages.ExamplePages
{
    public partial class EmploymentReport
    {
        // Standard fields for the feedback and error messages sections on the razor page
        private string feedback = "";
        List<string> errorMessages = new List<string>();

        // The list that will be examined to determine what to display on the front page.  Will
        // hold all of the valid Employment instances we are able to Parse from the file.
        List<Employment> employments = new List<Employment>();


        protected override void OnInitialized()
        {
            base.OnInitialized();

            ReadEmploymentsFromFile();   // Call the file Parsing method on page load.
        }

        private void ReadEmploymentsFromFile()
        {
            // Reset the user messaging fields
            feedback = "";
            errorMessages.Clear();

            // On this page we will specify the working directory (current content directory) differently than the DataEntry page
            // This type of pathing will start at the top of your web application (meaning the root directory).
            string filePath = @"./Data";

            // Files for testing different things.  Note that these are all in the Data folder
            // 0 - The good file.  All data is valid and so will display without errors
            // 1 - An Empty file to have our empty List message display
            // 2 - A file with an incorrect file extension.  Should cause the invalid file message to display
            // 3 - A non-existant file.  Should cause the invalid file message to display
            // 4 - A file with one or more rows that are invalid.  The valid rows will Parse and display correctly.
            //     Some errors for the bad lines will also be displayed.  If all bad lines, then the empty list message
            //     would display with the errors instead of a table.
            string[] filenames = { "Employment.csv", "Empty.csv", "TextFile.txt", "", "BadData.csv"};
            string fullFileName = @$"{filePath}/{filenames[2]}";  // Adjust the index to one of the above to see the different results.

            string[] rawEmploymentDataLines = null;     // Array for holding the results of the ReadAllLines() operation successfully read
            Employment employment = null;               // Object to hold each created employment instance when Parsed
            int recordIndex = 1;                        // For tracking which line of the file we are reading.  Wanted to write more complete
                                                        // error message when a bad Parse is encountered.

            try
            {
                // Attempt to run the Parse if the file exists and it has the correct file extension (.csv)
                if (File.Exists(fullFileName) && Path.GetExtension(fullFileName) == ".csv")
                {
                    employments = new List<Employment>();   // Create the List so that the bad file message does not display
                                                            // Still allows the No Employments message to display

                    rawEmploymentDataLines = File.ReadAllLines(fullFileName);  // Read all of the data from the file

                    // Attempt to Parse each line in the file to a valid Employment instance
                    foreach (string rawDataLine in rawEmploymentDataLines)
                    {
                        try
                        {
                            //employment = Employment.Parse(rawDataLine);
                            //employments.Add(employment);

                            // This one line is equivalent to the two separate lines above.
                            employments.Add(Employment.Parse(rawDataLine));
                        }
                        catch (FormatException ex)
                        {
                            // Add an error message to the user indicating the row that had a problem.
                            errorMessages.Add($"RawEmploymentsData Line : {recordIndex} : {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add(GetInnerException(ex).Message);
                        }

                        ++recordIndex;  // Always increase the recordIndex
                    }
                }
                else
                {
                    errorMessages.Add($"File {fullFileName} does not exist or wrong extension!");
                }
            }
            catch (Exception ex) 
            {
                errorMessages.Add(GetInnerException(ex).Message);   // For displaying error messages we cannot predict.
                                                                    // Usually file operation mishaps.
            }

        }

        /// <summary>
        /// Method for getting to the root cause of an Exception
        /// </summary>
        /// <param name="ex">Provide the Highest Level exception to be mined for the root cause</param>
        /// <returns></returns>
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
