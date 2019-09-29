using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees
{
    /// <summary>
    /// .NET library assembly(DLL) for a system that handles the employees hierarchy.
    /// Developed by: Promaster Guru
    /// Date: 29/09/2019
    /// </summary>
    class Info
    {
        public string manager { get; set;}
        public long salary { get; set;}

        public Info(string mngr, long sal)
        {
            this.manager = mngr;
            this.salary = sal;
        }
    }
    public class Employee
    {
        //Variables
        String line;
        StreamReader streamReader;
        Dictionary<string, Info> employees = new Dictionary<string, Info>();

        public Employee(String fileName)
       {
            try
            {
                //StreamReader to read file data
                streamReader = new StreamReader(fileName);
                while ((line = streamReader.ReadLine()) != null)
                {
                    String[] data = line.Split(',');

                    //Validate salary
                    if (isSalaryValid(data[2]))
                    {
                        employees.Add(data[0], new Info(data[1], int.Parse(data[2])));
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid interger numbers for salaries");
                    }
                }
                streamReader.Close();
                //Validate employee reporting to managers function
                validateCEO();

                //Valiate circular reference function
                validateCircularReference();

                //Validate manager function
                validateManager();
            }
            catch(ArgumentException ex) {
                throw new ArgumentException("Employee cannot report to more than one manager.");
            }
       }

        //Salary has valid interger numbers
        bool isSalaryValid(string sal)
        {
            int val;
            return int.TryParse(sal,out val);
        }

        //There is only one CEO, i.e, only one employee with no manager
        void validateCEO()
        {
            int counter = 0;
            foreach (KeyValuePair<string, Info> info in employees){
                if(info.Value .manager.Equals(""))
                {
                    counter += 1;
                    if(counter > 1)
                    {
                        throw new InvalidDataException("There must be only one CEO.");
                    }
                }
            }
        }

        //There is no circular reference
        void validateCircularReference()
        {
            foreach (KeyValuePair<string, Info> emp in employees)
            {
                String employee = emp.Key;
                String manager = emp.Value.manager;
                foreach (KeyValuePair<string, Info> reference in employees)
                {
                    if (reference.Value.manager.Equals(employee) && reference.Key.Equals(manager))
                    {
                        throw new InvalidDataException("There should be no circular reference.");
                    }
                }
            }
        }

        //There is no manager that is not an employee
        void validateManager()
        {
            foreach (KeyValuePair<string, Info> mngr in employees)
            {
                if (employees.ContainsKey(mngr.Value.manager) || mngr.Value.manager.Equals(""))
                {
                }
                else
                {
                    throw new InvalidDataException("There should be no manager that is not an employee.");
                }
            }
        }

        //Instance method that returns the salary budget from the specified manager
        public long getSalary( string manager)
        {
            List<long> child = new List<long>();
            HashSet<string> keys = new HashSet<string>();
            foreach (KeyValuePair<string, Info> mngr in employees)
            {
                if (mngr.Key.Equals(manager) || mngr.Value.manager.Equals(manager))
                {
                    keys.Add(mngr.Key);
                    foreach (KeyValuePair<string, Info> data in employees)
                    {
                        if (data.Value.manager.Equals(mngr.Key))
                        {
                            keys.Add(data.Key);
                        }
                    }
                }              
            }
            foreach(string key in keys){
                child.Add(employees[key].salary);
            }
            return child.Sum();
        }
    }
}
