using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BeverageStoreManagement;
namespace BeverageStoreManagement.Test
{
    [TestFixture]
    class Coverter
    {
        [TestCase("", ExpectedResult = "D41D8CD98F00B204E9800998ECF8427E")]
        [TestCase("1", ExpectedResult = "C4CA4238A0B923820DCC509A6F75849B")]
        public string TestMD5Hash(string password)
        {
            return Converter.Instance.MD5Hash(password);
        }
    }
}
