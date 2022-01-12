using BeverageStoreManagement.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Test
{
    [TestFixture]
    class ConvertBooleanToStatus
    {
        BaseViewModel baseViewModel = new BaseViewModel();
        [Test]
        public void TestCase1()
        {
            Assert.AreEqual("Available", baseViewModel.ConvertBooleanToStatus(true));
        }
        [Test]
        public void TestCase2()
        {
            Assert.AreEqual("Unavailable", baseViewModel.ConvertBooleanToStatus(false));
        }
        [Test]
        public void TestCase3()
        {
            Assert.AreEqual("", baseViewModel.ConvertIntToType(null));
        }
    }
}
