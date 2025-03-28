using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using DBInteractionSystem.DAL;
using DBInteractionSystem.BLL;

namespace DBInteractionSystem
{
    public static class WestWindExtensions
    {
        // This class can hold a set of extension methods
        // This sample will create an extension method that will add services to 
        //      IServiceCollection
        // This method will be called from a separate project to gain data from the 
        //      WestWind database
        // In this demonstration, the call will be completed from the DBInteractionWebApp 
        //      Program.cs file
        // This method will do two things
        // 1) Register the context connection string
        // 2) Register ALL services that we create within the BLL classes
        public static void WestWindExtensionServices (this IServiceCollection services, 
                                                        Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<WestWindContext>(options);

            // For every new Service class added in the BLL folder, a new Transient will be required.
            // The only thing you should have to change is the name of the Service class.  Everything
            // else should be identical if still using the same database, and thus the same context.
            services.AddTransient<RegionServices>(serviceProvider =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new RegionServices(context);
            });

            services.AddTransient<ProductServices>(serviceProvider =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new ProductServices(context);
            });

            services.AddTransient<CategoryServices>(serviceProvider =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new CategoryServices(context);
            });

            services.AddTransient<ShipmentServices>(serviceProvider =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new ShipmentServices(context);
            });
        }

   
    }
}
