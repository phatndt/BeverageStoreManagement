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
    class EmployeeTestCompareDateStartWorkingAnDateNowFunction
    {
        EmployeeDALValidation employeeDALValidation = new EmployeeDALValidation();
        [Test]
        public void TestCase1()
        {
            Assert.AreEqual(true, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse("21/09/2021")));
            Assert.AreEqual(true, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse("10/01/2020")));
            Assert.AreEqual(true, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse("31/12/2020")));
        }
        [Test]
        public void TestCase2()
        {
            Assert.AreEqual(false, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse("22/02/2022")));
            Assert.AreEqual(false, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse("10/01/3000")));
            Assert.AreEqual(false, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse("31/12/2222")));
        }
    }
}
