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
        
        [TestCase]
        public void TestIsEven()
        {
            // проверка возвращаемого значения
            // в первом случае оно должно быть истинно, во втором ложно
            Assert.IsTrue(Calc.Even(4));
            Assert.IsFalse(Calc.Even(5));

        }

        [TestCase]
        public void TestConv()
        {
            Assert.AreEqual(1, Calc.Convr(2.54));

            // получение исключения
            var exception = Assert.Throws<ArgumentException>(() => Calc.Convr(-1));
            // сравнение полученного сообщения с ожидаемым
            Assert.That(exception.Message, Is.EqualTo("The number must be > 0"));
            
        }

        [TestCase]
        public void TestMax()
        {
            double[] mas = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            Assert.AreEqual(1, Calc.Max(mas));

            // получение исключения
            mas = new double[] {  };
            var exception = Assert.Throws<ArgumentException>(() => Calc.Max(mas));
            // сравнение полученного сообщения с ожидаемым
            Assert.That(exception.Message, Is.EqualTo("The count must be > 0"));

            mas = new double[] { double.MinValue  };
            Assert.AreEqual(double.MinValue, Calc.Max(mas));
        }

        [TestCase]
        public void TestMod()
        {
            // получение исключения
            var exception = Assert.Throws<ArgumentException>(() => Calc.Mod(2, 0));
            // сравнение полученного сообщения с ожидаемым
            Assert.That(exception.Message, Is.EqualTo("The divisor must be > 0"));
            // проверка выполняется успешно, если исключение не было сгенерировано
            Assert.DoesNotThrow(() => Calc.Mod(2, 1));
        }

        [TestCase]
        public void TestReturnMod()
        {
            Assert.AreEqual(1, Calc.Mod(1,2));
            Assert.AreEqual(0, Calc.Mod(double.MaxValue,1));
            Assert.AreEqual(0, Calc.Mod(double.MinValue,1));
            Assert.AreEqual(-2, Calc.Mod(-8,3));
        }
    }
}
