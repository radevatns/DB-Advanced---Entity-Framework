
namespace Introduction_EF_test
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    class Program
    {
        static void Main(string[] args)
        {
            SoftuniContext context = new SoftuniContext();
            Console.BufferHeight = 380;
            Console.WriteLine("Please make your choice. Enter num from 1 do 19");
            Console.WriteLine("Select an option:");
            Console.WriteLine("3.->Employees full information");
            Console.WriteLine("4.->Employees with Salary Over 50000");
            Console.WriteLine("5.->Employees from Seattle");
            Console.WriteLine("6.->Adding a New Address and Updating Employee");
            Console.WriteLine("7.->Find employees in period");
            Console.WriteLine("8.->Addresses by town name");
            Console.WriteLine("9.->Employee with id 147");
            Console.WriteLine("10.->Departments with more than 5 employees");
            int input = 999;
            try
            {
                input = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Type a number form 1 to 10 not different char");
            }
            switch (input)
            {
                case 3: EmployeeFullInfo(context); break;
                case 4: EmployeeWithSalaryOver50000(context); break;
                case 5: EmployeesFromSeattle(context); break;
                case 6: AddingNewAddress(context); break;
                case 7: FindEmployeesInPeriod(context); break;
                case 8: AddressByTownName(context); break;
                case 9: EmployeeWithId147(context); break;
             //   case 10:
             //       DepartmentsWithMoreThan5Employees(context); break;
             }
        }


        private static void EmployeeFullInfo(SoftuniContext context)//3
        {
            
            List<Employee> employee = context.Employees.ToList();
            foreach (var oneEmployee in employee)
            {
                Console.WriteLine($"{oneEmployee.FirstName} {oneEmployee.LastName} {oneEmployee.MiddleName} {oneEmployee.JobTitle} {oneEmployee.Salary}");
            }
        }
        private static void EmployeeWithSalaryOver50000(SoftuniContext context)//4
        {
            List<String> employeeNames = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e =>e.FirstName)
                .ToList();
            foreach (string employeeName in employeeNames)
            {
                Console.WriteLine(employeeName);
            }

            //option 2 with LINQ
            // List<Employee> employee = context.Employees
            //     .Where(e => e.Salary > 50000)
            //     .ToList();
            //foreach (var oneEmployee in employee)
            // {
            //     Console.WriteLine(oneEmployee.FirstName);
            // }

            // option 1 without LINQ
            //foreach (var oneEmployee in employee)
            //{
            //    if (oneEmployee.Salary>50000)
            //    {
            //        Console.WriteLine($"{oneEmployee.FirstName}");
            //    }
            //}
        }
        private static void EmployeesFromSeattle(SoftuniContext context)//5
        {
            List<Employee> employee = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();
            foreach (var oneEmployee in employee)
            {
                Console.WriteLine($"{oneEmployee.FirstName} {oneEmployee.LastName} from Research and Development - ${oneEmployee.Salary:f2}");
            }
        }
        private static void AddingNewAddress(SoftuniContext context)//6
        {
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownID = 4
            };
            Employee employee = null;
            employee = context.Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();

            employee.Address = newAddress; 
            context.SaveChanges();

            List<string> employeesNewAddress = context.Employees
                .OrderByDescending(e => e.AddressID)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            foreach (var oneAddress in employeesNewAddress)
            {
                Console.WriteLine(oneAddress);
            }
       }
        private static void FindEmployeesInPeriod(SoftuniContext context)//7
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); // English - US
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US"); 

            List<Employee> employee = context.Employees
                .Where(e => e.Projects.Count(p=>p.StartDate.Year >= 2001 && p.StartDate.Year <=2003)>0)
                .Take(30)
                .ToList();
            foreach (var oneEmployee in employee)
            {
                Console.WriteLine($"{ oneEmployee.FirstName} {oneEmployee.LastName} {oneEmployee.Manager.FirstName}");
                foreach (var proj in oneEmployee.Projects)
                {
                    Console.WriteLine($"--{proj.Name} {proj.StartDate} {proj.EndDate}");
                }
            } 
        }
        private static void AddressByTownName(SoftuniContext context)
        {
            List <Address> address = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(t=> t.Town.Name)
                .Take(10)
                .ToList();

            foreach (var oneAddress in address)
            {
                Console.Write($"{oneAddress.AddressText}, {oneAddress.Town.Name}");
                Console.WriteLine($" - {oneAddress.Employees.Count} employees");
            }   
        }
        private static void EmployeeWithId147(SoftuniContext context)
        {
            Employee employee = context.Employees
                .Where(e => e.EmployeeID == 147)
                .FirstOrDefault();

            Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.JobTitle}");

            List<Project> projects = new List<Project>();
            foreach (var proj in employee.Projects)
            {
                projects.Add(proj);
            }
            var projs = projects.OrderBy(p => p.Name);

            foreach (Project p in projs)
            {
                Console.WriteLine(p.Name);
            }

        }
    }
}
