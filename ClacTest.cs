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

        [TestCase]
        public void TestConv()
        {
            Assert.AreEqual(1, calc.convr(2.54));
        }

        [TestCase]
        public void TestMax()
        {
            double[] mas = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };

            Assert.AreEqual(1, calc.max(mas));
        }

        [TestCase]
        public void TestMod()
        {
            // получение исключения
            var exception = Assert.Throws<ArgumentException>(() => calc.mod(2, 0));
            // сравнение полученного сообщения с ожидаемым
            Assert.That(exception.Message, Is.EqualTo("Делитель должен быть >= 0"));
            // проверка выполняется успешно, если исключение не было сгенерировано
            Assert.DoesNotThrow(() => calc.mod(2, 1));
        }

    }
}
