using DBInteractionSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractionSystem.Entities;

namespace DBInteractionSystem.BLL
{
    public class CategoryServices
    {
        private readonly WestWindContext _westWindContext;

        internal CategoryServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }

        // UPDATE!  The .AsNoTracking() method was used below to ensure that entities
        //          for all the categories are not created in the 
        //          Entity Framework's Tracking subsystem.  This will help avoid the duplicate
        //          entity tracking error that was being caused in an edge case. 
        public List<Category> Category_GetAll()
        {
            return _westWindContext.Categories
                                   .OrderBy(category => category.CategoryName)
                                   .ToList();
        }
    }
}
