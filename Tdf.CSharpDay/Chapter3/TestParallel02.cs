using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter3
{
    public class TestParallel02
    {
        public static void TestMethod1()
        {
            Thread.Sleep(3000);
            var swTask = new Stopwatch();
            swTask.Start();
            // 执行并行操作
            Parallel.Invoke(SetProcuct1_500, SetProcuct2_500, SetProcuct3_500, SetProcuct4_500);
            swTask.Stop();
            Console.WriteLine("500条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            Thread.Sleep(3000);  // 防止并行操作与顺序操作冲突
            var sw = new Stopwatch();
            sw.Start();
            SetProcuct1_500();
            SetProcuct2_500();
            SetProcuct3_500();
            SetProcuct4_500();
            sw.Stop();
            Console.WriteLine("500条数据  顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            Thread.Sleep(3000);
            swTask.Restart();

            // 执行并行操作
            Parallel.Invoke(SetProcuct1_10000, SetProcuct2_10000, SetProcuct3_10000, SetProcuct4_10000);
            swTask.Stop();
            Console.WriteLine("10000条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            Thread.Sleep(3000);
            sw.Restart();
            SetProcuct1_10000();
            SetProcuct2_10000();
            SetProcuct3_10000();
            SetProcuct4_10000();
            sw.Stop();
            Console.WriteLine("10000条数据 顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static void SetProcuct1_500()
        {
            var productList = new List<Product>();
            for (var index = 1; index < 500; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct1 执行完成");
        }

        private static void SetProcuct2_500()
        {
            var productList = new List<Product>();
            for (var index = 500; index < 1000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成");
        }

        private static void SetProcuct3_500()
        {
            var productList = new List<Product>();
            for (var index = 1000; index < 2000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct3 执行完成");
        }

        private static void SetProcuct4_500()
        {
            var productList = new List<Product>();
            for (var index = 2000; index < 3000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct4 执行完成");
        }

        private static void SetProcuct1_10000()
        {
            var productList = new List<Product>();
            for (var index = 1; index < 20000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct1 执行完成");
        }

        private static void SetProcuct2_10000()
        {
            var productList = new List<Product>();
            for (var index = 20000; index < 40000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成");
        }

        private static void SetProcuct3_10000()
        {
            var productList = new List<Product>();
            for (var index = 40000; index < 60000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct3 执行完成");
        }

        private static void SetProcuct4_10000()
        {
            var productList = new List<Product>();
            for (var index = 60000; index < 80000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
            }
            Console.WriteLine("SetProcuct4 执行完成");
        }
    }
}


/*
 * SetProcuct1 执行完成
SetProcuct2 执行完成
SetProcuct3 执行完成
SetProcuct4 执行完成
500条数据 并行编程所耗时间:60
SetProcuct1 执行完成
SetProcuct2 执行完成
SetProcuct3 执行完成
SetProcuct4 执行完成
500条数据  顺序编程所耗时间:6
SetProcuct1 执行完成
SetProcuct4 执行完成
SetProcuct3 执行完成
SetProcuct2 执行完成
10000条数据 并行编程所耗时间:57
SetProcuct1 执行完成
SetProcuct2 执行完成
SetProcuct3 执行完成
SetProcuct4 执行完成
10000条数据 顺序编程所耗时间:72

 但是在操作500条数据时，显然采用并行操作并不明智，并行所带来的损耗比较大，在实际的开发中，还是要注意下是否有必要进行并行编程。


 */
