using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace Assignment_1
{
    class Global
    {
        public static IConfigurationRoot Configuration { get; } = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public static string ConnectionString { get; } = Configuration["ConnectionString"];
        internal static SqlConnection sqlConnection = new SqlConnection(ConnectionString);
        
        //internal

        public static void PrintInvalidInputErrorMSG()
        {
            Console.WriteLine("\nNot Valid Input. Try Again");
        }
    }
}
