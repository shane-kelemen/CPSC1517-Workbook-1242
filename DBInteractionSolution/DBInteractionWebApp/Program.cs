using DBInteractionWebApp.Components;
using DBInteractionSystem;
using Microsoft.EntityFrameworkCore;

namespace DBInteractionWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // THESE TWO LINES ARE THE ONLY ONES ADDED FOR CONNECTION TO THE DATABASE FROM THE WEB APP
            // Retrieve the connection string from the appsettings.json
            // The connection string will be passed to the class library using the encapsulated
            //      extension method
            // The connection string will be registered to get access to the database
            var connectionString = builder.Configuration.GetConnectionString("WestWindConnection");
            // Call the ExtensionsServices method in your system library to provide the connection string
            // from the web application that you wish to use to communicate with the database.
            builder.Services.WestWindExtensionServices(options => options.UseSqlServer(connectionString));


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
