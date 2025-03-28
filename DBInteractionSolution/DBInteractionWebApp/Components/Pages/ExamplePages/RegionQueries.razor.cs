using DBInteractionSystem.BLL;
using DBInteractionSystem.Entities;
using Microsoft.AspNetCore.Components;

namespace DBInteractionWebApp.Components.Pages.ExamplePages
{
    public partial class RegionQueries
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                     // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();    // For storing error messages to be displayed on the razor page

        int regionArg = 0;              // For storing the user's input to the numeric control
        int regionSelect = 0;           // For storing the user's selection by region description
        List<Region> regions = null;    // For storing all of the Regions to be used for populating the select control on the razor page
        Region regionInfo = null;       // For storing the information for the single region selected by the user


        // Properties
        // An Inject will be needed for every Service class from your system library that you wish to use
        [Inject]
        public RegionServices RegionServices { get; set; }


        // Similar to OnLoad / Constructor for the page
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Using the RegionServices system library class, retrieve all Region information from the database
            regions = RegionServices.Region_GetAll();
        }

        // This method will take the user's entry into the numberic control on the razor page and use it as
        // input to the system library method for retrieving information of a single region corresponding to
        // the regionID provided.
        void GetByID()
        {
            // The next three lines reset our variables for displaying information to the razor page
            feedback = "";
            errorMessages.Clear();
            regionInfo = null;

            // Here we are limiting a call to the system method to region ID greater than zero because we 
            // know all IDs are positive values.  It should be noted that the application will not crash 
            // if a negative ID is allowed, as the system method will just return a null,
            // which is the same as would happen if the positive ID provided does not match a region in the database.
            if (regionArg > 0)
            {
                regionInfo = RegionServices.Region_GetByID(regionArg);
            }
            else
            {
                errorMessages.Add("Region ID must be greater than 0!");
            }

        }

        // This method will take the user's selection from the select control on the razor page, 
        // remembering that the value of the select option will be accessed, and use it as input 
        // to the system library method for retrieving information of a single region corresponding to
        // the regionID provided.
        // The rest of the code in the method operates exactly the same as the method above.
        void GetBySelect()
        {
            feedback = "";
            errorMessages.Clear();
            regionInfo = null;

            if (regionSelect > 0)
            {
                regionInfo = RegionServices.Region_GetByID(regionSelect);
            }
            else
            {
                errorMessages.Add("Region ID must be greater than 0!");
            }
        }
    }
}
