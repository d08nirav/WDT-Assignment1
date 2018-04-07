using System;
using System.Data.SqlClient;
using System.Data;

namespace Assignment_1
{
    static class Customer
    {
        internal static int StoreID;
        internal static string storeName = "Not Valid Input. Try Again";
        internal static void CustomerMenu()
        {
            Console.Clear();
            Console.WriteLine("Stores");
            FranchiseHolder.GetStores();
            string inp;
            while (storeName.Equals("Not Valid Input. Try Again"))
            {
                Console.Write("Enter the Store to use: ");
                if (Int32.TryParse(inp = Console.ReadLine(), out StoreID))
                {

                    storeName = FranchiseHolder.GetStoreName(StoreID);
                }
                else if (inp == "")
                    return;
                if (storeName.Equals("Not Valid Input. Try Again"))
                    Global.PrintInvalidInputErrorMSG();
            }
            PrintCustomerSubMenu();
        }

        private static void PrintCustomerSubMenu()
        {
            int choise = 0;
            while (choise != 2)
            {
                Console.Write(
    "\nWelcome to Marvelous Magic (Retail - " + storeName + ")\n" +
@"==========================
1. Display Products
2. Return To Main Menu

Enter an option: ");

                while (!Int32.TryParse(Console.ReadLine(), out choise))
                {
                    Global.PrintInvalidInputErrorMSG();
                }
                switch (choise)
                {
                    case 1:
                        GetInventory();
                        break;
                    case 2:
                        StoreID = 0;
                        storeName = "Not Valid Input. Try Again";
                        break;
                    default:
                        Global.PrintInvalidInputErrorMSG();
                        break;
                }

            }
        }

        private static void GetInventory()
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("getInventory", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@StoreID", StoreID));
                sqlCommand.Parameters.Add(new SqlParameter("@threshold", -1));
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                Global.PrintInvalidInputErrorMSG();
                //return true;
            }
            finally
            {
                sqlCommand.Dispose();
                Global.sqlConnection.Close();
            }
            if (table.Rows.Count <= 0)
            {
                    Console.WriteLine("No Inventory to Display.");
                //return false;
            }
            else
            {
                Console.WriteLine("Inventory");
                Console.WriteLine("ID     Product                        CurrentStock");
                int i = 0;
                string input;
                int[] options = new int[3];
                foreach (DataRow row in table.Rows)
                {
                    options[i] = Convert.ToInt32(row["ID"]);
                    Console.WriteLine(" {0,-5} {1,-30} {2,-11}",
                                                  row["ID"],
                                                  row["Product"],
                                                  row["Current Stock"]);
                    
                    if (i == 2 || table.Rows.Count == table.Rows.IndexOf(row)+1)
                    {                        
                        i = 0;
                        bool flg = false;
                        Console.WriteLine("[Legend: 'N' Next Page | 'R' Return to Menu]");
                        Console.Write("Enter priduct ID to purcase or function: ");
                        while (!flg)
                        {
                            input = Console.ReadLine();
                            
                            if (options[0].ToString() == input || input == options[1].ToString() || input == options[2].ToString())
                            {
                                Console.Write("Enter quantity to purchase: ");
                                int choise;
                                while (!Int32.TryParse(Console.ReadLine(), out choise))
                                {
                                    Global.PrintInvalidInputErrorMSG();
                                }
                                PurchaseStock(int.Parse(input),choise);
                                Console.Write("Press any key to Continue: ");
                                Console.ReadKey();
                                return;                                
                            }
                            else if ((input == "N" || input == "n")&& table.Rows.Count != table.Rows.IndexOf(row) + 1)
                            {
                                Console.WriteLine("Inventory");
                                Console.WriteLine("ID     Product                        CurrentStock");
                                flg = true;
                            }
                            else if (input == "R" || input == "r")
                            {
                                return;
                            }
                            else
                            {
                                Global.PrintInvalidInputErrorMSG();
                            }
                        }
                        options = new int[3];
                    }
                    else
                        i++;

                }
            }
        }

        private static void PurchaseStock(int productID,int quantity)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("purchaseStock", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@ProductID", productID));
                
                sqlCommand.Parameters.Add(new SqlParameter("@StoreID", StoreID));
                sqlCommand.Parameters.Add(new SqlParameter("@Quantity", quantity));
                sqlCommand.Parameters.Add(new SqlParameter("@result", SqlDbType.NVarChar, 100)).Direction = ParameterDirection.Output;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
                Console.WriteLine(sqlCommand.Parameters["@result"].Value.ToString());
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                Console.WriteLine("Not Valid Input. Try Again");
            }
            finally
            {
                sqlCommand.Dispose();
                Global.sqlConnection.Close();
            }
        }
    }
}
