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
    class ConvertIntToType
    {
        BaseViewModel baseViewModel = new BaseViewModel();
        [Test]
        public void TestCase1()
        {
            Assert.AreEqual("Coffee", baseViewModel.ConvertIntToType(1));
        }
        [Test]
        public void TestCase2()
        {
            Assert.AreEqual("Tea", baseViewModel.ConvertIntToType(2));
        }
        [Test]
        public void TestCase3()
        {
            Assert.AreEqual("Milk Tea", baseViewModel.ConvertIntToType(3));
        }
        [Test]
        public void TestCase4()
        {
            Assert.AreEqual("Fruit Tea", baseViewModel.ConvertIntToType(4));
        }
        [Test]
        public void TestCase5()
        {
            Assert.AreEqual("Snacks", baseViewModel.ConvertIntToType(5));
        }
        [Test]
        public void TestCase6()
        {
            Assert.AreEqual("", baseViewModel.ConvertIntToType(0));
        }
        [Test]
        public void TestCase7()
        {
            Assert.AreEqual("", baseViewModel.ConvertIntToType(null));
        }
    }
}
