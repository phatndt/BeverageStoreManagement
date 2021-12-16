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
    class SeparateThousands
    {
        BaseViewModel baseViewModel = new BaseViewModel();

        [TestCase("123456789","123,456,789")]
        [TestCase("", "")]
        public void TestCase(string input, string output)
        {
            Assert.AreEqual(output, baseViewModel.SeparateThousands(input));
        }
    }
}
