using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employees;
using System;

namespace UnitTests
{
    [TestClass]
    public class EployeesLibraryUnitests
    {
        Employee employee;

        [TestMethod]
        public void Test_Salaries_Must_Be_Valid_Integers()
        {
            try
            {
                employee = new Employee(@"TestEmployeesFile1.csv");
            }
            catch(System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "Invalid interger numbers for salaries");
            }
        }

        [TestMethod]
        public void Test_One_Employee_Reports_To_One_Manager()
        {
            try
            {
                employee = new Employee(@"TestEmployeesFile2.csv");
            }
            catch (System.ArgumentException e)
            {
                StringAssert.Contains(e.Message, "Employee cannot report to more than one manager.");
            }
        }

        [TestMethod]
        public void Test_There_Is_Only_One_CEO()
        {
            try
            {
                employee = new Employee(@"TestEmployeesFile3.csv");
            }
            catch (System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "There must be only one CEO.");
            }
        }

        [TestMethod]
        public void Test_No_Circular_Reference()
        {
            try
            {
                employee = new Employee(@"TestEmployeesFile4.csv");
            }
            catch(System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "There should be no circular reference.");
            }
        }

        [TestMethod]
        public void Test_No_Manager_Is_Not_an_Employee()
        {
            try
            {
                employee = new Employee(@"TestEmployeesFile5.csv");
            }
            catch (System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "There should be no manager that is not an employee.");
            }
        }

        [TestMethod]
        public void Test_Can_View_Manager_Budget()
        {
            employee = new Employee(@"TestEmployeesFile6.csv");
            Assert.AreEqual(3800, employee.getSalary("Employee1"));
            Assert.AreEqual(1800, employee.getSalary("Employee2"));
            Assert.AreEqual(500, employee.getSalary("Employee3"));
            Assert.AreEqual(500, employee.getSalary("Employee4"));
            Assert.AreEqual(500, employee.getSalary("Employee5"));
            Assert.AreEqual(500, employee.getSalary("Employee6"));
        }
    }
}