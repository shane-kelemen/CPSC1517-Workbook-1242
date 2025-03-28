using DBInteractionSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractionSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBInteractionSystem.BLL
{
    public class ShipmentServices
    {
        private readonly WestWindContext _westWindContext;

        internal ShipmentServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }

        public List<Shipment> Shipment_GeyByYearAndMonth(int year, int month)
        {
            List<Exception> errorMessages = new List<Exception>();

            if(year < 1950 || year > DateTime.Today.Year)
            {
                errorMessages.Add(new Exception("The input year must be greater than or equal to 1950 and either today or before!"));
            }

            if(month < 1 || month > 12)
            {
                errorMessages.Add(new Exception("You must supply a valid month number!"));
            }

            if (errorMessages.Count > 0)
            {
                throw new AggregateException("There was an error(s) with your request:", errorMessages);
            }

            return _westWindContext.Shipments
                                   .Include(shipment => shipment.ShipViaNavigation)
                                   .Where(shipment => shipment.ShippedDate.Year == year
                                                        && shipment.ShippedDate.Month == month)
                                   .ToList();
        }
    }
}
