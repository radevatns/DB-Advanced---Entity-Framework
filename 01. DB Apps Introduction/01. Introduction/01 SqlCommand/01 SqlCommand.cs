using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_SqlCommand
{
    class Program
    {
        static void Main(string[] args)
        {

            // option 1
            //SqlConnection connection = new SqlConnection( 
            //"Server=.;" + 
            //  "Database=SoftUni;" +
            //    "Integrated Security=true");
            SqlConnection connection = new SqlConnection(@"
            Server=.;
              Database=SoftUni;;
                Integrated Security=true");
            connection.Open();

            using (connection)
            {
                //commant here
                string query = "SELECT COUNT(*) FROM Employees";
                SqlCommand cmd = new SqlCommand(query, connection);
                int employeesCount = (int)cmd.ExecuteScalar();
                Console.WriteLine("Employees count: {0} ", employeesCount);
            }
    }
}
