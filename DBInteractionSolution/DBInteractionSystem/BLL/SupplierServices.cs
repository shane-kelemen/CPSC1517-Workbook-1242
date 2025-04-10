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

        public List<Supplier> Supplier_GetAll()
        {
            return _westWindContext.Suppliers
                                   .OrderBy(supplier => supplier.CompanyName)
                                   .ToList();
        }
        
    }
}
