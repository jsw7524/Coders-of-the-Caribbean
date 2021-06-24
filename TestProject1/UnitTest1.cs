using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Wood3;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var reader = new StreamReader("turn1.txt"))
            {
                Console.SetIn(reader);
                Player.Main(null);
            }
        }


        [TestMethod]
        public void TestMethod2()
        {
            using (var reader = new StreamReader("turn2.txt"))
            {
                Console.SetIn(reader);
                Player.Main(null);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            using (var reader = new StreamReader("turn3.txt"))
            {
                Console.SetIn(reader);
                Player.Main(null);
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            using (var reader = new StreamReader("turn4.txt"))
            {
                Console.SetIn(reader);
                Player.Main(null);
            }
        }
    }
}
