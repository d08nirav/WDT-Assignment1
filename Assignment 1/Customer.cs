using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_1
{
    static class Customer
    {
        internal static void CustomerMenu()
        {
            Console.Clear();
            Console.WriteLine("List of Stores");
            Console.WriteLine("1. Display All Stock Requests");
            Console.WriteLine("2. Display Owner Inventory");
            Console.WriteLine("3. Reset Inventory");
            Console.WriteLine("4. Return To Main Menu");
            Console.Write("\nEnter an option: ");
        }
    }
}
