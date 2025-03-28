using DBInteractionSystem.Entities;
using Microsoft.AspNetCore.Components;
using DBInteractionSystem.BLL;
using DBInteractionWebApp.Utilities;

namespace DBInteractionWebApp.Components.Pages.ExamplePages
{
    public partial class Shipments
    {
        private string feedback = string.Empty;                     // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();    // For storing error messages to be displayed on the razor page

        int yearArg = 0;
        int monthArg = 0;

        List<Shipment> shipments = new List<Shipment>();

        [Inject]
        public ShipmentServices ShipmentServices { get; set; }


        void GetShipmentByYearAndMonth()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            shipments = null;

            if (yearArg < 1950 || yearArg > DateTime.Today.Year)
            {
                errorMessages.Add("The input year must be greater than or equal to 1950 and either today or before!");
            }

            if (monthArg < 1 || monthArg > 12)
            {
                errorMessages.Add("You must supply a valid month number!");
            }

            if (errorMessages.Count == 0)
            {
                try
                {
                    shipments = ShipmentServices.Shipment_GeyByYearAndMonth(yearArg, monthArg);
                }
                catch (AggregateException ex)
                {
                    foreach (Exception aggregate in ex.InnerExceptions)
                    {
                        errorMessages.Add(aggregate.Message);
                    }
                }
                catch (Exception ex)
                {
                    errorMessages.Add("Unexpected System Error : " + ExceptionHelper.GetInnerException(ex));
                }
            }
        }
    }
}
