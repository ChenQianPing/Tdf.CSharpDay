using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter9
{
    public class AsynSample01
    {
        public static void TestMethod1()
        {
            FileReader reader = new FileReader(1024);

            // 改为自己的文件路径
            var path = "E:\\src\\Tdf.CSharpDay\\Tdf.CSharpDay\\Chapter9\\Bearer.txt";

            Console.WriteLine("开始读取文件了...");
            reader.SynsReadFile(path);

            Console.WriteLine("我这里还有一大滩事呢.");
            DoSomething();
            Console.WriteLine("终于完事了，输入任意键，歇着！");
            Console.ReadKey();
        }

        public static void TestMethod2()
        {
            FileReader reader = new FileReader(1024);

            // 改为自己的文件路径
            var path = "E:\\src\\Tdf.CSharpDay\\Tdf.CSharpDay\\Chapter9\\Bearer.txt";

            Console.WriteLine("开始读取文件了...");

            reader.AsynReadFile(path);

            Console.WriteLine("我这里还有一大滩事呢.");
            DoSomething();
            Console.WriteLine("终于完事了，输入任意键，歇着！");
            Console.ReadKey();
        }

        static void DoSomething()
        {
            Thread.Sleep(1000);
            for (var i = 0; i < 10000; i++)
            {
                if (i % 888 == 0)
                {
                    Console.WriteLine("888的倍数：{0}", i);
                }
            }
        }
    }
}
