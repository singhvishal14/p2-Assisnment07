using System;
using System.Data;
using System.Data.SqlClient;

namespace DisconnCRUD
{
    internal class Program
    {
        static SqlConnection con;
        static SqlDataAdapter adapter;
        static DataSet ds;
        static string constring = "server=DESKTOP-FUGQNF4;database=LibraryDB;trusted_connection=true;";

        public static void InsertBook(DataTable dt)
        {
            DataRow dr = dt.NewRow();

            Console.WriteLine("Enter BookId");
            dr["BookId"] = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Title");
            dr["Title"] = Console.ReadLine();
            Console.WriteLine("Enter Author");
            dr["Author"] = Console.ReadLine();
            Console.WriteLine("Enter Genre");
            dr["Genre"] = Console.ReadLine();
            Console.WriteLine("Enter Quantity");
            dr["Quantity"] = int.Parse(Console.ReadLine());
            dt.Rows.Add(dr);
            Console.WriteLine("Book inserted successfully");
        }

        public static void UpdateBookQty(DataTable dt)
        {
            Console.WriteLine("Enter BookId to update Quantity:");
            int bookId = int.Parse(Console.ReadLine());

            DataRow[] rows = dt.Select($"BookId = {bookId}");
            if (rows.Length > 0)
            {
                DataRow dr = rows[0];

                Console.WriteLine("Enter new Quantity");
                dr["Quantity"] = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public static void DisplayAllBooks(DataTable dt)
        {
            Console.WriteLine("All Books:");
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine($"BookId: {row["BookId"]}," +
                    $" Title: {row["Title"]}," +
                    $" Author: {row["Author"]}, " +
                    $"Genre: {row["Genre"]}," +
                    $" Quantity: {row["Quantity"]}");
            }
        }


        static void Main(string[] args)
        {
            try
            {
            start:
                con = new SqlConnection(constring);
                adapter = new SqlDataAdapter("select * from Books", con);

                ds = new DataSet();   // collection of tables
                adapter.Fill(ds, "Books");

                DataTable dt = ds.Tables["Books"];

                while (true)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Insert Book");
                    Console.WriteLine("2. Update Book Quantity");
                    Console.WriteLine("3. Display All Books");

                    Console.WriteLine("4. Exit");

                    int option = int.Parse(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            InsertBook(dt);
                            break;
                        case 2:
                            UpdateBookQty(dt);
                            break;
                        case 3:
                            DisplayAllBooks(dt);
                            break;

                        case 4:
                            Console.WriteLine("Exiting the program.");
                            return;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }

                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds, "Books");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
                Console.ReadKey();
            }
        }
    }
}