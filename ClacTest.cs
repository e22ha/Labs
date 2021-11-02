using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNa5
{
    // атрибут, указывающий на то, что это класс содержит тесты
    [TestFixture]
    class ClacTest
    {
        Calc calc = new();
        [TestCase]
        public void TestIsEven()
        {
            // проверка возвращаемого значения
            // в первом случае оно должно быть истинно, во втором ложно
            Assert.IsTrue(calc.Even(4));
            Assert.IsFalse(calc.Even(5));
        }
    }
}
