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

        public List<Category> Category_GetAll()
        {
            return _westWindContext.Categories
                                   .OrderBy(category => category.CategoryName)
                                   .ToList();
        }
    }
}
