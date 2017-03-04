namespace _01Initial_Setup
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    class Program
    {
        static void Main()
        {
            SqlConnection connection = new SqlConnection(@"
            Server=.;
                Integrated Security=true");
            connection.Open();

            //string createDB = "CREATE DATABASE MinionsDB";// when create db
            //SqlCommand createDBCommand = new SqlCommand(createDB, connection);/when create db

            //CreateTables(createTablesCommand); //use this row when you want to create tables
            using (connection)
            {
               // while (true)
                {

                Console.WriteLine("Select one option with numbers from 2 to 9. Every numbers is different problem from homework:");

                //Console.WriteLine("type '69' for EXIT");
                Console.WriteLine();  
                Console.WriteLine("2.Get Villains’ Names");
                Console.WriteLine("3.Get Minion Names");
                Console.WriteLine("4.Add Minion");
                Console.WriteLine("5.Change Town Names Casing");
                Console.WriteLine("6.*Remove Villain");
                Console.WriteLine("7.Print All Minion Names");
                Console.WriteLine("8.Increase Minions Age");
                Console.WriteLine("9.Increase Age Stored Procedure");
                var numOption = int.Parse(Console.ReadLine());
                    switch (numOption)
                    {
                        case 2:
                            GetVillainsNames(connection);
                            break;
                        case 3:
                            FindMinionsByVillainId(connection);
                            break;
                        case 4:
                            AddMinion(connection);
                            break;
                        case 5:
                            ChangeTownNamesCasing(connection);
                            break;
                        case 6:
                            Console.WriteLine("I am not ready with this problem");   
                            break;
                        case 7:
                            PrintAllMinionNames(connection);
                            break;
                        case 8:
                            IncreaseMinionsAge(connection);
                            break;
                        case 9:
                            IncreaseAgeStoredProcedure(connection);
                        break;
                        case 69:
                            return;
                        default:
                            break;
                    }
                }
            }
        }

        private static void IncreaseAgeStoredProcedure(SqlConnection connection)
        {
            Console.Write("Enter the minion ID: ");
            int ageParameter = int.Parse(Console.ReadLine());
            string increaseAge =
            File.ReadAllText("../../9addAge.sql");
            SqlCommand increaseAgeCommand = new SqlCommand(increaseAge, connection);
            SqlParameter parameter = new SqlParameter("@mid", ageParameter);
            increaseAgeCommand.Parameters.Add(parameter);
            SqlDataReader reader = increaseAgeCommand.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"{(string)reader["Name"]} {(int)reader["Age"]}");
            }
        }
        private static void IncreaseMinionsAge(SqlConnection connection)
        {
            Console.WriteLine("Enter the minion IDs separated by space:");
            int[] idArrays = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            string idString = String.Join(",", idArrays);

            string increaseAge = File.ReadAllText("../../8addYearToAge.sql");
            SqlCommand increaseAgeCommand = new SqlCommand(increaseAge, connection);
            SqlParameter param1 = new SqlParameter("@idString", idString);
            increaseAgeCommand.Parameters.Add(param1);
            SqlDataReader reader = increaseAgeCommand.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{(string)reader["Name"]} {(int)reader["Age"]}");
            }
        }
        private static void PrintAllMinionNames(SqlConnection connection)
        {
            string queryCountNames = "USE MinionsDB SELECT COUNT(*) AS Count FROM Minions";
            SqlCommand printMinionNames = new SqlCommand(queryCountNames, connection);
            SqlDataReader reader = printMinionNames.ExecuteReader();
            int last = 0;
            if (reader.Read())
            {
                last = (int)reader["Count"];
            }
            reader.Close();
            string queryAllNames = "USE MinionsDB SELECT Name FROM Minions";
            SqlCommand printAllMinionNames = new SqlCommand(queryAllNames, connection);
            SqlDataReader readerNames = printAllMinionNames.ExecuteReader();
            List<string> list = new List<string>();
            while (readerNames.Read())
            {
                list.Add((string)readerNames["Name"]);
            }
            while (list.LongCount() > 0)
            {
                Console.WriteLine(list.First());
                list.RemoveAt(0);
                if (list.LongCount() > 0)
                {
                    Console.WriteLine(list.Last());
                    list.RemoveAt(last - 2);
                    last--;
                }
                last--;
            }
        }
        private static void ChangeTownNamesCasing(SqlConnection connection)
        {
            Console.Write("Country: ");
            string country = Console.ReadLine();
            string changeToUpper = File.ReadAllText("../../5changeTown.sql");
            SqlCommand changeToUpperCommand = new SqlCommand(changeToUpper, connection);
            SqlParameter commandCountryName = new SqlParameter("@countryName", country);
            changeToUpperCommand.Parameters.Add(commandCountryName);

            SqlDataReader reader = changeToUpperCommand.ExecuteReader();
            int count = 0;
            List<string> townsList = new List<string>();
            using (reader)
            {
                while (reader.Read())
                {
                    count++;
                    townsList.Add(reader[0].ToString());
                }
            }

            Console.Write(changeToUpperCommand.ExecuteNonQuery());
            if (count > 0)
            {
                Console.WriteLine(" town names were affected.");
                Console.WriteLine($"[{String.Join(", ", townsList)}]");
            }
            else
            {
                Console.WriteLine("No town names were affected.");
            }
        }
        private static void AddMinion(SqlConnection connection)
        {
            Console.Write("Minion : ");
            string[] array = Console.ReadLine().Split(' ').ToArray();
            Console.Write("Villain: ");
            string villain = Console.ReadLine();

            string isTownExist = "USE MinionsDB SELECT * FROM Towns WHERE Name = @town";
            SqlCommand isTownExistComand = new SqlCommand(isTownExist, connection);
            SqlParameter newTown = new SqlParameter("@town", array[2]);// add town
            isTownExistComand.Parameters.Add(newTown);

            SqlDataReader reader = isTownExistComand.ExecuteReader();
            if (!reader.Read())
            {
                Console.Write("Country: ");
                string countryName = Console.ReadLine();

                string addMinionQuery = File.ReadAllText("../../4AddOneMinion.sql");
                SqlCommand addMinionComand = new SqlCommand(addMinionQuery, connection);

                addMinionComand.Parameters.AddRange(new[]
                {
                        new SqlParameter("@name", array[0]),
                        new SqlParameter("@age", int.Parse(array[1])),
                        new SqlParameter("@town", array[2]),
                        new SqlParameter("@villain", villain),
                        new SqlParameter("@country", countryName)
                    });
                reader.Close();
                Console.WriteLine(addMinionComand.ExecuteNonQuery());
                Console.WriteLine($"{array[0]} and {array[2]} ware added");

            }
            else
            {
                string addMinionQuery = File.ReadAllText("../../4AddOneMinionFile2.sql");
                SqlCommand addMinionComand = new SqlCommand(addMinionQuery, connection);

                addMinionComand.Parameters.AddRange(new[]
                {
                        new SqlParameter("@name", array[0]),
                        new SqlParameter("@age", int.Parse(array[1])),
                        new SqlParameter("@town", array[2]),
                        new SqlParameter("@villain", villain)
                    });
                reader.Close();
                Console.WriteLine(addMinionComand.ExecuteNonQuery());
                Console.WriteLine($"{array[0]} was added");
            }
        }
        private static void FindMinionsByVillainId(SqlConnection connection)
        {
            string queryGetName = File.ReadAllText("../../3GetMinionNames.sql");
            SqlCommand command = new SqlCommand(queryGetName, connection);

            int inputNum = int.Parse(Console.ReadLine());
            SqlParameter villParam = new SqlParameter("@inputNum", inputNum);
            command.Parameters.Add(villParam);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string villName = (string)reader["name"];
                Console.WriteLine($"Villain : {villName}");

                string findMinionsQuery = File.ReadAllText("../../findTheMinion.sql");
                SqlCommand findMinionsCommand = new SqlCommand(findMinionsQuery, connection);
                SqlParameter param = new SqlParameter("@inputNum", inputNum);
                findMinionsCommand.Parameters.Add(param);
                reader.Close();

                SqlDataReader minionsRead = findMinionsCommand.ExecuteReader();
                int index = 1;
                while (minionsRead.Read())
                {
                    string nameOfMinion = (string)minionsRead["name"];
                    int ageOfMinion = (int)minionsRead["age"];
                    Console.WriteLine($"{index}. {nameOfMinion} {ageOfMinion}");
                    index++;
                }
            }
            else
            {
                Console.WriteLine("No villain with your ID exists in the database.");
                //reader.Close();
            }
            //reader.Close();
        }
        private static void GetVillainsNames(SqlConnection connection)
        {
            string queryGetVillains = File.ReadAllText("../../2GetVillainsNames.sql");
            SqlCommand command = new SqlCommand(queryGetVillains, connection);
             
            SqlDataReader reader = command.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    string villainName = (string)reader["Name"];
                    int countSubordinates = (int)reader["MinionsCount"];

                    Console.WriteLine($"{villainName} {countSubordinates}");
                }
            }
        }
        private static void CreateTables(SqlConnection connection)
        {
            string createQuery = File.ReadAllText(@"../../../DB_Create.sql");
            SqlCommand createTablesCommand = new SqlCommand(createQuery, connection);
            Console.WriteLine(createTablesCommand.ExecuteNonQuery());
        }

    }
}
