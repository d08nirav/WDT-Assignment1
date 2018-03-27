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
                        GetInventory();
                        Console.Write("Press any key to Continue: ");
                        Console.Read();
                        break;
                    case 2:
                        FranchiseHolder.FranchiseHolderMenu();
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
            Console.WriteLine("Inventory");
            Console.WriteLine("ID     Product                        CurrentStock");
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine(" {0,-5} {1,-30} {2,-11}",
                                              row["ID"],
                                              row["Product"],
                                              row["Current Stock"]);
            }
        }

        private static void GetInventory(string storeName)
        {
            throw new NotImplementedException();
        }

        private static string GetStoreName(int choise)
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

        private static void GetStores()
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
