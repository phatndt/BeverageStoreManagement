using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BeverageStoreManagement.Validations;

namespace BeverageStoreManagement.Test
{
    [TestFixture]
    class EmployeeTestDateStartWorkingFunction
    {
        EmployeeDALValidation employeeDALValidation = new EmployeeDALValidation();
        [Test]
        public void TestCase1()
        {
            Assert.AreEqual(true, employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse("20/09/2001"), DateTime.Parse("21/09/2021")));
            Assert.AreEqual(true, employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse("1/01/2001"), DateTime.Parse("21/09/2021")));
            Assert.AreEqual(true, employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse("31/12/2001"), DateTime.Parse("21/09/2022")));
        }
        [Test]
        public void TestCase2()
        {
            Assert.AreEqual(false, employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse("22/09/2021"), DateTime.Parse("21/09/2021")));
            Assert.AreEqual(false, employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse("1/01/3000"), DateTime.Parse("21/09/2021")));
            Assert.AreEqual(false, employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse("31/12/2020"), DateTime.Parse("21/09/2000")));
        }
    }
}
