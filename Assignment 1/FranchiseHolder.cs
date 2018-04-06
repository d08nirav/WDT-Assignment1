using System;
using System.Data.SqlClient;
using System.Data;

namespace Assignment_1
{
    static class FranchiseHolder
    {
        internal static int StoreID;
        internal static string storeName = "Not Valid Input. Try Again";
        internal static void FranchiseHolderMenu()
        { 
            Console.Clear();
            Console.WriteLine("Stores");
            GetStores();            
            while (storeName.Equals("Not Valid Input. Try Again"))
            {
                Console.Write("Enter the Store to use: ");
                if (Int32.TryParse(Console.ReadLine(), out StoreID))
                {
                    storeName = GetStoreName(StoreID);
                }
                if (storeName.Equals("Not Valid Input. Try Again"))
                    Global.PrintInvalidInputErrorMSG();
            }
            PrintFranchiseHolderSubMenu();
        }

        private static void PrintFranchiseHolderSubMenu()
        {
            int choise =0;
            while (choise != 4)
            {
                Console.Write(
    "\nWelcome to Marvelous Magic (Franchise Holder - " + storeName + ")\n" +
@"==========================
1. Display Inventory
2. Stock Request (Threshold)
3. Add New Inventory Item
4. Return To Main Menu

Enter an option: ");
                
                while (!Int32.TryParse(Console.ReadLine(), out choise))
                {
                    Global.PrintInvalidInputErrorMSG();
                }
                switch (choise)
                {
                    case 1:
                        GetInventory(-1);
                        Console.Write("Press any key to Continue: ");
                        Console.ReadKey();
                        break;
                    case 2:
                        int threshold,StockID=0;
                        Console.Write("Enter threshold for re-stocking: ");
                        while (!Int32.TryParse(Console.ReadLine(), out threshold))
                        {
                            Global.PrintInvalidInputErrorMSG();
                        }
                        if (!GetInventory(threshold))
                        {
                            break;
                        }
                        string op;
                        Console.Write("Enter request to Process: ");
                        while ((!Int32.TryParse(Console.ReadLine(), out StockID)) || ((op = RequestStock(threshold, StockID, StoreID)).Equals("Not Valid Input. Try Again")))
                        {                            
                                Global.PrintInvalidInputErrorMSG();                           
                        }
                        Console.Write(op+"\nPress any key to Continue: ");
                        Console.ReadKey();
                        break;
                    case 3:
                        Customer.CustomerMenu();
                        break;
                    case 4:
                        StoreID = 0;
                        storeName = "Not Valid Input. Try Again";
                        break;
                    default:
                        Global.PrintInvalidInputErrorMSG();
                        break;
                }

            }

        }

        private static string RequestStock(int threshold, int StockID,int StoreID)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("requestStock", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@StoreID", StoreID));
                sqlCommand.Parameters.Add(new SqlParameter("@StockID", StockID));
                sqlCommand.Parameters.Add(new SqlParameter("@threshold", threshold));
                sqlCommand.Parameters.Add(new SqlParameter("@result", SqlDbType.NVarChar, 30)).Direction = ParameterDirection.Output;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
                return sqlCommand.Parameters["@result"].Value.ToString();
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return ("Not Valid Input. Try Again");
            }
            finally
            {
                sqlCommand.Dispose();
                Global.sqlConnection.Close();
            }
        }

        private static Boolean GetInventory(int threshold)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("getInventory", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@StoreID", StoreID));
                sqlCommand.Parameters.Add(new SqlParameter("@threshold", threshold));
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                Global.PrintInvalidInputErrorMSG();
                return true;
            }
            finally
            {
                sqlCommand.Dispose();
                Global.sqlConnection.Close();
            }
            if (table.Rows.Count <=0)
            {
                if (threshold != -1)
                    Console.WriteLine("All invnetory stock levels are equal to or above " + threshold);
                else
                    Console.WriteLine("No Inventory to Display.");
                return false;
            }
            else
            {
                Console.WriteLine("Inventory");
                Console.WriteLine("ID     Product                        CurrentStock");
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine(" {0,-5} {1,-30} {2,-11}",
                                                  row["ID"],
                                                  row["Product"],
                                                  row["Current Stock"]);
                }
                return true;
            }
        }

        private static void GetInventory(string storeName)
        {
            throw new NotImplementedException();
        }

        internal static string GetStoreName(int choise)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("getStoreName", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@StoreID", choise));
                sqlCommand.Parameters.Add(new SqlParameter("@StoreName", SqlDbType.NVarChar, 30)).Direction = ParameterDirection.Output;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
                return sqlCommand.Parameters["@StoreName"].Value.ToString();                
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return ("Not Valid Input. Try Again");
            }
            finally
            {
                sqlCommand.Dispose();
                Global.sqlConnection.Close();
            }
        }

        internal static void GetStores()
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("getStores", Global.sqlConnection);
               Global.sqlConnection.Open();                
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                Global.PrintInvalidInputErrorMSG();
            }
            finally
            {
                sqlCommand.Dispose();
                Global.sqlConnection.Close();
            }
            Console.WriteLine("ID \tName");
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("{0} \t{1}",
                                              row["ID"],
                                              row["Name"]);
            }
        }
    }
}
