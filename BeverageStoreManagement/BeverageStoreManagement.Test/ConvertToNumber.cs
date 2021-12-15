using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BeverageStoreManagement.ViewModels;

namespace BeverageStoreManagement.Test
{
    [TestFixture]
    class ConvertToNumber
    {
        BaseViewModel baseViewModel = new BaseViewModel();
        [Test]
        public void testCase1()
        {
            Assert.AreEqual(3, baseViewModel.ConvertToNumber("Tea"));
        }
        [Test]
        public void testCase2()
        {
            Assert.AreEqual(353464590, baseViewModel.ConvertToNumber("353464590"));
        }
        [Test]
        public void testCase3()
        {
            Assert.AreEqual(100000000, baseViewModel.ConvertToNumber("100000000"));
        }
        [Test]
        public void testCase4()
        {
            Assert.AreEqual(0, baseViewModel.ConvertToNumber(""));
        }
        [Test]
        public void testCase5()
        {
            Assert.AreEqual(5, baseViewModel.ConvertToNumber(null));
        }
    }
}
