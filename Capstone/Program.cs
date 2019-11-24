using Microsoft.Extensions.Configuration;
using Security.BusinessLogic;
using Security.DAO;
using System;
using System.IO;
using Capstone.Menus;
using Capstone.DAO;

namespace Capstone
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get the connection string from the appsettings.json file
                IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                string connectionString = configuration.GetConnectionString("Npcampground");

                Menu menu = new Menu(connectionString);
                menu.MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
