using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter8
{
    public class DynamicDemo01
    {
        public static void TestMethod1()
        {
            dynamic obj = new DynamicTest();
            var name = "Bobby";
            obj.Welcome(name);

            Console.WriteLine("Input any key to exit.");
            Console.ReadKey();
        }

        public static void TestMethod2()
        {
            var obj = new DynamicTest();
            var name = "Samba";
            obj.Welcome(name);

            Console.WriteLine("Input any key to exit.");
            Console.ReadKey();
        }
    }

    public class DynamicTest
    {
        public void Welcome(string name)
        {
            Console.WriteLine($"Hello {name},welcome to dynamic world.");
        }
    }
}

/*
 * Hello Bobby,welcome to dynamic world.
Input any key to exit.
 */
