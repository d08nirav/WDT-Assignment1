﻿using System;
using System.Data.SqlClient;
using System.Data;

namespace Assignment_1
{
    static class Owner
    {        
        static internal void OwnerMenu()
        {
            int choise = 0;
            
            
            while (choise != 4)
            {
                Console.Clear();
                Console.Write(
    @"Welcome to Marvelous Magic (Owner)
==========================
1. Display All Stock Requests
2. Display Owner Inventory
3. Reset Inventory
4. Return To Main Menu

Enter an option: ");

                while (!Int32.TryParse(Console.ReadLine(), out choise))
                {
                    Global.PrintInvalidInputErrorMSG();
                }
                switch (choise)
                {
                    case 1:
                        PrintStockRequest();
                        break;
                    case 2:
                        PrintOwnerInventory();
                        break;
                    case 3:
                        PrintResetStock();
                        break;
                    case 4:
                        break;
                    default:
                        Global.PrintInvalidInputErrorMSG();
                        break;
                }
            }
        }

        private static void PrintResetStock()
        {
            
            Console.WriteLine("\nReset Stock\nProduct Stock will be reset to 20.");
            GetOwnerInventory();
            Console.Write("\nEnter Product ID to reset: ");
            int choise = 0;
            string inp;
            while (!Int32.TryParse(inp = Console.ReadLine(), out choise))
            {
                if (inp == "")
                    return;
                Global.PrintInvalidInputErrorMSG();
            }
            ResetStock(choise);
        }

        private static void ResetStock(int choise)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {
                sqlCommand = new SqlCommand("resetStock", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@ProductID", choise));
                sqlCommand.Parameters.Add(new SqlParameter("@Result", SqlDbType.NVarChar,50)).Direction = ParameterDirection.Output;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(table);
                Console.WriteLine(sqlCommand.Parameters["@Result"].Value.ToString());
                Console.Write("\nPress Any Key to Continue: ");
                Console.ReadKey();
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
        }

        private static void PrintOwnerInventory()
        {
            Console.WriteLine("\nOwner Inventory");
            GetOwnerInventory();
            Console.Write("\nPress Any Key to Continue: ");
            Console.ReadKey();
        }

        private static void GetOwnerInventory()
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("getOwnerInventory", Global.sqlConnection);
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
            Console.WriteLine("ID     Product                        CurrentStock");
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine(" {0,-5} {1,-30} {2,-11}",
                                              row["ID"],
                                              row["Product"],
                                              row["Current Stock"]);
            }
        }

        private static void PrintStockRequest()
        {
            int choise = 0;
            GetStockRequest();            
            Console.Write("\nEnter an option: ");
            string inp;            
            while (!Int32.TryParse(inp = Console.ReadLine(), out choise))
            {
                if (inp == "")
                    return;
                Global.PrintInvalidInputErrorMSG();
            }
            ProcessStockRequest(choise);
        }

        private static void ProcessStockRequest(int choise)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("processStockRequest", Global.sqlConnection);
                Global.sqlConnection.Open();
                sqlCommand.Parameters.Add(new SqlParameter("@StockRequestID", choise));
                sqlCommand.Parameters.Add(new SqlParameter("@result", SqlDbType.NVarChar, 100)).Direction = ParameterDirection.Output;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sqlCommand;
                //sqlDataAdapter.
                sqlDataAdapter.Fill(table);
                Console.WriteLine(sqlCommand.Parameters["@result"].Value.ToString());
                Console.Write("Press any key to Continue: ");
                Console.ReadKey();
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
        }

        private static void GetStockRequest()
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            try
            {

                sqlCommand = new SqlCommand("getStockRequest", Global.sqlConnection);
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
            Console.WriteLine("Stock Reqqust");
            Console.WriteLine("ID \tStore \t\t     Product \t\t     Quantity \tCurrentStock   \t StockAvailability");
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("{0,-7} {1,-20} {2,-23} {3,-10} {4,-12}     {5,-20}",
                                              row["ID"],
                                              row["Store"],
                                              row["Product"],
                                              row["Quantity"],
                                              row["Current Stock"],
                                              row["Stock Availability"]);
            }
        }
    }
}
