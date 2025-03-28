using DBInteractionSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractionSystem.Entities;

namespace DBInteractionSystem.BLL
{
    public class RegionServices
    {
        // This set up is the same in every system service file in your BLL folder
        // The only thing that will change is the name of the context being used
        private readonly WestWindContext _westWindContext;

        internal RegionServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }



        // This method will retrieve all of the regions from the database connected via the context
        public List<Region> Region_GetAll()
        {
            // Note that there is no data validation or business rules initial section because this
            // method does not require any input data

            // Separate commands to accomplish the end result, but not really needed          
                    //List<Region> regions = new List<Region>();

                    // Retrieve all records from the Regions table in the database
                    //regions = _westWindContext.Regions.ToList();

                    // Order the results by the region descriotion and return the results
                    //regions = regions.OrderBy(region => region.RegionDescription).ToList();
                                                // The code above this comment, in the parentheses, is called a
                                                // lambda expression, which is a short form of an anonymous method.
                                                // The first item before the => operator is the name of a variable 
                                                // to be used in the body of the lambda expression. In this case it
                                                // represents one row from the collection of regions that was returned
                                                // from the database.  The properties inside the row may be accessed
                                                // using the dot operator just like you are accessing a class instance.
                                                // in this case, the lambda expression is being used in the OrderBy LINQ
                                                // method to tell the compiler by which property to order the rows in
                                                // the collection.
                    //return regions;


            // Using chaining of method calls, we can accomplish the same task above a lot cleaner.
            // Remember that each method is going to return a new collection
            return _westWindContext.Regions
                                   .OrderBy(region => region.RegionDescription)
                                   .ToList();

        }


        // This method will return only a single region row from the database depending on the
        // region ID that was accepted as input data.
        public Region Region_GetByID(int regionID)
        {
            // This method also does not require a data validation and business rules section,
            // as all integers may be used.  If no results are returned from the database because there
            // is no matching region to the supplied regionID, than a null will be returned from the method.

            // As in the previous method, the verbose way of retrieving the desired information follows
                    // Create a variable of type Region to hold the  retrieved information
                    //Region region = new Region();     

                    // Use the context to retrieve the desired information from the database
                    //region = _westWindContext.Regions
                    //                         .Where(region => region.RegionID == regionID)        //     Use the Where LINQ method to filter the results to only the one region desired,
                                                                                                    //     in this case by matching the RegionID to the accepted regionID.
                                                                                                    //     Note the smilar use of the lambda statement, but this time we are performing
                                                                                                    //     a comparison rather than just indicating which property to look at
                    //                         .FirstOrDefault();                                   // Use the FirstOrDefault() method to convert the returned object into a Region
                                                                                                    //     object for assigning to the region variable.

                    //return region;    // Return the Region object from the method


            // The following shows how the above commands may all be combined into a single executable
            // statement for a more streamlined method.
            return _westWindContext.Regions.Where(region => region.RegionID == regionID)
                                           .FirstOrDefault();

        }
    }
}
