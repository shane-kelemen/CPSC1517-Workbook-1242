using DBInteractionSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractionSystem.Entities;

namespace DBInteractionSystem.BLL
{
    public class SupplierServices
    {
        private readonly WestWindContext _westWindContext;

        internal SupplierServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }

        // UPDATE!  The .AsNoTracking() method was used below to ensure that entities
        //          for all the suppliers are not created in the 
        //          Entity Framework's Tracking subsystem.  This will help avoid the duplicate
        //          entity tracking error that was being caused in an edge case. 
        public List<Supplier> Supplier_GetAll()
        {
            return _westWindContext.Suppliers
                                   .OrderBy(supplier => supplier.CompanyName)
                                   .ToList();
        }
        
    }
}
