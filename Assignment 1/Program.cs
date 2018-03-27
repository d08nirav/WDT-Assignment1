using System;

namespace Assignment_1
{
   
    class Program
    {
        
        /* Not working 
            1.1 and 1.3*/
        static void Main()
        {
            // Variables
            int choise = 0;

            while (choise!=4)
            {
                PrintMainMenu();
                while (!Int32.TryParse(Console.ReadLine(), out choise))
                {
                    Global.PrintInvalidInputErrorMSG();
                    PrintMainMenu();
                }
                switch (choise)
                {
                    case 1:
                        Owner.OwnerMenu();
                        break;
                    case 2:
                        FranchiseHolder.FranchiseHolderMenu();
                        break;
                    case 3:
                        Customer.CustomerMenu();
                        break;
                    case 4:
                        break;
                    default:
                        Global.PrintInvalidInputErrorMSG();
                        break;

                }

            }

        

        Console.WriteLine("Thanks for Using the System!");
            Console.WriteLine("");
        }

        

        

        private static void PrintMainMenu()
        {
            Console.WriteLine("Welcome to Marvelous Magic");
            Console.WriteLine("==========================");
            Console.WriteLine("1. Owner");
            Console.WriteLine("2. Franchise Holder");
            Console.WriteLine("3. Customer");
            Console.WriteLine("4. Quit");
            Console.Write("\nEnter an option: ");
        }
    }
}
