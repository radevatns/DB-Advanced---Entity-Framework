
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
        public static object ExploreTheFullSourceCodeOfEF { get; private set; }

        static void Main(string[] args)
        {
            SoftuniContext context = new SoftuniContext();
            Console.BufferHeight = 380;
            Console.WriteLine("Please make your choice. Enter num from 1 do 17");
            Console.WriteLine("Select an option:");
            Console.WriteLine("3.->Employees full information");
            Console.WriteLine("4.->Employees with Salary Over 50000");
            Console.WriteLine("5.->Employees from Seattle");
            Console.WriteLine("6.->Adding a New Address and Updating Employee");
            Console.WriteLine("7.->Find employees in period");
            Console.WriteLine("8.->Addresses by town name");
            Console.WriteLine("9.->Employee with id 147");
            Console.WriteLine("10.->Departments with more than 5 employees");
            Console.WriteLine("11.->Find Latest 10 Projects");
            Console.WriteLine("12.->Increase Salaries");
            Console.WriteLine("13.->Find Employees by First Name starting with SA");
            Console.WriteLine("14.->First Letter");
            Console.WriteLine("15.->Delete Project by Id");
            Console.WriteLine("16.->Remove Towns");
            Console.WriteLine("17.->Native SQL Query");
            Console.WriteLine("18.->ExploreTheFullSourceCodeOfEF(context)");
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
                case 10: DepartmentsWithMoreThan5Employees(context); break;
                case 11: FindLatest10Projects(context); break;
                case 12: IncreaseSalaries(context); break;
                case 13: FindEmployeesWhithSA(context); break;
                case 14: FirstLetter(); break;
                case 15: DeleteProjectById(context); break;
                case 16: RemoveTowns(context); break;
                case 17: NativeSQLQuery(context); break;
                case 18: NativeSQLQuery(context); break;

                default: break;
            }
        }

        private static void NativeSQLQuery(SoftuniContext context)
        {
            Console.WriteLine("This is problem with star -->> '*' ");
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
            //option 1
            Address address = new Address();
            address.AddressText = "Vitoshka 15";
            address.AddressID = 4;

            context.Addresses.Add(address);
            context.SaveChanges();

            Employee emp = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
            emp.Address = address;
            context.SaveChanges();
             
            var addresses = context.Employees.OrderByDescending(e => e.AddressID)
                .Take(10)
                .ToList();


            //option 2

        //    Address newAddress = new Address()
        //    {
        //        AddressText = "Vitoshka 15",
        //        TownID = 4
        //    };
        //    Employee employee = null;
        //    employee = context.Employees
        //        .Where(e => e.LastName == "Nakov")
        //        .FirstOrDefault();

        //    employee.Address = newAddress; 
        //    context.SaveChanges();

        //    List<string> employeesNewAddress = context.Employees
        //        .OrderByDescending(e => e.AddressID)
        //        .Take(10)
        //        .Select(e => e.Address.AddressText)
        //        .ToList();

        //    foreach (var oneAddress in employeesNewAddress)
        //    {
        //        Console.WriteLine(oneAddress);
        //    }
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
        private static void AddressByTownName(SoftuniContext context)//8
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
        private static void EmployeeWithId147(SoftuniContext context)//9
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
        private static void DepartmentsWithMoreThan5Employees(SoftuniContext context)//10
        {
            List<Department> department = context.Departments
                .Where(e => e.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ToList();

            foreach (var oneDepartment in department)
            {
                Console.WriteLine($"{oneDepartment.Name} {oneDepartment.Employee.FirstName}");
                foreach (var oneEmployee in oneDepartment.Employees)
                {
                    Console.WriteLine($"{oneEmployee.FirstName} {oneEmployee.LastName} {oneEmployee.JobTitle}");
                }
            }
        }
        private static void FindLatest10Projects(SoftuniContext context)//11
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); // English - US
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            List<Project> project = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();
            foreach (var oneProject in project)
            {
                Console.WriteLine($"{oneProject.Name} {oneProject.Description} {oneProject.StartDate} {oneProject.EndDate}");
                Console.WriteLine($"{oneProject.Name} {oneProject.Description} {oneProject.StartDate:M/d/yyyy h:mm:ss tt} {oneProject.EndDate}");
            }
        }
        private static void IncreaseSalaries(SoftuniContext context)//12
        {
            List<Employee> employee = context.Employees
                .Where(e=>e.Department.Name == "Engineering" 
                || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing"
                || e.Department.Name == "Information Services")
                .ToList();
            foreach (var oneEmployee in employee)
            {
                //oneEmployee.Salary = oneEmployee.Salary * 1.12m;
                Console.WriteLine($"{oneEmployee.FirstName} {oneEmployee.LastName} (${oneEmployee.Salary})");

                //context.SaveChanges(); // to save data base
            }
        }//12
        private static void FindEmployeesWhithSA(SoftuniContext context)//13
        {
            List<Employee> employee = context.Employees
                .Where(e => e.FirstName.StartsWith("SA"))
                .ToList();
            foreach (var oneEmployee in employee)
            {
                Console.WriteLine($"{oneEmployee.FirstName} {oneEmployee.LastName} - {oneEmployee.JobTitle} - (${oneEmployee.Salary:f4})");
            }
        }//13
        private static void FirstLetter()//14
        {
            
            GringotsContext gringoContext = new GringotsContext();

            var letters = gringoContext.WizzardDeposits
               .Where(w => w.DepositGroup == "Troll Chest")
               .Select(w => w.FirstName)
               .ToList()
               .Select(fchar => fchar[0])
               .Distinct()
               .OrderBy(c => c);
            
                foreach (var letter in letters)
            {
                Console.WriteLine(letter);
            } 
        }//14
        private static void DeleteProjectById(SoftuniContext context)//15
        {
            var project = context.Projects.Find(2);
            
            foreach (var oneEmployee in project.Employees)
            {
                oneEmployee.Projects.Remove(project);
            }
            
            context.Projects.Remove(project);
            context.SaveChanges();
            
            List<Project> projects = context.Projects
                .Take(10)
                .ToList();
            foreach (var oneProject in projects)
            {
                Console.WriteLine(oneProject.Name);
            }
        }//15
        private static void RemoveTowns(SoftuniContext context)//16
        {// to do check
            Console.Write("Enter town to delete: ");
            string town = Console.ReadLine();

            Town tn = context.Towns
                .Where(t => t.Name == town)
                .FirstOrDefault();

            List<Address> addrs = context.Addresses
                .Where(a => a.Town.Name == town)
                .ToList();
            
            context.Towns.Remove(tn);
            foreach (Address a in addrs)
            {
                List<Employee> empls = context.Employees
                    .Where(e => e.AddressID == a.AddressID)
                    .ToList();

                foreach (Employee e in empls)
                {
                    e.AddressID = null;
                }
                context.Addresses.Remove(a);
            }
            context.SaveChanges();
            if (addrs.Count == 1)
            {
                Console.WriteLine($"{addrs.Count} " +
                    $"address in {town} was deleted");
            }
            else
            {
                Console.WriteLine($"{addrs.Count} " +
                    $"addresses in {town} were deleted");
            }
        }//16
        
    }
}
