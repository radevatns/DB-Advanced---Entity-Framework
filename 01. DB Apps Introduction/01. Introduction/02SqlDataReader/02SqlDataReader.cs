using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02SqlDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"
            Server=.;
              Database=SoftUni;;
                Integrated Security=true");
            connection.Open();

            using (connection)
            {
                //commant here
                string query = "SELECT *FROM Employees"; 
                SqlCommand cmd = new SqlCommand(query, connection);
                var reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];
                        decimal salary = (decimal)reader["Salary"];
                        Console.WriteLine("{0} {1} - {2}", firstName, lastName, salary);
                    }
                    //reader.Read();
                    //Console.WriteLine(reader[1]);
                }
            }
        }
    }
}
